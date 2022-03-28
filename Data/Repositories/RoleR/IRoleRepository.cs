using System.Threading.Tasks;

namespace Parking_System_API.Data.Repositories.RoleR
{
    public interface IRoleRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        Task<bool> RoleExistsAsync(string RoleName);
    }
}
