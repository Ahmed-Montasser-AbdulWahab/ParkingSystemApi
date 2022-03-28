using Microsoft.EntityFrameworkCore;
using Parking_System_API.Data.DBContext;
using System.Linq;
using System.Threading.Tasks;

namespace Parking_System_API.Data.Repositories.RoleR
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            this._context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> RoleExistsAsync(string RoleName)
        {
            var query =await _context.Roles.Where(c => c.RoleName == RoleName).FirstOrDefaultAsync();
            
            return query is null ? false : true;


        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
