using Microsoft.AspNetCore.Mvc;
using SterreWebApi.Models;


namespace SterreWebApi.Repositorys
{
    public interface IEnvironment2DRepository
    {
        Task<IEnumerable<Environment2D>> GetAllAsync();
        Task<Environment2D?> GetByIdAsync(int id);
        Task<Environment2D> AddAsync(Environment2D environment);
        Task<bool> UpdateAsync(int id, Environment2D updateEnvironment);
        Task<bool> DeleteAsync(int id);
    }
}