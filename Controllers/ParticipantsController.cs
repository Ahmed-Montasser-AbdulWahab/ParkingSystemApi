using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Parking_System_API.Data.Models;
using Parking_System_API.Data.Repositories.ConstantsR;
using Parking_System_API.Data.Repositories.ParticipantR;
using Parking_System_API.Data.Repositories.VehicleR;
using Parking_System_API.Model;
using System;
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

        [HttpGet, Authorize(Roles ="admin, operator")]
        public async Task<ActionResult<ParticipantResponseModel[]>> GetAllParticipants()
        {
            try {
                var participants = await participantRepository.GetAllParticipants();
                if (participants.Length == 0)
                {
                    return NotFound("No Participants Exist");

                }
                ParticipantResponseModel[] models = mapper.Map<ParticipantResponseModel[]>(participants);
                return models;
            }
            catch(Exception) {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ParticipantResponseModel>> GetParticipant(long id)
        {
            try
            {
                var participant = await participantRepository.GetParticipantAsyncByID(id);
                if (participant == null)
                {
                    return NotFound($"Participant with id {id} Don't Exist");
                }

                var model = mapper.Map<ParticipantResponseModel>(participant);
                return model;
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpGet("getParticipantByEmail/{email}")]
        public async Task<ActionResult<ParticipantResponseModel>> GetParticipantByEmail(string email)
        {
            try
            {
                var participant = await participantRepository.GetParticipantAsyncByEmail(email);
                if (participant == null)
                {
                    return NotFound($"Participant with emai {email} Don't Exist");
                }

                var model = mapper.Map<ParticipantResponseModel>(participant);
                return model;
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }


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

    }
}
