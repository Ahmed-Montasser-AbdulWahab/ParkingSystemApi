using Microsoft.EntityFrameworkCore;
using Parking_System_API.Data.DBContext;
using Parking_System_API.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Parking_System_API.Data.Repositories.ParticipantR
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly AppDbContext _context;

        public ParticipantRepository(AppDbContext appDbContext)
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

        public async Task<Participant[]> GetAllParticipants(bool getVehicles = false, bool getTransactions = false)
        {
            IQueryable<Participant> query = _context.Participants ;

            if (getVehicles)
            {
                query = query
                  .Include(c => c.Vehicles) ;
            }

            if (getTransactions)
            {
                query = query.Include(c => c.ParkingTransactions) ;
            }

            // Order It
            query = query.OrderByDescending(c => c.Email);

            return await query.ToArrayAsync();
        }

        public async Task<Participant> GetParticipantAsyncByEmail(string email, bool getVehicles = false, bool getTransactions = false)
        {
            IQueryable<Participant> query = _context.Participants;

            if (getVehicles)
            {
                query = query
                  .Include(c => c.Vehicles);
            }

            if (getTransactions)
            {
                query = query.Include(c => c.ParkingTransactions);
            }

            // Order It
            query = query.Where(c => c.Email == email);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Participant> GetParticipantAsyncByNationalID(long NationalId, bool getVehicles = false, bool getTransactions = false)
        {
            IQueryable<Participant> query = _context.Participants;

            if (getVehicles)
            {
                query = query
                  .Include(c => c.Vehicles);
            }

            if (getTransactions)
            {
                query = query.Include(c => c.ParkingTransactions);
            }

            // Order It
            query = query.Where(c => c.NationalId == NationalId);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<Participant> GetParticipantAsyncByID(string id, bool getVehicles = false, bool getTransactions = false)
        {
            IQueryable<Participant> query = _context.Participants;

            if (getVehicles)
            {
                query = query
                  .Include(c => c.Vehicles);
            }

            if (getTransactions)
            {
                query = query.Include(c => c.ParkingTransactions);
            }

            // Order It
            query = query.Where(c => c.Id == id);

            return await query.FirstOrDefaultAsync();
        }
            public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
