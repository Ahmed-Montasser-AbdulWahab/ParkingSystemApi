using Parking_System_API.Data.Entities;
using System.Threading.Tasks;

namespace Parking_System_API.Data.Repositories.HardwareR
{
    public interface IHardwareRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        // SystemUsers
        Task<Camera[]> GetAllHardwaresAsync(bool checkParkingTransaction = false);
        Task<Camera> GetHardwareAsyncById(int id, bool checkParkingTransaction = false);
        Task<Camera[]> GetHardwaresAsyncByType(string hardwareType, bool checkParkingTransaction = false);

        Task<Camera> GetHardwareAsyncByConnectionString(string ConnectionString, bool checkParkingTransaction = false);
    }
}
