using Microsoft.EntityFrameworkCore;
using Parking_System_API.Data.DBContext;
using Parking_System_API.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Parking_System_API.Data.Repositories.CameraR
{
    public class CameraRepository : ICameraRepository
    {
        private readonly AppDbContext _context;

        public CameraRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            this._context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Camera[]> GetAllCameras(bool includeTerminal = false)
        {
            IQueryable<Camera> query = _context.Cameras;
            if (includeTerminal)
            {
                query = query.Include(c => c.Terminal);
            }
            query = query.OrderBy(c => c.Id); 

            return await query.ToArrayAsync();
        }

        public async Task<Camera> GetCameraByConnectionString(string ConnectionString, bool includeTerminal = false)
        {
            IQueryable<Camera> query = _context.Cameras;

            if (includeTerminal)
            {
                query = query.Include(c => c.Terminal);
            }
            // Order It
            query = query.Where(c => c.ConnectionString == ConnectionString); 

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Camera> GetCameraById(int Id, bool includeTerminal = false)
        {
            IQueryable<Camera> query = _context.Cameras;

            if (includeTerminal)
            {
                query = query.Include(c => c.Terminal);
            }
            // Order It
            query = query.Where(c => c.Id == Id); 

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
