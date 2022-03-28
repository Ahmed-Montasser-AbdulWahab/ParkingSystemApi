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

using Parking_System_API.Hashing;
using Parking_System_API.Data.Repositories.ConstantR;
using System.Collections.Generic;
using Parking_System_API.Data.Repositories.RoleR;
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
        // private static long ForeignMemberId = 10000000000000 ;

        public SystemUsersController(IRoleRepository roleRepository, IConstantRepository constantRepository, ISystemUserRepository systemUserRepository, JwtAuthenticationManager jwtAuthenticationManager, IMapper mapper, LinkGenerator linkGenerator)
        {
            this.roleRepository = roleRepository;
            this.constantRepository = constantRepository;
            this.systemUserRepository = systemUserRepository;
            this.jwtAuthenticationManager = jwtAuthenticationManager;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
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
                if (!await roleRepository.RoleExistsAsync(systemUserModel.Role.ToLower()))
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
        [HttpGet("me"), Authorize(Roles = "admin, operator")]
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

        [HttpGet("SystemUser/{email?}"), Authorize(Roles = "admin")]//Write this in Post Man
        public async Task<ActionResult<SystemUserResponseModel>> GetSystemUser(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    email = User.Claims.First(i => i.Type == ClaimTypes.Email).Value;
                }
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

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordModel model)
        {
            try
            {
                var systemUser = await systemUserRepository.GetSystemUserAsyncByEmail(model.Email);
                if (systemUser is null)
                {
                    return BadRequest(new { Error = $"Participant of Email {model.Email} doesn't Exist." });
                }
                if (systemUser.IsPowerAccount)
                {
                    return BadRequest(new { Error = $"It is a Power Account. No Changes." });
                }
                var password = TokenGeneration.GenerateToken(8);

                systemUser.Salt = HashingClass.GenerateSalt();

                systemUser.Password = HashingClass.GenerateHashedPassword(password, systemUser.Salt);
                if (!await systemUserRepository.SaveChangesAsync())
                {
                    return BadRequest(new { Error = "Try Again Later" });
                }
                Email.EmailCode.SendEmail(systemUser.Email, password, "Password Reset");

                return Ok(new { Message = "Check Reset Password in your Email Inbox" });
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error");
            }

        }


        [HttpPut("SystemUser/changeMyPassword"), Authorize(Roles = "admin,operator")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            try
            {
                var email = User.Claims.First(i => i.Type == ClaimTypes.Email).Value;
                var systemUser = await systemUserRepository.GetSystemUserAsyncByEmail(email);
                if (systemUser == null)
                {
                    return NotFound(new { Error = $"Participant with email {email} doesn't exist" });
                }

                var oldHashedPassword = HashingClass.GenerateHashedPassword(model.OldPassword, systemUser.Salt);

                if (oldHashedPassword != systemUser.Password)
                {
                    return BadRequest(new { Error = "Error is found" });
                }

                systemUser.Salt = HashingClass.GenerateSalt();

                systemUser.Password = HashingClass.GenerateHashedPassword(model.NewPassword, systemUser.Salt);


                if (!await systemUserRepository.SaveChangesAsync())
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







    }
}
