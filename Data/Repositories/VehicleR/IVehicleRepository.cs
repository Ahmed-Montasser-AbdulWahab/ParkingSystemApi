using Parking_System_API.Data.Entities;
using System.Threading.Tasks;

namespace Parking_System_API.Data.Repositories.VehicleR
{
    public interface IVehicleRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        // SystemUsers
        Task<Vehicle[]> GetAllVehicles(bool getParticipants = false, bool getTransactions = false);
        Task<Vehicle> GetVehicleAsyncByPlateNumber(string plateNumber, bool getParticipants = false, bool getTransactions = false);
    }
}
