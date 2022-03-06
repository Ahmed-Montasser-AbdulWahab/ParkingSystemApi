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
            this.CreateMap<SystemUser, SystemUserModel>().ForMember(c => c.Type, m => m.MapFrom(o => (o.Type)? "admin":"operator"));
        }

    
    }
}
