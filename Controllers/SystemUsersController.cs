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

namespace Parking_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemUsersController : ControllerBase
    {
        private readonly IConstantRepository constantRepository;
        private readonly ISystemUserRepository systemUserRepository;
        private readonly JwtAuthenticationManager jwtAuthenticationManager;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IParticipantRepository participantRepository;
        // private static long ForeignMemberId = 10000000000000 ;

        public SystemUsersController(IConstantRepository constantRepository, ISystemUserRepository systemUserRepository, JwtAuthenticationManager jwtAuthenticationManager, IMapper mapper, LinkGenerator linkGenerator, IVehicleRepository vehicleRepository, IParticipantRepository participantRepository)
        {
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
            try {
                var authResult = await jwtAuthenticationManager.AuthenticateAdminAndOpertor(authenticationRequest.Email, authenticationRequest.Password);
                if (authResult == null)
                    return Unauthorized();
                else
                    return Ok(authResult);
            }
            catch (Exception) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }

        }

        [HttpPost("signup")]//, Authorize(Roles = "admin,operator")]
        public async Task<IActionResult> Signup([FromBody] SystemUser systemUser)
        {
            try
            {

                var checkSystemUser = systemUserRepository.GetSystemUserAsyncByEmail(systemUser.Email);
                if(checkSystemUser != null)
                {
                    return BadRequest($"System User with {systemUser.Email} already exists");

                }
                

                var location = linkGenerator.GetPathByAction("GetSystemUser", "SystemUsers", new { email = systemUser.Email });
                if (String.IsNullOrEmpty(location))
                {
                    BadRequest("Try Again");
                }
                var salt = Hashing.HashingClass.GenerateSalt();
                var hashed = Hashing.HashingClass.GenerateHashedPassword(systemUser.Password, salt);
                systemUser.Password = hashed;
                systemUser.Salt = salt;
                systemUserRepository.Add(systemUser);

                if (await systemUserRepository.SaveChangesAsync())
                {
                    return Created(location, mapper.Map<SystemUserModel>(systemUser));
                }
                return BadRequest("Check Provided Data, i.e:Data may be duplicated");
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }


        [HttpGet]//, Authorize(Roles = "admin")]
        public async Task<ActionResult<SystemUserModel[]>> GetAllSystemUsers()
        {
            try
            {
                
                
                var systemUsers = await systemUserRepository.GetAllSystemUsersAsync();
                if (systemUsers.Length == 0)
                {
                    return NotFound("No SystemUsers Exist");

                }
                SystemUserModel[] models = mapper.Map<SystemUserModel[]>(systemUsers);
                return models;
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }
        [HttpGet("getByEmail/{email}"), Authorize(Roles = "admin")]
        public async Task<ActionResult<SystemUserModel>> GetSystemUser(string email)
        {
            try
            {
                var systemUser = await systemUserRepository.GetSystemUserAsyncByEmail(email);
                if (systemUser == null)
                {
                    return NotFound($"System User with {systemUser.Email} not Found");

                }
                SystemUserModel model = mapper.Map<SystemUserModel>(systemUser);
                return model;
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpGet("getByName/{name}"), Authorize(Roles = "admin")]
        public async Task<ActionResult<SystemUserModel[]>> GetAllSystemUsersByName(string name)
        {
            try
            {
                var systemUsers = await systemUserRepository.GetSystemUsersAsyncByName(name);
                if (systemUsers.Length == 0)
                {
                    return NotFound($"No SystemUsers with name : {name} Exist");

                }
                SystemUserModel[] models = mapper.Map<SystemUserModel[]>(systemUsers);
                return models;
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
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
                    return BadRequest($"Participant with email {model.Email} already exists");
                }
                if(model.Id != null)
                {
                    participant = await participantRepository.GetParticipantAsyncByID(model.Id.Value);
                    if (participant != null)
                    {
                        return BadRequest($"Participant with id {model.Id.Value} already exists");
                    }
                }
                
                participant = new Participant() { Status = true };
                if (model.isEgyptian)
                {
                    if (model.Id == null)
                    {
                        return BadRequest("Please provide National Id");
                    }
                    else
                    {
                        participant.ParticipantId = model.Id.Value;
                    }

                }

                else if (!model.isEgyptian)
                {
                    var Constant = await constantRepository.GetForeignIdAsync();

                    participant.ParticipantId = Constant.Value;
                    Constant.Value++;

                    if (!await constantRepository.SaveChangesAsync())
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "Error");
                    }

                }

                //Adding Email

                participant.Email = model.Email;

                //Adding Password and sending it Via Email
                var password = GenerateToken(8);
                
                participant.Salt = HashingClass.GenerateSalt();

                participant.Password = HashingClass.GenerateHashedPassword(password, participant.Salt);

                //Checking Photo and detection
                if (model.ProfileImage == null)
                {
                    participant.DoProvidePhoto = false;
                    participant.Status = false;
                }
                else
                {
                    participant.DoProvidePhoto = true;
                    //go to model and detection "Muhammad Samy"
                    /*
                     * 
                     * 
                     * 
                     * 
                     */
                    participant.DoDetected = true;
                }

                //checking name
                if (model.Name == null)
                {
                    participant.Status = false;
                }
                else
                {
                    participant.Name = model.Name;
                }

                //Adding Vehicles
                
                if (model.PlateNumberIds.Count == 0) //Null or Empty
                {
                    participant.Status = false;
                }
                else
                {
                    foreach (var v in model.PlateNumberIds)
                    {
                        var Vehicle = await vehicleRepository.GetVehicleAsyncByPlateNumber(v);
                        if (Vehicle == null)
                        {
                            return BadRequest($"No Vehicle saved with the provided License Plate {v}");
                        }
                        else
                        {
                            participant.Vehicles.Add(Vehicle);

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
                    return Created("", response_model);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error {ex}"); 
            }

            
        }

    /*    [HttpPut("Participant/{email}"), Authorize(Roles = "admin,operator")]
        public async Task<ActionResult<Participant>> UpdateParticipant(string email,ParticipantAdminModel model)
        {
            try
            {
                var participant = await participantRepository.GetParticipantAsyncByEmail(model.Email);
                if (participant != null)
                {
                    return BadRequest($"Participant with email {model.Email} already exists");
                }


            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error {ex}");
            }
            }
    */

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
