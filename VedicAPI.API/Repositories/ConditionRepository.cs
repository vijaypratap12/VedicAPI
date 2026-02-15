using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using VedicAPI.API.Models;
using VedicAPI.API.Repositories.Interfaces;

namespace VedicAPI.API.Repositories
{
    /// <summary>
    /// Repository implementation for Condition entity using Dapper
    /// </summary>
    public class ConditionRepository : IConditionRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ConditionRepository> _logger;

        public ConditionRepository(IConfiguration configuration, ILogger<ConditionRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string cannot be null");
            _logger = logger;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<Condition?> GetByIdAsync(long id)
        {
            try
            {
                using var connection = CreateConnection();
                var result = await connection.QueryAsync<Condition>(
                    "sp_GetConditionById",
                    new { ConditionId = id },
                    commandType: CommandType.StoredProcedure
                );
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving condition with ID {ConditionId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Condition>> GetAllAsync()
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT Id, Name, SanskritName, Category, Description, CommonSymptoms,
                           AffectedDoshas, Severity, CreatedAt, UpdatedAt, IsActive
                    FROM CONDITIONS
                    WHERE IsActive = 1
                    ORDER BY Name";

                return await connection.QueryAsync<Condition>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all conditions");
                throw;
            }
        }

        public async Task<IEnumerable<Condition>> SearchAsync(string searchTerm, string? category = null)
        {
            try
            {
                using var connection = CreateConnection();
                return await connection.QueryAsync<Condition>(
                    "sp_SearchConditions",
                    new { SearchTerm = searchTerm, Category = category },
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching conditions with term {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<IEnumerable<Condition>> GetByCategoryAsync(string category)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT Id, Name, SanskritName, Category, Description, CommonSymptoms,
                           AffectedDoshas, Severity, CreatedAt, UpdatedAt, IsActive
                    FROM CONDITIONS
                    WHERE Category = @Category AND IsActive = 1
                    ORDER BY Name";

                return await connection.QueryAsync<Condition>(sql, new { Category = category });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving conditions by category {Category}", category);
                throw;
            }
        }
    }
}
