using Parking_System_API.Data.Entities;
using System.Threading.Tasks;

namespace Parking_System_API.Data.Repositories.GateR
{
    public interface IGateRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        Task<Gate[]> GetAllGates(bool includeTerminal = false);
        Task<Gate> GetGateById(int id, bool includeTerminal = false);
    }
}
