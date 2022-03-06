using Microsoft.EntityFrameworkCore;
using Parking_System_API.Data.DBContext;
using Parking_System_API.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Parking_System_API.Data.Repositories.HardwareR
{
    public class HardwareRepository : IHardwareRepository
    {
        private readonly AppDbContext _context;

        public HardwareRepository(AppDbContext appDbContext)
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

        public async Task<Hardware[]> GetAllHardwaresAsync(bool checkParkingTransaction = false)
        {
            IQueryable<Hardware> query = _context.Hardwares;

            if (checkParkingTransaction)
            {
                query = query.Include(c => c.ParkingTransactions);
            }
            // Order It
            query = query.OrderByDescending(c => c.HardwareId); //Return Password

            return await query.ToArrayAsync();
        }

        public async Task<Hardware> GetHardwareAsyncByConnectionString(string ConnectionString, bool checkParkingTransaction = false)
        {
            IQueryable<Hardware> query = _context.Hardwares;

            if (checkParkingTransaction)
            {
                query = query.Include(c => c.ParkingTransactions);
            }
            // Order It
            query = query.Where(c=> c.ConnectionString == ConnectionString); //Return Password

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Hardware> GetHardwareAsyncById(int id, bool checkParkingTransaction = false)
        {
            IQueryable<Hardware> query = _context.Hardwares;

            if (checkParkingTransaction)
            {
                query = query.Include(c => c.ParkingTransactions);
            }
            // Order It
            query = query.Where(c => c.HardwareId == id); //Return Password

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Hardware[]> GetHardwaresAsyncByType(string hardwareType, bool checkParkingTransaction = false)
        {
            IQueryable<Hardware> query = _context.Hardwares;

            if (checkParkingTransaction)
            {
                query = query.Include(c => c.ParkingTransactions);
            }

            query = query.Where(c => c.HardwareType == hardwareType);
            // Order It
            query = query.OrderByDescending(c => c.HardwareId); //Return Password

            return await query.ToArrayAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
