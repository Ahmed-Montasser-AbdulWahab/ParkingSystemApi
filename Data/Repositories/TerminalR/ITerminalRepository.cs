using Parking_System_API.Data.Entities;
using System.Threading.Tasks;

namespace Parking_System_API.Data.Repositories.HardwareR
{
    public interface ITerminalRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        // SystemUsers
        Task<Terminal[]> GetAllTerminalsAsync(bool checkParkingTransaction = false);
        Task<Terminal> GetTerminalAsyncById(int id, bool checkParkingTransaction = false);
        

        Task<Terminal> GetTerminalAsyncByConnectionString(string ConnectionString, bool checkParkingTransaction = false);
    }
}
