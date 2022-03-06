using Parking_System_API.Data.Entities;
using System.Threading.Tasks;

namespace Parking_System_API.Data.Repositories.SystemUserR
{
    public interface ISystemUserRepository
    {
        // General 
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        // SystemUsers
        Task<SystemUser[]> GetAllSystemUsersAsync();
        Task<SystemUser> GetSystemUserAsyncByEmail(string email);
        Task<SystemUser[]> GetSystemUsersAsyncByName(string name);

    }
}
