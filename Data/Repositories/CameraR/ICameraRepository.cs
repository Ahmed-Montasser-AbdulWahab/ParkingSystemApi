using Parking_System_API.Data.Entities;
using System.Threading.Tasks;

namespace Parking_System_API.Data.Repositories.CameraR
{
    public interface ICameraRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        Task<Camera[]> GetAllCameras(bool includeTerminal = false);
        Task<Camera> GetCameraById(int Id, bool includeTerminal = false);
        Task<Camera> GetCameraByConnectionString(string ConnectionString, bool includeTerminal = false);

    }
}
