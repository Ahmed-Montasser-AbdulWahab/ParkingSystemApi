using Microsoft.EntityFrameworkCore;
using Parking_System_API.Data.DBContext;
using Parking_System_API.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Parking_System_API.Data.Repositories.HardwareR
{
    public class TerminalRepository : ITerminalRepository
    {
        private readonly AppDbContext _context;

        public TerminalRepository(AppDbContext appDbContext)
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

        public async Task<Terminal[]> GetAllTerminalsAsync(bool checkParkingTransaction = false)
        {
            IQueryable<Terminal> query = _context.Terminals;

            if (checkParkingTransaction)
            {
                query = query.Include(c => c.ParkingTransactions);
            }
            // Order It
            query = query.OrderByDescending(c => c.Id); //Return Password

            return await query.ToArrayAsync();
        }

        public async Task<Terminal> GetTerminalAsyncByConnectionString(string ConnectionString, bool checkParkingTransaction = false)
        {
            IQueryable<Terminal> query = _context.Terminals;

            if (checkParkingTransaction)
            {
                query = query.Include(c => c.ParkingTransactions);
            }
            // Order It
            query = query.Where(c=> c.ConnectionString == ConnectionString); //Return Password

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Terminal> GetTerminalAsyncById(int id, bool checkParkingTransaction = false)
        {
            IQueryable<Terminal> query = _context.Terminals;

            if (checkParkingTransaction)
            {
                query = query.Include(c => c.ParkingTransactions);
            }
            // Order It
            query = query.Where(c => c.Id == id); //Return Password

            return await query.FirstOrDefaultAsync();
        }


        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
