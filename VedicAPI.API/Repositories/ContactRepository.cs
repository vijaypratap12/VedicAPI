using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using VedicAPI.API.Models;
using VedicAPI.API.Repositories.Interfaces;

namespace VedicAPI.API.Repositories
{
    /// <summary>
    /// Repository implementation for ContactSubmission entity using Dapper
    /// </summary>
    public class ContactRepository : IContactRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ContactRepository> _logger;

        public ContactRepository(IConfiguration configuration, ILogger<ContactRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string cannot be null");
            _logger = logger;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<ContactSubmission> CreateAsync(ContactSubmission contactSubmission)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    INSERT INTO CONTACTSUBMISSIONS 
                    (Id, Name, Email, Organization, Subject, Message, ContactType, Status, Priority, 
                     SubmittedAt, IpAddress, UserAgent, CreatedAt, UpdatedAt)
                    VALUES 
                    (@Id, @Name, @Email, @Organization, @Subject, @Message, @ContactType, @Status, @Priority,
                     @SubmittedAt, @IpAddress, @UserAgent, @CreatedAt, @UpdatedAt);
                    
                    SELECT * FROM CONTACTSUBMISSIONS WHERE Id = @Id;";

                var result = await connection.QuerySingleAsync<ContactSubmission>(sql, contactSubmission);
                _logger.LogInformation("Contact submission created with ID: {Id}", contactSubmission.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating contact submission");
                throw;
            }
        }

        public async Task<ContactSubmission?> GetByIdAsync(Guid id)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = "SELECT * FROM CONTACTSUBMISSIONS WHERE Id = @Id";
                return await connection.QuerySingleOrDefaultAsync<ContactSubmission>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contact submission with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<ContactSubmission>> GetAllAsync()
        {
            try
            {
                using var connection = CreateConnection();
                var sql = "SELECT * FROM CONTACTSUBMISSIONS WHERE IsArchived = 0 ORDER BY SubmittedAt DESC";
                return await connection.QueryAsync<ContactSubmission>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all contact submissions");
                throw;
            }
        }

        public async Task<IEnumerable<ContactSubmission>> GetByStatusAsync(string status)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"SELECT * FROM CONTACTSUBMISSIONS 
                           WHERE Status = @Status AND IsArchived = 0 
                           ORDER BY SubmittedAt DESC";
                return await connection.QueryAsync<ContactSubmission>(sql, new { Status = status });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contact submissions by status: {Status}", status);
                throw;
            }
        }

        public async Task<IEnumerable<ContactSubmission>> GetByContactTypeAsync(string contactType)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"SELECT * FROM CONTACTSUBMISSIONS 
                           WHERE ContactType = @ContactType AND IsArchived = 0 
                           ORDER BY SubmittedAt DESC";
                return await connection.QueryAsync<ContactSubmission>(sql, new { ContactType = contactType });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contact submissions by type: {ContactType}", contactType);
                throw;
            }
        }

        public async Task<IEnumerable<ContactSubmission>> GetByEmailAsync(string email)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"SELECT * FROM CONTACTSUBMISSIONS 
                           WHERE Email = @Email 
                           ORDER BY SubmittedAt DESC";
                return await connection.QueryAsync<ContactSubmission>(sql, new { Email = email });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contact submissions by email: {Email}", email);
                throw;
            }
        }

        public async Task<ContactSubmission> UpdateAsync(ContactSubmission contactSubmission)
        {
            try
            {
                using var connection = CreateConnection();
                contactSubmission.UpdatedAt = DateTime.UtcNow;

                var sql = @"
                    UPDATE CONTACTSUBMISSIONS 
                    SET Status = @Status, 
                        Priority = @Priority,
                        RespondedAt = @RespondedAt,
                        RespondedBy = @RespondedBy,
                        ResponseMessage = @ResponseMessage,
                        Notes = @Notes,
                        IsArchived = @IsArchived,
                        UpdatedAt = @UpdatedAt
                    WHERE Id = @Id;
                    
                    SELECT * FROM CONTACTSUBMISSIONS WHERE Id = @Id;";

                var result = await connection.QuerySingleAsync<ContactSubmission>(sql, contactSubmission);
                _logger.LogInformation("Contact submission updated with ID: {Id}", contactSubmission.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating contact submission with ID: {Id}", contactSubmission.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = "DELETE FROM CONTACTSUBMISSIONS WHERE Id = @Id";
                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
                _logger.LogInformation("Contact submission deleted with ID: {Id}", id);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting contact submission with ID: {Id}", id);
                throw;
            }
        }

        public async Task<int> GetCountByStatusAsync(string status)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = "SELECT COUNT(*) FROM CONTACTSUBMISSIONS WHERE Status = @Status AND IsArchived = 0";
                return await connection.ExecuteScalarAsync<int>(sql, new { Status = status });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting count by status: {Status}", status);
                throw;
            }
        }

        public async Task<IEnumerable<ContactSubmission>> GetRecentAsync(int count)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"SELECT TOP (@Count) * FROM CONTACTSUBMISSIONS 
                           WHERE IsArchived = 0 
                           ORDER BY SubmittedAt DESC";
                return await connection.QueryAsync<ContactSubmission>(sql, new { Count = count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving recent contact submissions");
                throw;
            }
        }
    }
}

