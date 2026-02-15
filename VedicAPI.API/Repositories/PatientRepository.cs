using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using VedicAPI.API.Models;
using VedicAPI.API.Repositories.Interfaces;

namespace VedicAPI.API.Repositories
{
    /// <summary>
    /// Repository implementation for Patient entity using Dapper
    /// </summary>
    public class PatientRepository : IPatientRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<PatientRepository> _logger;

        public PatientRepository(IConfiguration configuration, ILogger<PatientRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string cannot be null");
            _logger = logger;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<Patient?> GetByIdAsync(long id)
        {
            try
            {
                using var connection = CreateConnection();
                var result = await connection.QueryAsync<Patient>(
                    "sp_GetPatientById",
                    new { PatientId = id },
                    commandType: CommandType.StoredProcedure
                );
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patient with ID {PatientId}", id);
                throw;
            }
        }

        public async Task<Patient?> GetByUserIdAsync(int userId)
        {
            try
            {
                using var connection = CreateConnection();
                var result = await connection.QueryAsync<Patient>(
                    "sp_GetPatientByUserId",
                    new { UserId = userId },
                    commandType: CommandType.StoredProcedure
                );
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving patient with UserID {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT Id, UserId, Name, Age, Gender, ContactNumber, Email, Address,
                           Prakriti, PrakritiScore, CreatedAt, UpdatedAt, IsActive
                    FROM PATIENTS
                    WHERE IsActive = 1
                    ORDER BY CreatedAt DESC";

                return await connection.QueryAsync<Patient>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all patients");
                throw;
            }
        }

        public async Task<long> CreateAsync(Patient patient)
        {
            try
            {
                using var connection = CreateConnection();
                var result = await connection.QueryAsync<long>(
                    "sp_CreatePatient",
                    new
                    {
                        patient.UserId,
                        patient.Name,
                        patient.Age,
                        patient.Gender,
                        patient.ContactNumber,
                        patient.Email,
                        patient.Address
                    },
                    commandType: CommandType.StoredProcedure
                );
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating patient {PatientName}", patient.Name);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Patient patient)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    UPDATE PATIENTS
                    SET Name = @Name,
                        Age = @Age,
                        Gender = @Gender,
                        ContactNumber = @ContactNumber,
                        Email = @Email,
                        Address = @Address,
                        UpdatedAt = GETUTCDATE()
                    WHERE Id = @Id";

                var rowsAffected = await connection.ExecuteAsync(sql, patient);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating patient with ID {PatientId}", patient.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    UPDATE PATIENTS
                    SET IsActive = 0,
                        UpdatedAt = GETUTCDATE()
                    WHERE Id = @Id";

                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting patient with ID {PatientId}", id);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(long id)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = "SELECT COUNT(1) FROM PATIENTS WHERE Id = @Id AND IsActive = 1";

                var count = await connection.ExecuteScalarAsync<int>(sql, new { Id = id });
                return count > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking existence of patient with ID {PatientId}", id);
                throw;
            }
        }
    }
}
