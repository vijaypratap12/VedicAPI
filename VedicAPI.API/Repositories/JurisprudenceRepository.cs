using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using VedicAPI.API.Models;
using VedicAPI.API.Repositories.Interfaces;

namespace VedicAPI.API.Repositories
{
    /// <summary>
    /// Repository for Jurisprudence data access using Dapper
    /// </summary>
    public class JurisprudenceRepository : IJurisprudenceRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<JurisprudenceRepository> _logger;

        public JurisprudenceRepository(IConfiguration configuration, ILogger<JurisprudenceRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string not found");
            _logger = logger;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<(int TotalCount, IEnumerable<JurisprudenceItem> Items)> GetPagedAsync(
            string? searchTerm,
            string? type,
            int pageNumber,
            int pageSize,
            bool includeInactive = false,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = CreateConnection();
                var parameters = new
                {
                    SearchTerm = searchTerm,
                    Type = type,
                    IncludeInactive = includeInactive ? 1 : 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                using var multi = await connection.QueryMultipleAsync(
                    "sp_GetJurisprudencePaged",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                var totalCount = await multi.ReadFirstOrDefaultAsync<int>();
                var items = await multi.ReadAsync<JurisprudenceItem>();

                return (totalCount, items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged jurisprudence items");
                throw;
            }
        }

        public async Task<JurisprudenceItem?> GetByIdAsync(
            int id,
            bool includeInactive = false,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = CreateConnection();
                var parameters = new
                {
                    Id = id,
                    IncludeInactive = includeInactive ? 1 : 0
                };

                var item = await connection.QueryFirstOrDefaultAsync<JurisprudenceItem>(
                    "sp_GetJurisprudenceById",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting jurisprudence item by ID: {Id}", id);
                throw;
            }
        }

        public async Task<int> CreateAsync(
            JurisprudenceItem entity,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = CreateConnection();
                var parameters = new
                {
                    Title = entity.Title,
                    Type = entity.Type,
                    Date = entity.Date,
                    Description = entity.Description,
                    DocumentUrl = entity.DocumentUrl,
                    State = entity.State,
                    DisplayOrder = entity.DisplayOrder
                };

                var id = await connection.QuerySingleAsync<int>(
                    "sp_CreateJurisprudence",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating jurisprudence item");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(
            JurisprudenceItem entity,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = CreateConnection();
                var parameters = new
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Type = entity.Type,
                    Date = entity.Date,
                    Description = entity.Description,
                    DocumentUrl = entity.DocumentUrl,
                    State = entity.State,
                    DisplayOrder = entity.DisplayOrder
                };

                var rowsAffected = await connection.QuerySingleAsync<int>(
                    "sp_UpdateJurisprudence",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating jurisprudence item: {Id}", entity.Id);
                throw;
            }
        }

        public async Task<bool> SoftDeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = CreateConnection();
                var parameters = new { Id = id };

                var rowsAffected = await connection.QuerySingleAsync<int>(
                    "sp_DeleteJurisprudence",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting jurisprudence item: {Id}", id);
                throw;
            }
        }
    }
}
