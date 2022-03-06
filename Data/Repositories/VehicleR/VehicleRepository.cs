using Microsoft.EntityFrameworkCore;
using Parking_System_API.Data.DBContext;
using Parking_System_API.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Parking_System_API.Data.Repositories.VehicleR
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _context;

        public VehicleRepository(AppDbContext appDbContext)
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

        public async Task<Vehicle[]> GetAllVehicles(bool getParticipants = false, bool getTransactions = false)
        {
            IQueryable<Vehicle> query = _context.Vehicles;

            if (getParticipants)
            {
                query = query
                  .Include(c => c.Participants);
            }

            if (getTransactions)
            {
                query = query.Include(c => c.ParkingTransactions);
            }

            // Order It
            query = query.OrderByDescending(c => c.PlateNumberId);

            return await query.ToArrayAsync(); ;
        }

        public async Task<Vehicle> GetVehicleAsyncByPlateNumber(string plateNumber, bool getParticipants = false, bool getTransactions = false)
        {
            IQueryable<Vehicle> query = _context.Vehicles;

            if (getParticipants)
            {
                query = query
                  .Include(c => c.Participants);
            }

            if (getTransactions)
            {
                query = query.Include(c => c.ParkingTransactions);
            }

            // Order It
            query = query.Where(c => c.PlateNumberId == plateNumber);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
