using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using VedicAPI.API.Models;
using VedicAPI.API.Repositories.Interfaces;

namespace VedicAPI.API.Repositories
{
    /// <summary>
    /// Repository implementation for YogaAsana entity using Dapper
    /// </summary>
    public class YogaAsanaRepository : IYogaAsanaRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<YogaAsanaRepository> _logger;

        public YogaAsanaRepository(IConfiguration configuration, ILogger<YogaAsanaRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string cannot be null");
            _logger = logger;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<YogaAsana?> GetByIdAsync(long id)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT Id, AsanaName, SanskritName, Category, Benefits, Duration,
                           Difficulty, Instructions, Precautions, ImageUrl, VideoUrl,
                           VataEffect, PittaEffect, KaphaEffect,
                           CreatedAt, UpdatedAt, IsActive
                    FROM YOGAASANAS
                    WHERE Id = @Id AND IsActive = 1";

                var result = await connection.QueryAsync<YogaAsana>(sql, new { Id = id });
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving yoga asana with ID {AsanaId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<YogaAsana>> GetAllAsync()
        {
            try
            {
                using var connection = CreateConnection();
                return await connection.QueryAsync<YogaAsana>(
                    "sp_GetYogaAsanas",
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all yoga asanas");
                throw;
            }
        }

        public async Task<IEnumerable<YogaAsana>> GetByCategoryAsync(string category)
        {
            try
            {
                using var connection = CreateConnection();
                return await connection.QueryAsync<YogaAsana>(
                    "sp_GetYogaAsanas",
                    new { Category = category },
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving yoga asanas by category {Category}", category);
                throw;
            }
        }

        public async Task<IEnumerable<YogaAsana>> GetByDifficultyAsync(string difficulty)
        {
            try
            {
                using var connection = CreateConnection();
                return await connection.QueryAsync<YogaAsana>(
                    "sp_GetYogaAsanas",
                    new { Difficulty = difficulty },
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving yoga asanas by difficulty {Difficulty}", difficulty);
                throw;
            }
        }

        public async Task<IEnumerable<YogaAsana>> GetByPrakritiEffectAsync(string prakriti)
        {
            try
            {
                using var connection = CreateConnection();
                return await connection.QueryAsync<YogaAsana>(
                    "sp_GetYogaAsanas",
                    new { PrakritiEffect = prakriti },
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving yoga asanas for Prakriti {Prakriti}", prakriti);
                throw;
            }
        }
    }
}
