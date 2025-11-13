using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using VedicAPI.API.Models;
using VedicAPI.API.Repositories.Interfaces;

namespace VedicAPI.API.Repositories
{
    /// <summary>
    /// Repository implementation for Book entity using Dapper
    /// </summary>
    public class BookRepository : IBookRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(IConfiguration configuration, ILogger<BookRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string cannot be null");
            _logger = logger;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<Book?> GetByIdAsync(int id)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT Id, Title, Author, Description, CoverImageUrl, TotalChapters, 
                           Category, Language, PublicationYear, ISBN, CreatedAt, UpdatedAt, IsActive
                    FROM Books 
                    WHERE Id = @Id";
                
                return await connection.QueryFirstOrDefaultAsync<Book>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving book with ID {BookId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT Id, Title, Author, Description, CoverImageUrl, TotalChapters, 
                           Category, Language, PublicationYear, ISBN, CreatedAt, UpdatedAt, IsActive
                    FROM Books 
                    ORDER BY CreatedAt DESC";
                
                return await connection.QueryAsync<Book>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all books");
                throw;
            }
        }

        public async Task<IEnumerable<Book>> GetActiveAsync()
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT Id, Title, Author, Description, CoverImageUrl, TotalChapters, 
                           Category, Language, PublicationYear, ISBN, CreatedAt, UpdatedAt, IsActive
                    FROM Books 
                    WHERE IsActive = 1
                    ORDER BY CreatedAt DESC";
                
                return await connection.QueryAsync<Book>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active books");
                throw;
            }
        }

        public async Task<IEnumerable<Book>> GetByCategoryAsync(string category)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT Id, Title, Author, Description, CoverImageUrl, TotalChapters, 
                           Category, Language, PublicationYear, ISBN, CreatedAt, UpdatedAt, IsActive
                    FROM Books 
                    WHERE Category = @Category AND IsActive = 1
                    ORDER BY CreatedAt DESC";
                
                return await connection.QueryAsync<Book>(sql, new { Category = category });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving books by category {Category}", category);
                throw;
            }
        }

        public async Task<IEnumerable<Book>> SearchAsync(string searchTerm)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT Id, Title, Author, Description, CoverImageUrl, TotalChapters, 
                           Category, Language, PublicationYear, ISBN, CreatedAt, UpdatedAt, IsActive
                    FROM Books 
                    WHERE (Title LIKE @SearchTerm 
                           OR Author LIKE @SearchTerm 
                           OR Description LIKE @SearchTerm
                           OR Category LIKE @SearchTerm)
                          AND IsActive = 1
                    ORDER BY CreatedAt DESC";
                
                var searchPattern = $"%{searchTerm}%";
                return await connection.QueryAsync<Book>(sql, new { SearchTerm = searchPattern });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching books with term {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<int> CreateAsync(Book book)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    INSERT INTO Books (Title, Author, Description, CoverImageUrl, TotalChapters, 
                                      Category, Language, PublicationYear, ISBN, CreatedAt, IsActive)
                    VALUES (@Title, @Author, @Description, @CoverImageUrl, @TotalChapters, 
                           @Category, @Language, @PublicationYear, @ISBN, @CreatedAt, @IsActive);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
                
                book.CreatedAt = DateTime.UtcNow;
                return await connection.ExecuteScalarAsync<int>(sql, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating book {BookTitle}", book.Title);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Book book)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    UPDATE Books 
                    SET Title = @Title,
                        Author = @Author,
                        Description = @Description,
                        CoverImageUrl = @CoverImageUrl,
                        TotalChapters = @TotalChapters,
                        Category = @Category,
                        Language = @Language,
                        PublicationYear = @PublicationYear,
                        ISBN = @ISBN,
                        UpdatedAt = @UpdatedAt,
                        IsActive = @IsActive
                    WHERE Id = @Id";
                
                book.UpdatedAt = DateTime.UtcNow;
                var rowsAffected = await connection.ExecuteAsync(sql, book);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book with ID {BookId}", book.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = "DELETE FROM Books WHERE Id = @Id";
                
                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book with ID {BookId}", id);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = "SELECT COUNT(1) FROM Books WHERE Id = @Id";
                
                var count = await connection.ExecuteScalarAsync<int>(sql, new { Id = id });
                return count > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking existence of book with ID {BookId}", id);
                throw;
            }
        }
    }
}

