using Parking_System_API.Data.Entities;
using System.Threading.Tasks;

namespace Parking_System_API.Data.Repositories.ParticipantR
{
    public interface IParticipantRepository
    {
        // General 
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        // SystemUsers
        Task<Participant[]> GetAllParticipants(bool getVehicles = false, bool getTransactions = false);
        Task<Participant> GetParticipantAsyncByEmail(string email, bool getVehicles = false, bool getTransactions = false);
        Task<Participant> GetParticipantAsyncByNationalID(long NationalId, bool getVehicles = false, bool getTransactions = false);
        Task<Participant> GetParticipantAsyncByID(string id, bool getVehicles = false, bool getTransactions = false);
    }
}
