using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Parking_System.Classes;
using Parking_System_API.Data.DBContext;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Parking_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerminalsController : ControllerBase
    {
        private readonly AppDbContext context;

        public object Datetime { get; private set; }

        public TerminalsController(AppDbContext context)
        {
            this.context = context;
        }
        [HttpPost]
        public async Task<IActionResult> CarEntering(int GateId)
        {
            //Car Press Presence Sensor
            string PlateNum;
            var gate = await context.Gates.FindAsync(GateId);
            if (gate == null)
                return NotFound();
            else if (gate.State)//gate is open
                return NotFound();
            else//gate is closed 
            {
                //calling APNR model

                PlateNum = "ABC123";
                var car = context.Vehicles.Find(PlateNum);
                if (car == null)
                    return NotFound();

                //calling the faceModel
                string FaceRecognitionUrl = "http://127.0.0.1:5000/";
                WebClient client = new WebClient();
                byte[] response = client.DownloadData(FaceRecognitionUrl);
                string res = System.Text.Encoding.ASCII.GetString(response);
                JObject json = JObject.Parse(res);
                string ParticipantId = json["Id"].ToString();
                if (ParticipantId == null)
                    return NotFound();
                if (ParticipantId == "unknown")
                    return NotFound();

                //checking if Id exists in DB
                var Person = await context.Participants.Include(E => E.Vehicles).Where(m => m.Id == ParticipantId).FirstOrDefaultAsync();
                if (Person == null)
                    return NotFound();
                if (Person.Vehicles.Contains(car))
                {
                    //check subscription
                    DateTime Timenow = DateTime.Now;
                    if (Timenow > car.StartSubscription)
                    {
                        if (Timenow < car.EndSubscription)
                        {
                            //Parking Transaction
                            //ParkingTransaction PT=new ParkingTransaction(car.PlateNumberId,gate.TerminalId,) 
                            gate.State = true;
                            return Ok("Access Allowed; Gate is being open");
                        }
                    }



                }
                return NotFound();

            }
        }


        [HttpPost]
        public async Task<IActionResult> CarExiting(int GateId)
        {
             //Car Press Presence Sensor
            string PlateNum;
            var gate = await context.Gates.FindAsync(GateId);
            if (gate == null)
                return NotFound();
            else if (gate.State)//gate is open
                return NotFound();
            else//gate is closed 
            {
                //calling APNR model

                PlateNum = "ABC123";
                var car = context.Vehicles.Find(PlateNum);
                if (car == null)
                    return NotFound();

                //calling the faceModel
                string FaceRecognitionUrl = "http://127.0.0.1:5000/";
                WebClient client = new WebClient();
                byte[] response = client.DownloadData(FaceRecognitionUrl);
                string res = System.Text.Encoding.ASCII.GetString(response);
                JObject json = JObject.Parse(res);
                string ParticipantId = json["Id"].ToString();
                if (ParticipantId == null)
                    return NotFound();
                if (ParticipantId == "unknown")
                    return NotFound();

                //checking if Id exists in DB
                var Person = await context.Participants.Include(E => E.Vehicles).Where(m => m.Id == ParticipantId).FirstOrDefaultAsync();
                if (Person == null)
                    return NotFound();
                if (Person.Vehicles.Contains(car))
                {
                    //check subscription
                    DateTime Timenow = DateTime.Now;
                    if (Timenow < car.EndSubscription)
                    {
                        //Parking Transaction
                        //ParkingTransaction PT=new ParkingTransaction(car.PlateNumberId,gate.TerminalId,) 
                        gate.State = true;
                        return Ok("exit Allowed; Gate is being open");
                    }



                }
                return NotFound();

            }

        }


        [HttpPost]
        public async Task<IActionResult> CarDeparture(int GateId)
        {
            var gate = context.Gates.Find(GateId);
            if (gate == null)
                return NotFound();
            await Task.Delay(3000);
            gate.State = false;
            return Ok("Gate is closed");

        }
    }
}
