using AutoMapper;
using Parking_System_API.Data.Entities;
using Parking_System_API.Data.Models;

namespace Parking_System_API.Data.ProjectProfile
{
    public class ParticipantProfile : Profile
    {
        public ParticipantProfile()
        {
            this.CreateMap<Participant, ParticipantResponseModel>();
        }
    }
}
