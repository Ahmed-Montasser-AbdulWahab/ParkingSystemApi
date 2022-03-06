using Parking_System_API.Data.Entities;
using System;
using System.Threading.Tasks;

namespace Parking_System_API.Data.Repositories.ParkingTransactionsR
{
    public interface IParkingTransactionRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        // SystemUsers
        Task<ParkingTransaction[]> GetAllTransactions(bool getVehicles = false, bool getParticipants = false, bool getHardwares = false);
        Task<ParkingTransaction[]> GetAllTransactionsForParticipant(int ParticipantId, bool getVehicles = false, bool getHardwares = false);
        Task<ParkingTransaction[]> GetAllTransactionsForVehicle(string VehicleId, bool getParticipants = false, bool getHardwares = false);
        Task<ParkingTransaction[]> GetAllTransactionsForParticipantAndVehicle(int ParticipantId, string VehicleId, bool getHardwares = false);
        Task<ParkingTransaction> GetTransaction(DateTime dateTime, int participantId, string vehicleId, int hardwareId);
        Task<ParkingTransaction[]> GetTransactionsByDateTime(DateTime dateTime, bool getVehicles = false, bool getParticipants = false, bool getHardwares = false);
        Task<ParkingTransaction[]> GetTransactionsByDateTimeRange(DateTime StartdateTime, DateTime EnddateTime, bool getVehicles = false, bool getParticipants = false, bool getHardwares = false);
    }
}
