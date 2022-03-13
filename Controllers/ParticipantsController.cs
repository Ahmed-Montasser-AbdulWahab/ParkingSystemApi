using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Parking_System_API.Data.Entities;
using Parking_System_API.Data.Models;
using Parking_System_API.Data.Repositories.ConstantsR;
using Parking_System_API.Data.Repositories.ParticipantR;
using Parking_System_API.Data.Repositories.VehicleR;
using Parking_System_API.Hashing;
using Parking_System_API.Helper;
using Parking_System_API.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Parking_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly IParticipantRepository participantRepository;
        private readonly IConstantRepository constantRepository;
        private readonly JwtAuthenticationManager jwtAuthenticationManager;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;
        private readonly IVehicleRepository vehicleRepository;

        public ParticipantsController(IParticipantRepository participantRepository, IConstantRepository constantRepository, JwtAuthenticationManager jwtAuthenticationManager, IMapper mapper, LinkGenerator linkGenerator, IVehicleRepository vehicleRepository)
        {
            this.participantRepository = participantRepository;
            this.constantRepository = constantRepository;
            this.jwtAuthenticationManager = jwtAuthenticationManager;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
            this.vehicleRepository = vehicleRepository;
        }

        [HttpGet, Authorize(Roles = "admin, operator")]
        public async Task<ActionResult<ParticipantResponseModel[]>> GetAllParticipants()
        {
            try
            {
                var participants = await participantRepository.GetAllParticipants();
                if (participants.Length == 0)
                {
                    return NotFound(new { Error = "No Participants Exist" });

                }
                ParticipantResponseModel[] models = mapper.Map<ParticipantResponseModel[]>(participants);
                return models;
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }



        //[HttpGet("getParticipantByEmail/{email}")]
        //public async Task<ActionResult<ParticipantResponseModel>> GetParticipantByEmail(string email)
        //{
        //    try
        //    {
        //        var participant = await participantRepository.GetParticipantAsyncByEmail(email);
        //        if (participant == null)
        //        {
        //            return NotFound($"Participant with emai {email} Don't Exist");
        //        }

        //        var model = mapper.Map<ParticipantResponseModel>(participant);
        //        return model;
        //    }
        //    catch (Exception)
        //    {

        //        return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
        //    }
        //}


        [HttpPost("login"), AllowAnonymous]
        public async Task<IActionResult> login(AuthenticationRequest authenticationRequest)
        {
            try
            {
                var authResult = await jwtAuthenticationManager.AuthenticateCustomer(authenticationRequest.Email, authenticationRequest.Password);
                if (authResult == null)
                    return Unauthorized();
                else
                    return Ok(authResult);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }

        }


        [HttpPost(), Authorize(Roles = "operator, admin")]
        public async Task<ActionResult<ParticipantResponseModel>> AddParticipant([FromBody] ParticipantAdminModel model)
        {


            //Adding ParticipantId
            try
            {
                var participant = await participantRepository.GetParticipantAsyncByEmail(model.Email);
                if (participant != null)
                {
                    return BadRequest(new { Error = $"Participant with email {model.Email} already exists" });
                }
                if (model.Id != null)
                {
                    participant = await participantRepository.GetParticipantAsyncByID(model.Id.Value);
                    if (participant != null)
                    {
                        return BadRequest(new { Error = $"Participant with id {model.Id.Value} already exists" });
                    }
                }

                participant = new Participant() { Status = false, DoProvideFullData = true, DoProvidePhoto = false, DoDetected = false, PhotoUrl = ".\\wwwroot\\images\\Anonymous.jpg" };
                if (model.IsEgyptian)
                {
                    if (model.Id == null || model.Id < 2000000000000)
                    {
                        return BadRequest(new { Error = "Please provide National Id" });
                    }
                    else
                    {
                        participant.ParticipantId = model.Id.Value;
                    }

                }

                else if (!model.IsEgyptian)
                {
                    var Constant = await constantRepository.GetForeignIdAsync();

                    participant.ParticipantId = Constant.Value;
                    Constant.Value++;

                    if (!await constantRepository.SaveChangesAsync())
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
                    }

                }

                //Adding Email

                participant.Email = model.Email;

                //Adding Password and sending it Via Email
                var password = TokenGeneration.GenerateToken(8);

                participant.Salt = HashingClass.GenerateSalt();

                participant.Password = HashingClass.GenerateHashedPassword(password, participant.Salt);

                ////Checking Photo and detection
                //if (model.ProfileImage == null)
                //{
                //    participant.DoProvidePhoto = false;
                //    participant.Status = false;
                //}
                //else
                //{
                //    participant.DoProvidePhoto = true;
                //    //go to model and detection "Muhammad Samy"
                //    /*
                //     * 
                //     * 
                //     * 
                //     * 
                //     */
                //    participant.DoDetected = true;
                //}

                //checking name
                if (model.Name is null)
                {

                    participant.DoProvideFullData = false;

                }
                else

                {
                    participant.Name = model.Name;
                }

                //Adding Vehicles

                if (model.PlateNumberIds is null || model.PlateNumberIds.Count == 0) //Null or Empty
                {
                    participant.DoProvideFullData = false;
                }
                else
                {
                    foreach (var v in model.PlateNumberIds)
                    {
                        var Vehicle = await vehicleRepository.GetVehicleAsyncByPlateNumber(v);
                        if (Vehicle == null)
                        {
                            return BadRequest(new { Error = $"No Vehicle saved with the provided License Plate {v}" });
                        }
                        else
                        {
                            participant.Vehicles = new List<Vehicle>() { Vehicle };

                        }
                    }
                }

                //adding and saving
                participantRepository.Add(participant);
                if (!await participantRepository.SaveChangesAsync())
                {
                    return BadRequest("Participant Not Saved");
                }

                Email.EmailCode.SendEmail(participant.Email, password);
                var response_model = mapper.Map<ParticipantResponseModel>(participant);
                return Created(linkGenerator.GetPathByAction("GetParticipant", "Participants", new { id = participant.ParticipantId }), new { Participant = response_model, Message = "Please Add a photo" });

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error {ex}");
            }


        }

        [HttpPost("uploadProfilePicture"), Authorize(Roles = "admin,operator")]
        public async Task<IActionResult> UploadProfilePicture([FromForm] UploadPicture upload)
        {//DAMANA
            try
            {
                var participant = await participantRepository.GetParticipantAsyncByID(upload.Id);
                if (participant is null)
                {
                    return BadRequest(new { Error = $"Participant of Id {upload.Id} doesn't Exist." });
                }
                var pic = upload.Picture;
                if (pic.ContentType != "image/jpeg")
                {
                    return BadRequest(new { Error = $"Please Upload JPG File." });
                }
                var path = $".\\wwwroot\\images\\Participants\\{upload.Id}.jpg";

                //Connection Lost ??? 

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await pic.CopyToAsync(stream);
                }


                participant.DoProvidePhoto = true;
                participant.PhotoUrl = $".\\wwwroot\\images\\Participants\\{upload.Id}.jpg";

                participant.DoDetected = true;
                //Muhammed Samy
                /*
                 * 
                 * 
                 * 
                 * 
                 * 
                 * 
                 * 
                 * 
                 * 
                 * 
                 * 
                 * 
                 */
                //   string result = await FaceDetectionApi.Detect(upload.Id, upload.pic);


                if (participant.DoProvideFullData && participant.DoProvidePhoto && participant.DoDetected)
                {
                    participant.Status = true;
                }

                if (!await participantRepository.SaveChangesAsync())
                {
                    return BadRequest(new { Error = "Try Again adding Photo" });
                }
                var response_model = mapper.Map<ParticipantResponseModel>(participant);
                return Created("", new { Participant = response_model, Message = "Photo is Created" });

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error {ex}");
            }
        }


        [HttpPost("uploadMyProfilePicture"), Authorize(Roles = "customer")]
        public async Task<IActionResult> UploadProfilePictureForMe([FromForm] UploadMyPicture upload)
        {//DAMANA
            try
            {
                var id = User.Claims.First(i => i.Type == "ParticipantID").Value;
                var participant = await participantRepository.GetParticipantAsyncByID(long.Parse(id));
                if (participant is null)
                {
                    return BadRequest(new { Error = $"Participant of Id {id} doesn't Exist." });
                }
                var pic = upload.Picture;
                if (pic.ContentType != "image/jpeg")
                {
                    return BadRequest(new { Error = $"Please Upload JPG File." });
                }
                var path = $".\\wwwroot\\images\\Participants\\{id}.jpg";

                //Connection Lost ??? 

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await pic.CopyToAsync(stream);
                }


                participant.DoProvidePhoto = true;
                participant.PhotoUrl = $".\\wwwroot\\images\\Participants\\{id}.jpg";

                participant.DoDetected = true;
                //Muhammed Samy
                /*
                 * 
                 * 
                 * 
                 * 
                 * 
                 * 
                 * 
                 * 
                 * 
                 * 
                 * 
                 * 
                 */
                //   string result = await FaceDetectionApi.Detect(upload.Id, upload.pic);


                if (participant.DoProvideFullData && participant.DoProvidePhoto && participant.DoDetected)
                {
                    participant.Status = true;
                }
                else
                {
                    participant.Status = false;
                }

                if (!await participantRepository.SaveChangesAsync())
                {
                    return BadRequest(new { Error = "Try Again adding Photo" });
                }
                var response_model = mapper.Map<ParticipantResponseModel>(participant);
                return Created("", new { Participant = response_model, Message = "Photo is Created" });

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error {ex}");
            }
        }
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordModel model)
        {
            try
            {
                var participant = await participantRepository.GetParticipantAsyncByEmail(model.Email);
                if (participant is null)
                {
                    return BadRequest(new { Error = $"Participant of Email {model.Email} doesn't Exist." });
                }

                var password = TokenGeneration.GenerateToken(8);

                participant.Salt = HashingClass.GenerateSalt();

                participant.Password = HashingClass.GenerateHashedPassword(password, participant.Salt);
                if (!await participantRepository.SaveChangesAsync())
                {
                    return BadRequest(new { Error = "Try Again Later" });
                }
                Email.EmailCode.SendEmail(participant.Email, password, "Password Reset");

                return Ok(new { Message = "Check Reset Password in your Email Inbox" });
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error");
            }

        }
        [HttpGet("Participant/{id:long}"), Authorize(Roles = "admin, operator")]
        public async Task<ActionResult<ParticipantResponseModel>> GetParticipant(long id)
        {
            try
            {
                var participant = await participantRepository.GetParticipantAsyncByID(id);
                if (participant is null)
                {
                    return BadRequest(new { Error = $"Participant of Id {id} doesn't Exist." });
                }

                Byte[] b = System.IO.File.ReadAllBytes(participant.PhotoUrl);   // You can use your own method over here.         
                return mapper.Map<ParticipantResponseModel>(participant);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error {ex}");
            }
        }

        [HttpGet("me"), Authorize(Roles = "customer")]
        public async Task<ActionResult<ParticipantResponseModel>> GetMe()
        {
            try
            {
                var id = User.Claims.First(i => i.Type == "ParticipantID").Value;
                var participant = await participantRepository.GetParticipantAsyncByID(long.Parse(id));
                if (participant == null)
                {
                    return NotFound(new { Error = $"Participant with id {id} not Found" });

                }
                ParticipantResponseModel model = mapper.Map<ParticipantResponseModel>(participant);
                return model;
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }


        }
        [HttpPut("Participant/update-admin/{email}"), Authorize(Roles = "admin,operator")]
        public async Task<ActionResult<ParticipantResponseModel>> UpdateParticipantForAdmin(string email, [FromBody] ParticipantAdminModel model)
        {
            try
            {

                var participant = await participantRepository.GetParticipantAsyncByEmail(email, true);

                if (participant == null)
                {
                    return NotFound(new { Error = $"Participant with email {email} doesn't exist" });
                }


                if (model.Id != null)
                {
                    participant.ParticipantId = model.Id.Value;
                }
                if (model.Name != null)
                {
                    participant.Name = model.Name;
                }
                if (model.Email != null)
                {
                    var checkParticipant = await participantRepository.GetParticipantAsyncByEmail(model.Email);
                    if (checkParticipant != null)
                    {
                        return BadRequest(new { Error = $"Participant with email {model.Email} already exists" });
                    }

                    participant.Email = model.Email;
                }

                if (model.PlateNumberIds is not null && model.PlateNumberIds.Count > 0)
                {

                    foreach (var v in model.PlateNumberIds)
                    {
                        var Vehicle = await vehicleRepository.GetVehicleAsyncByPlateNumber(v);
                        if (Vehicle == null)
                        {
                            return BadRequest(new { Error = $"No Vehicle saved with the provided License Plate {v}" });
                        }
                        else
                        {
                            if (!participant.Vehicles.Contains(Vehicle))
                            {

                                participant.Vehicles.Add(Vehicle);
                            }
                        }
                    }


                }

                if (participant.Name is null || participant.Vehicles is null || participant.Vehicles.Count < 1)
                {

                    participant.DoProvideFullData = false;
                }
                else
                {
                    participant.DoProvideFullData = true;
                }

                if (participant.PhotoUrl == ".\\wwwroot\\images\\Anonymous.jpg")
                {
                    
                    participant.DoProvidePhoto = false;
                    participant.DoDetected = false;
                }
                else
                {
                    participant.DoProvidePhoto = true;
                }

                if (participant.DoProvidePhoto && participant.DoProvideFullData)
                {
                    if (participant.DoDetected)
                    {
                        participant.Status = true;
                    }
                    else
                    {
                        participant.Status = false;
                    }
                }
                else
                {
                    participant.Status = false;
                }


                if (!await participantRepository.SaveChangesAsync())
                {
                    return BadRequest(new { Error = "Updates Not Save" });
                }

                return mapper.Map<ParticipantResponseModel>(participant);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error {ex}");
            }
        }

        [HttpPut("Participant/updateById-admin/{id:long}"), Authorize(Roles = "admin,operator")]
        public async Task<ActionResult<ParticipantResponseModel>> UpdateParticipant(long id, [FromBody] ParticipantAdminModel model)
        {
            try
            {

                var participant = await participantRepository.GetParticipantAsyncByID(id, true);
                if (participant == null)
                {
                    return NotFound(new { Error = $"Participant with ID {id} doesn't exist" });
                }
                if (model.Id != null)
                {
                    participant.ParticipantId = model.Id.Value;
                }
                if (model.Name != null)
                {
                    participant.Name = model.Name;
                }
                if (model.Email != null)
                {
                    var checkParticipant = await participantRepository.GetParticipantAsyncByEmail(model.Email);
                    if (checkParticipant != null)
                    {
                        return BadRequest(new { Error = $"Participant with email {model.Email} already exists" });
                    }

                    participant.Email = model.Email;
                }

                if (model.PlateNumberIds is not null && model.PlateNumberIds.Count > 0)
                {

                    foreach (var v in model.PlateNumberIds)
                    {
                        var Vehicle = await vehicleRepository.GetVehicleAsyncByPlateNumber(v);
                        if (Vehicle == null)
                        {
                            return BadRequest(new { Error = $"No Vehicle saved with the provided License Plate {v}" });
                        }
                        else
                        {
                            if (!participant.Vehicles.Contains(Vehicle))
                            {

                                participant.Vehicles.Add(Vehicle);
                            }
                        }
                    }


                }

                if (participant.Name is null || participant.Vehicles is null || participant.Vehicles.Count < 1)
                {

                    participant.DoProvideFullData = false;
                }
                else
                {
                    participant.DoProvideFullData = true;
                }

                if (participant.PhotoUrl == ".\\wwwroot\\images\\Anonymous.jpg")
                {
                    participant.DoProvidePhoto = false;
                }
                else
                {
                    participant.DoProvidePhoto = true;
                }

                if (participant.DoProvidePhoto && participant.DoProvideFullData)
                {
                    participant.Status = true;
                }
                else
                {
                    participant.Status = false;
                }


                if (!await participantRepository.SaveChangesAsync())
                {
                    return BadRequest(new { Error = "Updates Not Save" });
                }

                return mapper.Map<ParticipantResponseModel>(participant);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error {ex}");
            }
        }


        [HttpPut("Participant/changeMyPassword"), Authorize(Roles = "participant")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            try
            {
                var email = User.Claims.First(i => i.Type == ClaimTypes.Email).Value;
                var participant = await participantRepository.GetParticipantAsyncByEmail(email);
                if (participant == null)
                {
                    return NotFound(new { Error = $"Participant with email {email} doesn't exist" });
                }

                var oldHashedPassword = HashingClass.GenerateHashedPassword(model.OldPassword, participant.Salt);

                if (oldHashedPassword != participant.Password)
                {
                    return BadRequest(new { Error = "Error is found" });
                }

                participant.Salt = HashingClass.GenerateSalt();

                participant.Password = HashingClass.GenerateHashedPassword(model.NewPassword, participant.Salt);


                if (!await participantRepository.SaveChangesAsync())
                {
                    return BadRequest(new { Error = "Updates Not Save" });
                }

                return Ok(new { message = "Password Changed Successfully" });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error {ex}");
            }
        }

        [HttpPut("Participant/changeMyPasswordById"), Authorize(Roles = "participant")]
        public async Task<IActionResult> ChangePasswordByID([FromBody] ChangePasswordModel model)
        {
            try
            {
                var id = User.Claims.First(i => i.Type == "ParticipantID").Value;
                var participant = await participantRepository.GetParticipantAsyncByID(long.Parse(id));
                if (participant == null)
                {
                    return NotFound(new { Error = $"Participant with id {id} doesn't exist" });
                }

                var oldHashedPassword = HashingClass.GenerateHashedPassword(model.OldPassword, participant.Salt);

                if (oldHashedPassword != participant.Password)
                {
                    return BadRequest(new { Error = "Error is found" });
                }

                participant.Salt = HashingClass.GenerateSalt();

                participant.Password = HashingClass.GenerateHashedPassword(model.NewPassword, participant.Salt);


                if (!await participantRepository.SaveChangesAsync())
                {
                    return BadRequest(new { Error = "Updates Not Save" });
                }

                return Ok(new { message = "Password Changed Successfully" });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error {ex}");
            }
        }





        [HttpPost("experiment")]
        public async Task<IActionResult> Experiment([FromBody] ExpModel model)
        {
            try
            {
                var participant = await participantRepository.GetParticipantAsyncByID(model.personID, true);
                if (participant == null)
                {
                    return NotFound(new { Error = $"Participant with id {model.personID} doesn't exist" });
                }

                var vehicle = participant.Vehicles.Where(c => c.PlateNumberId == model.plateNumber).FirstOrDefault();
                if (vehicle == null)
                {
                    return NotFound(new { Error = $"Vehicle with id {model.personID} doesn't exist" });
                }

                return Ok("Open Gate");

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error {ex}");
            }
        }
    }
}
