using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using VedicAPI.API.Models;
using VedicAPI.API.Repositories.Interfaces;

namespace VedicAPI.API.Repositories
{
    /// <summary>
    /// Repository implementation for Textbook entity using Dapper
    /// </summary>
    public class TextbookRepository : ITextbookRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<TextbookRepository> _logger;

        public TextbookRepository(IConfiguration configuration, ILogger<TextbookRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string cannot be null");
            _logger = logger;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<Textbook?> GetByIdAsync(int id)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT Id, Title, Author, Description, CoverImageUrl, TotalChapters, 
                           Category, Language, PublicationYear, ISBN, 
                           Rating, DownloadCount, ViewCount, Status, Tags, Level, Year, PageCount,
                           CreatedAt, UpdatedAt, IsActive
                    FROM TEXTBOOKS 
                    WHERE Id = @Id";
                
                return await connection.QueryFirstOrDefaultAsync<Textbook>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving textbook with ID {TextbookId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Textbook>> GetAllAsync()
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT Id, Title, Author, Description, CoverImageUrl, TotalChapters, 
                           Category, Language, PublicationYear, ISBN, 
                           Rating, DownloadCount, ViewCount, Status, Tags, Level, Year, PageCount,
                           CreatedAt, UpdatedAt, IsActive
                    FROM TEXTBOOKS 
                    ORDER BY CreatedAt DESC";
                
                return await connection.QueryAsync<Textbook>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all textbooks");
                throw;
            }
        }

        public async Task<IEnumerable<Textbook>> GetActiveAsync()
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT Id, Title, Author, Description, CoverImageUrl, TotalChapters, 
                           Category, Language, PublicationYear, ISBN, 
                           Rating, DownloadCount, ViewCount, Status, Tags, Level, Year, PageCount,
                           CreatedAt, UpdatedAt, IsActive
                    FROM TEXTBOOKS 
                    WHERE IsActive = 1
                    ORDER BY Rating DESC, CreatedAt DESC";
                
                return await connection.QueryAsync<Textbook>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active textbooks");
                throw;
            }
        }

        public async Task<IEnumerable<Textbook>> GetByCategoryAsync(string category)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT Id, Title, Author, Description, CoverImageUrl, TotalChapters, 
                           Category, Language, PublicationYear, ISBN, 
                           Rating, DownloadCount, ViewCount, Status, Tags, Level, Year, PageCount,
                           CreatedAt, UpdatedAt, IsActive
                    FROM TEXTBOOKS 
                    WHERE Category = @Category AND IsActive = 1
                    ORDER BY Rating DESC, CreatedAt DESC";
                
                return await connection.QueryAsync<Textbook>(sql, new { Category = category });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving textbooks by category {Category}", category);
                throw;
            }
        }

        public async Task<IEnumerable<Textbook>> GetByLevelAsync(string level)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT Id, Title, Author, Description, CoverImageUrl, TotalChapters, 
                           Category, Language, PublicationYear, ISBN, 
                           Rating, DownloadCount, ViewCount, Status, Tags, Level, Year, PageCount,
                           CreatedAt, UpdatedAt, IsActive
                    FROM TEXTBOOKS 
                    WHERE Level = @Level AND IsActive = 1
                    ORDER BY Rating DESC, CreatedAt DESC";
                
                return await connection.QueryAsync<Textbook>(sql, new { Level = level });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving textbooks by level {Level}", level);
                throw;
            }
        }

        public async Task<IEnumerable<Textbook>> SearchAsync(string searchTerm)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT Id, Title, Author, Description, CoverImageUrl, TotalChapters, 
                           Category, Language, PublicationYear, ISBN, 
                           Rating, DownloadCount, ViewCount, Status, Tags, Level, Year, PageCount,
                           CreatedAt, UpdatedAt, IsActive
                    FROM TEXTBOOKS 
                    WHERE (Title LIKE @SearchTerm 
                           OR Author LIKE @SearchTerm 
                           OR Description LIKE @SearchTerm
                           OR Category LIKE @SearchTerm
                           OR Tags LIKE @SearchTerm)
                          AND IsActive = 1
                    ORDER BY Rating DESC, CreatedAt DESC";
                
                var searchPattern = $"%{searchTerm}%";
                return await connection.QueryAsync<Textbook>(sql, new { SearchTerm = searchPattern });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching textbooks with term {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<int> CreateAsync(Textbook textbook)
        {
            if (textbook == null)
            {
                throw new ArgumentNullException(nameof(textbook));
            }

            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    INSERT INTO TEXTBOOKS (Title, Author, Description, CoverImageUrl, TotalChapters, 
                                          Category, Language, PublicationYear, ISBN, 
                                          Rating, DownloadCount, ViewCount, Status, Tags, Level, Year, PageCount,
                                          CreatedAt, IsActive)
                    VALUES (@Title, @Author, @Description, @CoverImageUrl, @TotalChapters, 
                           @Category, @Language, @PublicationYear, @ISBN, 
                           @Rating, @DownloadCount, @ViewCount, @Status, @Tags, @Level, @Year, @PageCount,
                           @CreatedAt, @IsActive);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
                
                textbook.CreatedAt = DateTime.UtcNow;
                return await connection.ExecuteScalarAsync<int>(sql, textbook);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating textbook {TextbookTitle}", textbook.Title);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Textbook textbook)
        {
            if (textbook == null)
            {
                throw new ArgumentNullException(nameof(textbook));
            }

            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    UPDATE TEXTBOOKS 
                    SET Title = @Title,
                        Author = @Author,
                        Description = @Description,
                        CoverImageUrl = @CoverImageUrl,
                        TotalChapters = @TotalChapters,
                        Category = @Category,
                        Language = @Language,
                        PublicationYear = @PublicationYear,
                        ISBN = @ISBN,
                        Rating = @Rating,
                        DownloadCount = @DownloadCount,
                        ViewCount = @ViewCount,
                        Status = @Status,
                        Tags = @Tags,
                        Level = @Level,
                        Year = @Year,
                        PageCount = @PageCount,
                        UpdatedAt = @UpdatedAt,
                        IsActive = @IsActive
                    WHERE Id = @Id";
                
                textbook.UpdatedAt = DateTime.UtcNow;
                var rowsAffected = await connection.ExecuteAsync(sql, textbook);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating textbook with ID {TextbookId}", textbook.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = "DELETE FROM TEXTBOOKS WHERE Id = @Id";
                
                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting textbook with ID {TextbookId}", id);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = "SELECT COUNT(1) FROM TEXTBOOKS WHERE Id = @Id";
                
                var count = await connection.ExecuteScalarAsync<int>(sql, new { Id = id });
                return count > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking existence of textbook with ID {TextbookId}", id);
                throw;
            }
        }

        public async Task<bool> IncrementDownloadCountAsync(int id)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    UPDATE TEXTBOOKS 
                    SET DownloadCount = DownloadCount + 1,
                        UpdatedAt = GETUTCDATE()
                    WHERE Id = @Id";
                
                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error incrementing download count for textbook ID {TextbookId}", id);
                throw;
            }
        }

        public async Task<bool> IncrementViewCountAsync(int id)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    UPDATE TEXTBOOKS 
                    SET ViewCount = ViewCount + 1,
                        UpdatedAt = GETUTCDATE()
                    WHERE Id = @Id";
                
                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error incrementing view count for textbook ID {TextbookId}", id);
                throw;
            }
        }
    }
}

