using Parking_System_API.Data.Entities;
using System.Threading.Tasks;

namespace Parking_System_API.Data.Repositories.ConstantR
{
    public interface IConstantRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        Task<Constant> GetConstantAsyncById(int id);
        Task<Constant> GetConstantAsyncByName(string Name);
        Task<Constant> GetForeignIdAsync();
    }
}
