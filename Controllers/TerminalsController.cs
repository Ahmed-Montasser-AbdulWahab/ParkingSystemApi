using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Parking_System_API.Data.DBContext;
using Parking_System_API.Data.Entities;
using Parking_System_API.Data.Repositories.CameraR;
using Parking_System_API.Data.Repositories.GateR;
using Parking_System_API.Data.Repositories.HardwareR;
using Parking_System_API.Data.Repositories.ParkingTransactionR;
using Parking_System_API.Data.Repositories.ParticipantR;
using Parking_System_API.Data.Repositories.VehicleR;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Parking_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerminalsController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IGateRepository gateRepository;
        private readonly ICameraRepository cameraRepository;
        private readonly ITerminalRepository terminalRepository;
        private readonly IParticipantRepository participantRepository;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IParkingTransactionRepository parkingTransactionRepository;

        public TerminalsController(IGateRepository gateRepository, ICameraRepository cameraRepository, ITerminalRepository terminalRepository, IParticipantRepository participantRepository, IVehicleRepository vehicleRepository, IParkingTransactionRepository parkingTransactionRepository)
        {
            this.gateRepository = gateRepository;
            this.cameraRepository = cameraRepository;
            this.terminalRepository = terminalRepository;
            this.participantRepository = participantRepository;
            this.vehicleRepository = vehicleRepository;
            this.parkingTransactionRepository = parkingTransactionRepository;
        }
        [HttpPost("CarEntry/{GateId}")]
        public async Task<IActionResult> CarEntering(int GateId)
        {
            try
            {
                //Car Press Presence Sensor

                var gate = await gateRepository.GetGateById(GateId, true);
                if (gate == null)
                    return NotFound(new { Error = $"Gate with Id {GateId} is not found." });
                if (!gate.Service)
                {
                    return BadRequest(new { Error = "Gate is offline" });
                }
                if (gate.State) //gate is open
                {
                    gate.State = false;
                }
                //gate is closed
                //calling APNR model

                string PlateNum = "ABC123";
                var car = await vehicleRepository.GetVehicleAsyncByPlateNumber(PlateNum);
                if (car == null)
                    return NotFound(new { Error = $"Car with PlateNumber {PlateNum} is not found" });

                //calling the faceModel

                string ParticipantId = "";
                Thread participantIdThread = new Thread(
                    () =>
                    ParticipantId = GetParticipantId("http://127.0.0.1:5000/"));
                participantIdThread.Start();
                participantIdThread.Join();
                if (ParticipantId == null)
                    return BadRequest(new { Error = "ParticipantId is null" });
                if (ParticipantId == "unknown")
                    return NotFound(new { Error = "ParticipantId is unknown" });

                //checking if Id exists in DB
                var Person = await participantRepository.GetParticipantAsyncByID(ParticipantId, true);
                if (Person == null)
                    return NotFound(new { Error = $"Person with Id {ParticipantId} is not found." });
                if (Person.Vehicles.Contains(car))
                {
                    //check subscription
                    DateTime Timenow = DateTime.Now;
                    if (Timenow > car.StartSubscription && Timenow < car.EndSubscription)
                    {
                        //Parking Transaction

                        gate.State = true;
                        return Ok("Access Allowed; Gate is being open");

                    }

                    else
                    {
                        return Unauthorized(new { Error = "Subscription is not valid." });
                    }

                }
                return NotFound(new { Error = $"Participant with {ParticipantId} doesn't own a Vehicle with PlateNumber {PlateNum}" });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error {ex}");
            }

        }

        private static String GetParticipantId(String Url)
        {
            WebClient client = new WebClient();
            byte[] response = client.DownloadData(Url);
            string res = System.Text.Encoding.ASCII.GetString(response);
            JObject json = JObject.Parse(res);
            return json["Id"].ToString();
        }

        [HttpPost("CarExit/{GateId}")]
        public async Task<IActionResult> CarExiting(int GateId)
        {
            try
            {
                //Car Press Presence Sensor

                var gate = await gateRepository.GetGateById(GateId, true);
                if (gate == null)
                    return NotFound(new { Error = $"Gate with Id {GateId} is not found." });
                if (!gate.Service)
                {
                    return BadRequest(new { Error = "Gate is offline" });
                }
                if (gate.State) //gate is open
                {
                    gate.State = false;
                }
                //gate is closed
                //calling APNR model

                string PlateNum = "ABC123";
                var car = await vehicleRepository.GetVehicleAsyncByPlateNumber(PlateNum);
                if (car == null)
                    return NotFound(new { Error = $"Car with PlateNumber {PlateNum} is not found" });

                //calling the faceModel
                string FaceRecognitionUrl = "http://127.0.0.1:5000/";
                WebClient client = new WebClient();
                byte[] response = client.DownloadData(FaceRecognitionUrl);
                string res = System.Text.Encoding.ASCII.GetString(response);
                JObject json = JObject.Parse(res);
                string ParticipantId = json["Id"].ToString();
                if (ParticipantId == null)
                    return BadRequest(new { Error = "ParticipantId is null" });
                if (ParticipantId == "unknown")
                    return NotFound(new { Error = "ParticipantId is unknown" });

                //checking if Id exists in DB
                var Person = await participantRepository.GetParticipantAsyncByID(ParticipantId, true);
                if (Person == null)
                    return NotFound(new { Error = $"Person with Id {ParticipantId} is not found." });
                if (Person.Vehicles.Contains(car))
                {
                    //check subscription
                    DateTime Timenow = DateTime.Now;
                    if (Timenow > car.StartSubscription && Timenow < car.EndSubscription)
                    {
                        //Parking Transaction

                        gate.State = true;
                        return Ok(new { Success = "Access Allowed; Gate is being open" });

                    }
                    else
                    {
                        return Unauthorized(new { Error = "Subscription is not valid." });
                    }


                }

                return NotFound(new { Error = $"Participant with {ParticipantId} doesn't own a Vehicle with PlateNumber {PlateNum}" });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error {ex}");
            }
        }



        [HttpPost("CarDeparture")]
        public async Task<IActionResult> CarDeparture(int GateId)
        {
            try
            {
                var gate = await gateRepository.GetGateById(GateId);
                if (gate == null)
                    return NotFound(new { Error = $"Gate with id {GateId} doesn't exit" });
                await Task.Delay(3000);
                gate.State = false;
                return Ok(new { Success = "Gate is closed" });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error {ex}");
            }
        }
    }
}
