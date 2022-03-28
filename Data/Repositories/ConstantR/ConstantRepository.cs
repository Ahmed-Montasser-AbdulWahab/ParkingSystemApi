using Microsoft.EntityFrameworkCore;
using Parking_System_API.Data.DBContext;
using Parking_System_API.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Parking_System_API.Data.Repositories.ConstantR
{
    public class ConstantRepository : IConstantRepository
    {
        private readonly AppDbContext _context;

        public ConstantRepository(AppDbContext appDbContext)
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

        public async Task<Constant> GetConstantAsyncById(int id)
        {
            return await _context.Constants.Where(c => c.ID == id).FirstOrDefaultAsync();
        }

        public async Task<Constant> GetConstantAsyncByName(string Name)
        {
            return await _context.Constants.Where(c => c.ConstantName.StartsWith(Name)).FirstOrDefaultAsync();
        }

        public async Task<Constant> GetForeignIdAsync()
        {
            return await (_context.Constants.Where(c => c.ID == 1)).FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
