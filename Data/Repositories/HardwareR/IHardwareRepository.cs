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
        Task<Hardware[]> GetAllHardwaresAsync(bool checkParkingTransaction = false);
        Task<Hardware> GetHardwareAsyncById(int id, bool checkParkingTransaction = false);
        Task<Hardware[]> GetHardwaresAsyncByType(string hardwareType, bool checkParkingTransaction = false);

        Task<Hardware> GetHardwareAsyncByConnectionString(string ConnectionString, bool checkParkingTransaction = false);
    }
}
