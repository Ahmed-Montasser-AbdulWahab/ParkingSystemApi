using Microsoft.EntityFrameworkCore;
using Parking_System_API.Data.DBContext;
using Parking_System_API.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Parking_System_API.Data.Repositories.ParkingTransactionR
{
    public class ParkingTransactionRepository : IParkingTransactionRepository
    {
        private readonly AppDbContext _context;

        public ParkingTransactionRepository(AppDbContext appDbContext)
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

        public async Task<ParkingTransaction[]> GetAllTransactions(bool getVehicles = false, bool getParticipants = false, bool getTerminals = false)
        {
            IQueryable<ParkingTransaction> query = _context.ParkingTransactions;

            if (getVehicles)
            {
                query = query
                  .Include(c => c.vehicle);
            }

            if (getParticipants)
            {
                query = query.Include(c => c.participant);
            }

            if (getTerminals)
            {
                query = query.Include(c => c.terminal);
            }
            // Order It
            query = query.OrderByDescending(c => c.DateTimeTransaction);

            return await query.ToArrayAsync();
        }

        public async Task<ParkingTransaction[]> GetAllTransactionsForParticipant(string ParticipantId, bool getVehicles = false, bool getTerminals = false)
        {
            IQueryable<ParkingTransaction> query = _context.ParkingTransactions;

            if (getVehicles)
            {
                query = query
                  .Include(c => c.vehicle);
            }


            if (getTerminals)
            {
                query = query.Include(c => c.terminal);
            }

            // Order It
            query = query.Where(c => c.ParticipantId == ParticipantId);

            return await query.ToArrayAsync();
        }

        public async Task<ParkingTransaction[]> GetAllTransactionsForVehicle(string VehicleId, bool getParticipants = false, bool getTerminals = false)
        {
            IQueryable<ParkingTransaction> query = _context.ParkingTransactions;

            if (getParticipants)
            {
                query = query
                  .Include(c => c.participant);
            }


            if (getTerminals)
            {
                query = query.Include(c => c.terminal);
            }

            // Order It
            query = query.Where(c => c.PlateNumberId == VehicleId);

            return await query.ToArrayAsync();
        }

        public async Task<ParkingTransaction[]> GetAllTransactionsForParticipantAndVehicle(string ParticipantId, string VehicleId, bool getTerminals = false)
        {
            IQueryable<ParkingTransaction> query = _context.ParkingTransactions;


            if (getTerminals)
            {
                query = query.Include(c => c.terminal);
            }

            // Order It
            query = query.Where(c => c.PlateNumberId == VehicleId && c.ParticipantId == ParticipantId).OrderByDescending(o => o.DateTimeTransaction);

            return await query.ToArrayAsync();
        }

        public async Task<ParkingTransaction> GetTransaction(DateTime dateTime, string participantId, string vehicleId, int terminalId)
        {
            IQueryable<ParkingTransaction> query = _context.ParkingTransactions;

            // Order It
            query = query.Where(c => c.ParticipantId == participantId && c.PlateNumberId == vehicleId && c.TerminalId == terminalId && c.DateTimeTransaction==dateTime);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<ParkingTransaction[]> GetTransactionsByDateTime(DateTime dateTime, bool getVehicles = false, bool getParticipants = false, bool getTerminals = false)
        {
            IQueryable<ParkingTransaction> query = _context.ParkingTransactions;

            if (getVehicles)
            {
                query = query
                  .Include(c => c.vehicle);
            }

            if (getParticipants)
            {
                query = query.Include(c => c.participant);
            }

            if (getTerminals)
            {
                query = query.Include(c => c.terminal);
            }
            query = query.Where(c => c.DateTimeTransaction == dateTime);
            // Order It
            query = query.OrderByDescending(c => c.DateTimeTransaction);

            return await query.ToArrayAsync();
        }

        public async Task<ParkingTransaction[]> GetTransactionsByDateTimeRange(DateTime StartdateTime, DateTime EnddateTime, bool getVehicles = false, bool getParticipants = false, bool getTerminals = false)
        {
            IQueryable<ParkingTransaction> query = _context.ParkingTransactions;

            if (getVehicles)
            {
                query = query
                  .Include(c => c.vehicle);
            }

            if (getParticipants)
            {
                query = query.Include(c => c.participant);
            }

            if (getTerminals)
            {
                query = query.Include(c => c.terminal);
            }
            query = query.Where(c => c.DateTimeTransaction <= EnddateTime && c.DateTimeTransaction >= StartdateTime );
            // Order It
            query = query.OrderByDescending(c => c.DateTimeTransaction);

            return await query.ToArrayAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }



    }
}
