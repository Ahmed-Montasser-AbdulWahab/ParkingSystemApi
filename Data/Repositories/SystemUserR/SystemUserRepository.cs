using Microsoft.EntityFrameworkCore;
using Parking_System_API.Data.DBContext;
using Parking_System_API.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Parking_System_API.Data.Repositories.SystemUserR
{
    public class SystemUserRepository : ISystemUserRepository
    {
        private readonly AppDbContext _context;

        public SystemUserRepository(AppDbContext appDbContext)
        {
            this._context = appDbContext;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<SystemUser[]> GetAllSystemUsersAsync()
        {
            IQueryable<SystemUser> query = _context.SystemUsers;
            // Order It
            query = query.OrderByDescending(c => c.Email); //Return Password

            return await query.ToArrayAsync();
        }



        public async Task<SystemUser> GetSystemUserAsyncByEmail(string email)
        {
            IQueryable<SystemUser> query = _context.SystemUsers;

            // Query It
            query = query.Where(c => c.Email == email);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<SystemUser[]> GetSystemUsersAsyncByName(string name)
        {
            IQueryable<SystemUser> query = _context.SystemUsers;

            // Query It
            query = query.Where(c => c.Name.StartsWith(name));

            return await query.ToArrayAsync();
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }


    }
}
