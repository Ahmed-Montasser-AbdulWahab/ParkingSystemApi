using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parking_System_API.Data.Entities;
using Parking_System_API.Data.Models;
using Parking_System_API.Data.Repositories.SystemUserR;
using Parking_System_API.Model;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Parking_System_API.Data.Repositories.VehicleR;
using Parking_System_API.Data.Repositories.ParticipantR;
using System.Linq;
using System.Security.Cryptography;
using Parking_System_API.Hashing;
using Parking_System_API.Data.Repositories.ConstantsR;
using System.Collections.Generic;
using Parking_System_API.Data.Repositories.RolesR;
using System.Security.Claims;
using System.IO;
using Parking_System_API.Helper;

namespace Parking_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemUsersController : ControllerBase
    {
        private readonly IRoleRepository roleRepository;
        private readonly IConstantRepository constantRepository;
        private readonly ISystemUserRepository systemUserRepository;
        private readonly JwtAuthenticationManager jwtAuthenticationManager;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IParticipantRepository participantRepository;
        // private static long ForeignMemberId = 10000000000000 ;

        public SystemUsersController(IRoleRepository roleRepository, IConstantRepository constantRepository, ISystemUserRepository systemUserRepository, JwtAuthenticationManager jwtAuthenticationManager, IMapper mapper, LinkGenerator linkGenerator, IVehicleRepository vehicleRepository, IParticipantRepository participantRepository)
        {
            this.roleRepository = roleRepository;
            this.constantRepository = constantRepository;
            this.systemUserRepository = systemUserRepository;
            this.jwtAuthenticationManager = jwtAuthenticationManager;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
            this.vehicleRepository = vehicleRepository;
            this.participantRepository = participantRepository;
        }
        [HttpPost("login"), AllowAnonymous]
        public async Task<IActionResult> login(AuthenticationRequest authenticationRequest)
        {
            try
            {
                var authResult = await jwtAuthenticationManager.AuthenticateAdminAndOpertor(authenticationRequest.Email, authenticationRequest.Password);
                if (authResult == null)
                    return Unauthorized(new { Error = "Email or Password may be incorrect" });
                else
                    return Ok(authResult);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }

        }

        [HttpPost("signup"), Authorize(Roles = "admin,operator")]
        public async Task<IActionResult> Signup([FromBody] SystemUserSignUpModel systemUserModel)
        {
            try
            {

                var checkSystemUser = await systemUserRepository.GetSystemUserAsyncByEmail(systemUserModel.Email);
                if (checkSystemUser != null)
                {
                    return BadRequest(new { Error = $"System User with {systemUserModel.Email} already exists" });

                }


                var location = linkGenerator.GetPathByAction("GetSystemUser", "SystemUsers", new { email = systemUserModel.Email });
                if (String.IsNullOrEmpty(location))
                {
                    return BadRequest(new { Error = "Try Again" });
                }
                if (!await roleRepository.RoleExistsAsync(systemUserModel.Role))
                {
                    return BadRequest(new { Error = $"({systemUserModel.Role}) role doesnot exist" });
                }


                var systemUser = mapper.Map<SystemUser>(systemUserModel);

                if (systemUserModel.Role.ToLower() == "admin")
                {
                    systemUser.IsAdmin = true;
                }
                else if (systemUserModel.Role.ToLower() == "operator")
                {
                    systemUser.IsAdmin = false;
                }


                var salt = HashingClass.GenerateSalt();
                var hashed = HashingClass.GenerateHashedPassword(systemUser.Password, salt);
                systemUser.Password = hashed;
                systemUser.Salt = salt;
                systemUser.IsPowerAccount = false;


                systemUserRepository.Add(systemUser);

                if (await systemUserRepository.SaveChangesAsync())
                {
                    return Created(location, mapper.Map<SystemUserResponseModel>(systemUser));
                }
                return BadRequest(new { Error = "Check Provided Data, i.e:Data may be duplicated" });
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }


        [HttpGet, Authorize(Roles = "admin")]
        public async Task<ActionResult<SystemUserResponseModel[]>> GetAllSystemUsers()
        {
            try
            {
                var systemUsers = await systemUserRepository.GetAllSystemUsersAsync();
                if (systemUsers.Length == 0)
                {
                    return NotFound(new { Error = "No SystemUsers Exist" });

                }
                SystemUserResponseModel[] models = mapper.Map<SystemUserResponseModel[]>(systemUsers);
                return models;
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
        [HttpGet("me"), Authorize(Roles = "admin")]
        public async Task<ActionResult<SystemUserResponseModel>> GetMe()
        {
            try
            {
                var email = User.Claims.First(i => i.Type == ClaimTypes.Email).Value;
                var systemUser = await systemUserRepository.GetSystemUserAsyncByEmail(email);
                if (systemUser == null)
                {
                    return NotFound(new { Error = $"System User with {systemUser.Email} not Found" });

                }
                SystemUserResponseModel model = mapper.Map<SystemUserResponseModel>(systemUser);
                return model;
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpGet("getByName/{name}"), Authorize(Roles = "admin")]
        public async Task<ActionResult<SystemUserResponseModel[]>> GetAllSystemUsersByName(string name)
        {
            try
            {
                var systemUsers = await systemUserRepository.GetSystemUsersAsyncByName(name);
                if (systemUsers.Length == 0)
                {
                    return NotFound(new { Error = $"No SystemUsers with name : {name} Exist" });

                }
                SystemUserResponseModel[] models = mapper.Map<SystemUserResponseModel[]>(systemUsers);
                return models;
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        [HttpPost("Participant"), Authorize(Roles = "operator, admin")]
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
                var password = GenerateToken(8);

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
                if (model.Name is null) {

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
                else
                {
                    Email.EmailCode.SendEmail(participant.Email, password);
                    var response_model = mapper.Map<ParticipantResponseModel>(participant);
                    return Created("", new { Participant = response_model, Message = "Please Add a photo" });
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error {ex}");
            }


        }

        [HttpPost("uploadProfile"), Authorize(Roles = "admin,operator")]
        public async Task<IActionResult> UploadProfilePicture([FromForm] UploadPicture upload)
        {
            try
            {
                var participant = await participantRepository.GetParticipantAsyncByID(upload.Id);
                if (participant is null)
                {
                    return BadRequest(new { Error = $"Participant of Id {upload.Id} doesn't Exist." });
                }
                var pic = upload.pic;
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
                string result = await FaceDetectionApi.Detect(upload.Id, upload.pic);


                if(participant.DoProvideFullData && participant.DoProvidePhoto && participant.DoDetected)
                {
                    participant.Status = true;
                }

                if(! await participantRepository.SaveChangesAsync())
                {
                    return BadRequest(new {Error = "Try Again adding Photo"});
                }
                var response_model = mapper.Map<ParticipantResponseModel>(participant);
                return Created("", new { Participant = response_model, Message = "Photo is Created" , Result = result});

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error {ex}");
            }
        }

        [HttpGet("getParticipant/{id:long}")]
        public async Task<IActionResult> GetParticipant(long id)
        {
            try {
                var participant = await participantRepository.GetParticipantAsyncByID(id);
                if (participant is null)
                {
                    return BadRequest(new { Error = $"Participant of Id {id} doesn't Exist." });
                }

                Byte[] b = System.IO.File.ReadAllBytes(participant.PhotoUrl);   // You can use your own method over here.         
                return Ok(new { 
                    Details = participant
                    
                    ,Photo = File(b, "image/jpeg") }) ;
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error {ex}");
            }
        }





        [HttpPut("Participant/update/{email}"), Authorize(Roles = "admin,operator")]
        public async Task<ActionResult<ParticipantResponseModel>> UpdateParticipant(string email, ParticipantAdminModel model)
        {
            try
            {

                var participant = await participantRepository.GetParticipantAsyncByEmail(email, true);
                if (participant == null)
                {
                    return NotFound(new { Error = $"Participant with email {model.Email} doesn't exist" });
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

                //if (model.ProfileImage != null)
                //{
                //    /*
                //     * 
                //     * 
                //     * 
                //     * */
                //}
                if (model.PlateNumberIds.Count > 0)
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


                if (!await vehicleRepository.SaveChangesAsync())
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



        [HttpPost("Vehicle"), Authorize(Roles = "admin, operator")]
        public async Task<ActionResult<Vehicle>> AddVehicle(VehicleAdminModel inputModel)
        {
            try
            {
                var o = await vehicleRepository.GetVehicleAsyncByPlateNumber(inputModel.PlateNumberId);
                if (o != null)
                {
                    return BadRequest(new { Error = $"Vehicle with PlateNumber {inputModel.PlateNumberId} already Exists" });
                }


                var newVehicle = new Vehicle() { PlateNumberId = inputModel.PlateNumberId, IsPresent = false, IsActive = true };

                if (string.IsNullOrEmpty(inputModel.BrandName))
                {
                    newVehicle.IsActive = false;
                }
                else
                {
                    newVehicle.BrandName = inputModel.BrandName;
                }

                if (string.IsNullOrEmpty(inputModel.SubCategory))
                {
                    newVehicle.IsActive = false;
                }
                else
                {
                    newVehicle.SubCategory = inputModel.SubCategory;
                }

                if (string.IsNullOrEmpty(inputModel.Color))
                {
                    newVehicle.IsActive = false;
                }
                else
                {
                    newVehicle.Color = inputModel.Color;
                }

                if (inputModel.StartSubscription == null)
                {
                    newVehicle.IsActive = false;
                }
                else
                {
                    newVehicle.StartSubscription = inputModel.StartSubscription;
                }

                if (inputModel.EndSubscription == null)
                {
                    newVehicle.IsActive = false;
                }
                else
                {
                    newVehicle.EndSubscription = inputModel.EndSubscription;
                }
                vehicleRepository.Add(newVehicle);

                if (!await vehicleRepository.SaveChangesAsync())
                {
                    return BadRequest(new { Error = "Vehicle Not Saved" });
                }

                return Created("", mapper.Map<VehicleResponseModel>(newVehicle));
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
        public string GenerateToken(int length)
        {
            using (RNGCryptoServiceProvider cryptRNG = new RNGCryptoServiceProvider())
            {
                byte[] tokenBuffer = new byte[length];
                cryptRNG.GetBytes(tokenBuffer);
                return Convert.ToBase64String(tokenBuffer);
            }
        }


    }
}
