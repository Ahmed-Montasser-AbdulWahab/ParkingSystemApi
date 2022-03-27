using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Parking_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerminalsController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CarEntering()
        {
            return Ok("");
            //Car Press Presence Sensor
            //gate is closed
            //Plate Recognition Api Calling
            /*
             * Model returns string of PlateNumber and characters
            */

            //If Successfully having plate number
            /*
             Calling Face Model Api
            a- Detect people, if return id : then this person is already stored.
            check relationship between vehicle and person
            -- if relation is valid :
                        check subscription of vehicle
                        -- if subscription is valid :
                                            entering parking transaction 
                                            gate status is true
                        -- else :
                                            return access denied
            else :
                return access denied
             */
            //return access denied

        }


        [HttpPost]
        public async Task<IActionResult> CarExiting()
        {
            return Ok("");
            //Car Press Presence Sensor
            //gate is closed
            //Plate Recognition Api Calling
            /*
             * Model returns string of PlateNumber and characters
            */

            //If Successfully having plate number
            /*
             Calling Face Model Api
            a- Detect people, if return id : then this person is already stored.
            check relationship between vehicle and person
            -- if relation is valid :
                        check subscription of vehicle
                        -- if subscription is valid :
                                            exit parking transaction
                                            gate status is true
                        -- else :
                                            return access denied
            else :
                return access denied
             */
            //return access denied

        }


        [HttpPost]
        public async Task<IActionResult> CarDeparture()
        {
            return Ok("");
            //Car leaves Presence Sensor
            //delay 2 seconds
            //gate is closed


        }
    }
}
