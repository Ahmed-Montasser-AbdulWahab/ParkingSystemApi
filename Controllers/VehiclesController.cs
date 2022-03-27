using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Parking_System_API.Data.Entities;
using Parking_System_API.Data.Models;
using Parking_System_API.Data.Repositories.VehicleR;
using System;
using System.Threading.Tasks;

namespace Parking_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleRepository vehicleRepository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;

        public VehiclesController(IVehicleRepository vehicleRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            this.vehicleRepository = vehicleRepository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
        }


        [Authorize(Roles = "admin, operator")]
        public async Task<ActionResult<Vehicle>> AddVehicle(VehicleAdminModel inputModel)
        {
            try
            {
                var o = await vehicleRepository.GetVehicleAsyncByPlateNumber(inputModel.PlateNumberId);
                if (o != null)
                {
                    return BadRequest($"Vehicle with PlateNumber {inputModel.PlateNumberId} already Exists");
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
                    return BadRequest("Vehicle Not Saved");
                }

                return Created(linkGenerator.GetPathByAction("GetVehicle", "Vehicles", new {plateID = newVehicle.PlateNumberId }), mapper.Map<VehicleResponseModel>(newVehicle));
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error {ex}");
            }
        }


        [HttpGet("{plateID}"), Authorize(Roles ="admin, operator")]
        public async Task<ActionResult<VehicleResponseModel>> GetVehicle(string plateID)
        {
            try {

                var vehicle = await vehicleRepository.GetVehicleAsyncByPlateNumber(plateID);
                if (vehicle == null)
                {
                    return NotFound($"Vehicle with PlateNumber {plateID} is not Found");
                }
                
                var model = mapper.Map<VehicleResponseModel>(vehicle);

                return model;
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error {ex}");
            }
        }


        [HttpGet, Authorize(Roles = "admin, operator")]
        public async Task<ActionResult<VehicleResponseModel[]>> GetVehicles()
        {
            try
            {

                var vehicle = await vehicleRepository.GetAllVehicles();
                if (vehicle.Length == 0)
                {
                    return NotFound($"No Vehicles Found");
                }

                var model = mapper.Map<VehicleResponseModel[]>(vehicle);

                return model;
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error {ex}");
            }
        }
    }
}
