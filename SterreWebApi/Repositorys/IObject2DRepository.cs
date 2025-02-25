
namespace SterreWebApi.Repositorys
{
    public interface IObject2DRepository
    {
        Task<IEnumerable<Object2D>> GetAllAsync();
        Task<Object2D?> GetByIdAsync(Guid id);
        Task<Object2D> AddAsync(Object2D object2D);
        Task<bool> UpdateAsync(Guid id, Object2D updateObject2D);
        Task<bool> DeleteAsync(Guid id);
    }

   
}
