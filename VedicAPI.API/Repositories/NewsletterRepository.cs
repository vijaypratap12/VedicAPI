using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using VedicAPI.API.Models;
using VedicAPI.API.Repositories.Interfaces;

namespace VedicAPI.API.Repositories
{
    /// <summary>
    /// Repository implementation for NewsletterSubscription entity using Dapper
    /// </summary>
    public class NewsletterRepository : INewsletterRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<NewsletterRepository> _logger;

        public NewsletterRepository(IConfiguration configuration, ILogger<NewsletterRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string cannot be null");
            _logger = logger;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<NewsletterSubscription> CreateAsync(NewsletterSubscription subscription)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    INSERT INTO NEWSLETTERSUBSCRIPTIONS 
                    (Id, Email, SubscribedAt, IsActive, UnsubscribeToken, Source, IpAddress, 
                     ConfirmationSent, CreatedAt, UpdatedAt)
                    VALUES 
                    (@Id, @Email, @SubscribedAt, @IsActive, @UnsubscribeToken, @Source, @IpAddress,
                     @ConfirmationSent, @CreatedAt, @UpdatedAt);
                    
                    SELECT * FROM NEWSLETTERSUBSCRIPTIONS WHERE Id = @Id;";

                var result = await connection.QuerySingleAsync<NewsletterSubscription>(sql, subscription);
                _logger.LogInformation("Newsletter subscription created for email: {Email}", subscription.Email);
                return result;
            }
            catch (SqlException ex) when (ex.Number == 2627) // Unique constraint violation
            {
                _logger.LogWarning("Duplicate newsletter subscription attempt for email: {Email}", subscription.Email);
                throw new InvalidOperationException($"Email {subscription.Email} is already subscribed to the newsletter.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating newsletter subscription");
                throw;
            }
        }

        public async Task<NewsletterSubscription?> GetByIdAsync(Guid id)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = "SELECT * FROM NEWSLETTERSUBSCRIPTIONS WHERE Id = @Id";
                return await connection.QuerySingleOrDefaultAsync<NewsletterSubscription>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving newsletter subscription with ID: {Id}", id);
                throw;
            }
        }

        public async Task<NewsletterSubscription?> GetByEmailAsync(string email)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = "SELECT * FROM NEWSLETTERSUBSCRIPTIONS WHERE Email = @Email";
                return await connection.QuerySingleOrDefaultAsync<NewsletterSubscription>(sql, new { Email = email });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving newsletter subscription by email: {Email}", email);
                throw;
            }
        }

        public async Task<IEnumerable<NewsletterSubscription>> GetAllActiveAsync()
        {
            try
            {
                using var connection = CreateConnection();
                var sql = "SELECT * FROM NEWSLETTERSUBSCRIPTIONS WHERE IsActive = 1 ORDER BY SubscribedAt DESC";
                return await connection.QueryAsync<NewsletterSubscription>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active newsletter subscriptions");
                throw;
            }
        }

        public async Task<IEnumerable<NewsletterSubscription>> GetAllAsync()
        {
            try
            {
                using var connection = CreateConnection();
                var sql = "SELECT * FROM NEWSLETTERSUBSCRIPTIONS ORDER BY SubscribedAt DESC";
                return await connection.QueryAsync<NewsletterSubscription>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all newsletter subscriptions");
                throw;
            }
        }

        public async Task<NewsletterSubscription> UpdateAsync(NewsletterSubscription subscription)
        {
            try
            {
                using var connection = CreateConnection();
                subscription.UpdatedAt = DateTime.UtcNow;

                var sql = @"
                    UPDATE NEWSLETTERSUBSCRIPTIONS 
                    SET IsActive = @IsActive,
                        UnsubscribedAt = @UnsubscribedAt,
                        ConfirmationSent = @ConfirmationSent,
                        ConfirmationSentAt = @ConfirmationSentAt,
                        EmailsSentCount = @EmailsSentCount,
                        LastEmailSentAt = @LastEmailSentAt,
                        UpdatedAt = @UpdatedAt
                    WHERE Id = @Id;
                    
                    SELECT * FROM NEWSLETTERSUBSCRIPTIONS WHERE Id = @Id;";

                var result = await connection.QuerySingleAsync<NewsletterSubscription>(sql, subscription);
                _logger.LogInformation("Newsletter subscription updated for ID: {Id}", subscription.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating newsletter subscription with ID: {Id}", subscription.Id);
                throw;
            }
        }

        public async Task<bool> UnsubscribeAsync(Guid unsubscribeToken)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    UPDATE NEWSLETTERSUBSCRIPTIONS 
                    SET IsActive = 0, 
                        UnsubscribedAt = @UnsubscribedAt,
                        UpdatedAt = @UpdatedAt
                    WHERE UnsubscribeToken = @UnsubscribeToken AND IsActive = 1";

                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    UnsubscribeToken = unsubscribeToken,
                    UnsubscribedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });

                if (rowsAffected > 0)
                {
                    _logger.LogInformation("Newsletter unsubscribed using token: {Token}", unsubscribeToken);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unsubscribing newsletter with token: {Token}", unsubscribeToken);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(string email)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = "SELECT COUNT(1) FROM NEWSLETTERSUBSCRIPTIONS WHERE Email = @Email";
                var count = await connection.ExecuteScalarAsync<int>(sql, new { Email = email });
                return count > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if email exists: {Email}", email);
                throw;
            }
        }

        public async Task<int> GetActiveCountAsync()
        {
            try
            {
                using var connection = CreateConnection();
                var sql = "SELECT COUNT(*) FROM NEWSLETTERSUBSCRIPTIONS WHERE IsActive = 1";
                return await connection.ExecuteScalarAsync<int>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active newsletter count");
                throw;
            }
        }

        public async Task<IEnumerable<NewsletterSubscription>> GetRecentAsync(int count)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"SELECT TOP (@Count) * FROM NEWSLETTERSUBSCRIPTIONS 
                           WHERE IsActive = 1 
                           ORDER BY SubscribedAt DESC";
                return await connection.QueryAsync<NewsletterSubscription>(sql, new { Count = count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving recent newsletter subscriptions");
                throw;
            }
        }
    }
}

