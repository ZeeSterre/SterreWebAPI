using Microsoft.Data.SqlClient;
using SterreWebApi.Models;
using Dapper;

namespace SterreWebApi.Repositorys
{
    public class Object2DRepository : IObject2DRepository
    {
        private readonly string _connectionString;

        public Object2DRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Object2D>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Object2D";
                return await connection.QueryAsync<Object2D>(query);
            }
        }

        public async Task<Object2D?> GetByIdAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Object2D WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Object2D>(query, new { Id = id });
            }
        }

        public async Task<Object2D> AddAsync(Object2D object2D)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    INSERT INTO Object2D (Id, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer)
                    VALUES (@Id, @PrefabId, @PositionX, @PositionY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer);";

                object2D.Id = Guid.NewGuid();
                await connection.ExecuteAsync(query, object2D);
                return object2D;
            }
        }

        public async Task<bool> UpdateAsync(Guid id, Object2D updateObject2D)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    UPDATE Object2D 
                    SET PrefabId = @PrefabId, PositionX = @PositionX, PositionY = @PositionY, 
                        ScaleX = @ScaleX, ScaleY = @ScaleY, RotationZ = @RotationZ, SortingLayer = @SortingLayer
                    WHERE Id = @Id";

                int rowsAffected = await connection.ExecuteAsync(query, new
                {
                    updateObject2D.PrefabId,
                    updateObject2D.PositionX,
                    updateObject2D.PositionY,
                    updateObject2D.ScaleX,
                    updateObject2D.ScaleY,
                    updateObject2D.RotationZ,
                    updateObject2D.SortingLayer,
                    Id = id
                });

                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Object2D WHERE Id = @Id";
                int rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
                return rowsAffected > 0;
            }
        }
    }
}
