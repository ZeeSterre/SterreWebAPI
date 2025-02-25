using Dapper;
using Microsoft.Data.SqlClient;
using SterreWebApi.Models;

namespace SterreWebApi.Repositorys
{
    public class UserInfoRepository : IUserInfoRepository
    {
        private readonly string _connectionString;

        public UserInfoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Get all environments based on USERID (alleen de environments die gekoppeld zijn aan een gebruiker)
        public async Task<IEnumerable<Environment2D>> GetEnvironmentsUser(Guid userId)
        {

            using var connection = new SqlConnection(_connectionString);
            var query = "SELECT Id, Name, MaxLength, MaxHeight, CAST(UserId AS UNIQUEIDENTIFIER) AS UserId, EnvironmentType FROM dbo.Environment2D WHERE UserId = @UserId";
            
            return await connection.QueryAsync<Environment2D>(query, new { UserId = userId });
        }

        public async Task<Environment2D?> GetEnvironmentById(Guid environmentId, Guid userId)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
        SELECT 
            Id, Name, MaxLength, MaxHeight, 
            CAST(UserId AS UNIQUEIDENTIFIER) AS UserId, EnvironmentType
        FROM dbo.Environment2D 
        WHERE Id = @EnvironmentId AND UserId = @UserId";

            return await connection.QueryFirstOrDefaultAsync<Environment2D>(query, new { EnvironmentId = environmentId, UserId = userId });
        }

        public async Task<bool> CreateEnvironment(Environment2D environment)
        {
            using var connection = new SqlConnection(_connectionString);

            // krijg hoeveelheid environments terug per user
            var countQuery = "SELECT COUNT(*) FROM dbo.Environment2D WHERE UserId = @UserId";
            var environmentCount = await connection.ExecuteScalarAsync<int>(countQuery, new { UserId = environment.UserId });

            if (environmentCount >= 5)
                throw new InvalidOperationException("User cannot have more than 5 environments.");

            // checked voor al bestaande environments zodat we kunnen checken of ze dezelfde naam hebben
            var nameCheckQuery = "SELECT COUNT(*) FROM dbo.Environment2D WHERE UserId = @UserId AND Name = @Name";
            var existingNameCount = await connection.ExecuteScalarAsync<int>(nameCheckQuery, new { UserId = environment.UserId, Name = environment.Name });

            if (existingNameCount > 0)
                throw new InvalidOperationException("An environment with this name already exists for this user.");

            var insertQuery = @"
            INSERT INTO dbo.Environment2D (Id, Name, MaxLength, MaxHeight, UserId, EnvironmentType) 
            VALUES (@Id, @Name, @MaxLength, @MaxHeight, @UserId, @EnvironmentType)";

            var result = await connection.ExecuteAsync(insertQuery, environment);
            return result > 0;
        }

        public async Task<bool> DeleteEnvironment(Guid environmentId, Guid userId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                var checkQuery = "SELECT COUNT(*) FROM dbo.Environment2D WHERE Id = @EnvironmentId AND UserId = @UserId";
                var existingCount = await connection.ExecuteScalarAsync<int>(
                    checkQuery, new { EnvironmentId = environmentId, UserId = userId }, transaction);

                if (existingCount == 0)
                {
                    transaction.Rollback();
                    return false;
                }

                var deleteObjectsQuery = "DELETE FROM dbo.Object2D WHERE Environment2D_Id = @EnvironmentId";
                await connection.ExecuteAsync(deleteObjectsQuery, new { EnvironmentId = environmentId }, transaction);

                var deleteEnvironmentQuery = "DELETE FROM dbo.Environment2D WHERE Id = @EnvironmentId AND UserId = @UserId";
                var result = await connection.ExecuteAsync(deleteEnvironmentQuery, new { EnvironmentId = environmentId, UserId = userId }, transaction);

                transaction.Commit();
                return result > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<IEnumerable<Object2D>> GetObjectsByEnvironmentId(Guid environmentId)
        {
            using var connection = new SqlConnection(_connectionString);

            var query = @" SELECT 
                Id, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer, Environment2D_Id
            FROM 
                Object2D
            WHERE 
                Environment2D_Id = @EnvironmentId";

            var objects = await connection.QueryAsync<Object2D>(query, new { EnvironmentId = environmentId });
            return objects;
        }

        public async Task<int> CreateObject(Object2D object2D)
        {
            using var connection = new SqlConnection(_connectionString);

            var query = @"
            INSERT INTO Object2D (PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer, Environment2D_Id)
            VALUES (@PrefabId, @PositionX, @PositionY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer, @Environment2D_Id)";

            var result = await connection.ExecuteAsync(query, object2D);
            return result;
        }

        public async Task<Object2D?> GetObjectById(Guid objectId)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"
            SELECT 
                Id, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer, Environment2D_Id
            FROM 
                Object2D
            WHERE 
                Id = @ObjectId";
            return await connection.QueryFirstOrDefaultAsync<Object2D>(query, new { ObjectId = objectId });
        }

        // verwijder object
        public async Task<bool> DeleteObject(Guid objectId)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "DELETE FROM Object2D WHERE Id = @ObjectId";
            var result = await connection.ExecuteAsync(query, new { ObjectId = objectId });
            return result > 0;
        }

    }
}
