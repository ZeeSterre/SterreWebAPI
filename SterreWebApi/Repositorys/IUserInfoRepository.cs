using Microsoft.Data.SqlClient;
using SterreWebApi.Models;

namespace SterreWebApi.Repositorys
{
    public interface IUserInfoRepository
    {
        Task<Object2D?> GetObjectById(Guid objectId);
        Task<bool> DeleteObject(Guid objectId);
        Task<int> CreateObject(Object2D object2D);
        Task<IEnumerable<Object2D>> GetObjectsByEnvironmentId(Guid environmentId);
        Task<bool> DeleteEnvironment(Guid environmentId, Guid userId);
        Task<Environment2D?> GetEnvironmentById(Guid environmentId, Guid userId);
        Task<bool> CreateEnvironment(Environment2D environment);
        Task<IEnumerable<Environment2D>> GetEnvironmentsUser(Guid userId);
    }
}