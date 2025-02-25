using Microsoft.AspNetCore.Mvc;
using SterreWebApi.Models;


namespace SterreWebApi.Repositorys
{
    public interface IEnvironment2DRepository
    {
        Task<IEnumerable<Environment2D>> GetAllAsync();
        Task<Environment2D?> GetByIdAsync(Guid id);
        Task<Environment2D> AddAsync(Environment2D environment);
        Task<bool> UpdateAsync(Guid id, Environment2D updateEnvironment);
        Task<bool> DeleteAsync(Guid id);
    }
}