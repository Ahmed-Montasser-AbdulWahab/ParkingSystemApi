using AutoMapper;
using Parking_System_API.Data.Entities;
using Parking_System_API.Data.Models;

namespace Parking_System_API.Data.ProjectProfile
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            this.CreateMap<Vehicle,VehicleResponseModel>();
        }
    }
}
