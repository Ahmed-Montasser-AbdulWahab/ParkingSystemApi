using AutoMapper;
using Parking_System_API.Data.Entities;
using Parking_System_API.Data.Models;
using System;

namespace Parking_System_API.Data.ParkingProfile
{
    public class SystemUserProfile : Profile
    {
        public SystemUserProfile()
        {
            this.CreateMap<SystemUser, SystemUserResponseModel>().ForMember(c => c.SystemUserRole, m => m.MapFrom(o => (o.IsAdmin)? "admin":"operator"));
            this.CreateMap<SystemUserSignUpModel, SystemUser>(); 
        }

    
    }
}
