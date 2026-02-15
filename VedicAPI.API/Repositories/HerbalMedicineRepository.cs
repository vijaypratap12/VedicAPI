using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using VedicAPI.API.Models;
using VedicAPI.API.Repositories.Interfaces;

namespace VedicAPI.API.Repositories
{
    /// <summary>
    /// Repository implementation for HerbalMedicine entity using Dapper
    /// </summary>
    public class HerbalMedicineRepository : IHerbalMedicineRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<HerbalMedicineRepository> _logger;

        public HerbalMedicineRepository(IConfiguration configuration, ILogger<HerbalMedicineRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string cannot be null");
            _logger = logger;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<HerbalMedicine?> GetByIdAsync(long id)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT Id, CommonName, SanskritName, ScientificName, HindiName,
                           Properties, Indications, Dosage, Contraindications, SideEffects,
                           ImageUrl, VataEffect, PittaEffect, KaphaEffect,
                           CreatedAt, UpdatedAt, IsActive
                    FROM HERBALMEDICINES
                    WHERE Id = @Id AND IsActive = 1";

                var result = await connection.QueryAsync<HerbalMedicine>(sql, new { Id = id });
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving herbal medicine with ID {MedicineId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<HerbalMedicine>> GetAllAsync()
        {
            try
            {
                using var connection = CreateConnection();
                return await connection.QueryAsync<HerbalMedicine>(
                    "sp_GetHerbalMedicines",
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all herbal medicines");
                throw;
            }
        }

        public async Task<IEnumerable<HerbalMedicine>> SearchAsync(string searchTerm)
        {
            try
            {
                using var connection = CreateConnection();
                return await connection.QueryAsync<HerbalMedicine>(
                    "sp_GetHerbalMedicines",
                    new { SearchTerm = searchTerm },
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching herbal medicines with term {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<IEnumerable<HerbalMedicine>> GetByPrakritiEffectAsync(string prakriti)
        {
            try
            {
                using var connection = CreateConnection();
                return await connection.QueryAsync<HerbalMedicine>(
                    "sp_GetHerbalMedicines",
                    new { PrakritiEffect = prakriti },
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving herbal medicines for Prakriti {Prakriti}", prakriti);
                throw;
            }
        }
    }
}
