using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using VedicAPI.API.Models;
using VedicAPI.API.Repositories.Interfaces;

namespace VedicAPI.API.Repositories
{
    /// <summary>
    /// Repository implementation for BookChapter entity using Dapper
    /// </summary>
    public class BookChapterRepository : IBookChapterRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<BookChapterRepository> _logger;

        public BookChapterRepository(IConfiguration configuration, ILogger<BookChapterRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string cannot be null");
            _logger = logger;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<BookChapter?> GetChapterByIdAsync(int id)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT Id, BookId, ChapterNumber, ChapterTitle, ChapterSubtitle, 
                           ContentHtml, Summary, WordCount, ReadingTimeMinutes, 
                           DisplayOrder, CreatedAt, UpdatedAt, IsActive
                    FROM BookChapters 
                    WHERE Id = @Id";

                return await connection.QueryFirstOrDefaultAsync<BookChapter>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving chapter with ID {ChapterId}", id);
                throw;
            }
        }

        public async Task<BookChapter?> GetChapterByNumberAsync(int bookId, int chapterNumber)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT Id, BookId, ChapterNumber, ChapterTitle, ChapterSubtitle, 
                           ContentHtml, Summary, WordCount, ReadingTimeMinutes, 
                           DisplayOrder, CreatedAt, UpdatedAt, IsActive
                    FROM BookChapters 
                    WHERE BookId = @BookId AND ChapterNumber = @ChapterNumber";

                return await connection.QueryFirstOrDefaultAsync<BookChapter>(
                    sql, 
                    new { BookId = bookId, ChapterNumber = chapterNumber });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving chapter {ChapterNumber} for book {BookId}", chapterNumber, bookId);
                throw;
            }
        }

        public async Task<IEnumerable<BookChapter>> GetChaptersByBookIdAsync(int bookId)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT Id, BookId, ChapterNumber, ChapterTitle, ChapterSubtitle, 
                           ContentHtml, Summary, WordCount, ReadingTimeMinutes, 
                           DisplayOrder, CreatedAt, UpdatedAt, IsActive
                    FROM BookChapters 
                    WHERE BookId = @BookId AND IsActive = 1
                    ORDER BY DisplayOrder, ChapterNumber";

                return await connection.QueryAsync<BookChapter>(sql, new { BookId = bookId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving chapters for book {BookId}", bookId);
                throw;
            }
        }

        public async Task<BookChapter?> GetNextChapterAsync(int bookId, int currentChapterNumber)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT TOP 1 Id, BookId, ChapterNumber, ChapterTitle, ChapterSubtitle, 
                           ContentHtml, Summary, WordCount, ReadingTimeMinutes, 
                           DisplayOrder, CreatedAt, UpdatedAt, IsActive
                    FROM BookChapters 
                    WHERE BookId = @BookId 
                      AND ChapterNumber > @CurrentChapterNumber 
                      AND IsActive = 1
                    ORDER BY ChapterNumber";

                return await connection.QueryFirstOrDefaultAsync<BookChapter>(
                    sql,
                    new { BookId = bookId, CurrentChapterNumber = currentChapterNumber });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving next chapter after {ChapterNumber} for book {BookId}", 
                    currentChapterNumber, bookId);
                throw;
            }
        }

        public async Task<BookChapter?> GetPreviousChapterAsync(int bookId, int currentChapterNumber)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT TOP 1 Id, BookId, ChapterNumber, ChapterTitle, ChapterSubtitle, 
                           ContentHtml, Summary, WordCount, ReadingTimeMinutes, 
                           DisplayOrder, CreatedAt, UpdatedAt, IsActive
                    FROM BookChapters 
                    WHERE BookId = @BookId 
                      AND ChapterNumber < @CurrentChapterNumber 
                      AND IsActive = 1
                    ORDER BY ChapterNumber DESC";

                return await connection.QueryFirstOrDefaultAsync<BookChapter>(
                    sql,
                    new { BookId = bookId, CurrentChapterNumber = currentChapterNumber });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving previous chapter before {ChapterNumber} for book {BookId}",
                    currentChapterNumber, bookId);
                throw;
            }
        }

        public async Task<int> CreateChapterAsync(BookChapter chapter)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    INSERT INTO BookChapters (BookId, ChapterNumber, ChapterTitle, ChapterSubtitle, 
                                            ContentHtml, Summary, WordCount, ReadingTimeMinutes, 
                                            DisplayOrder, CreatedAt, IsActive)
                    VALUES (@BookId, @ChapterNumber, @ChapterTitle, @ChapterSubtitle, 
                           @ContentHtml, @Summary, @WordCount, @ReadingTimeMinutes, 
                           @DisplayOrder, @CreatedAt, @IsActive);
                    SELECT CAST(SCOPE_IDENTITY() as int)";

                chapter.CreatedAt = DateTime.UtcNow;
                var chapterId = await connection.ExecuteScalarAsync<int>(sql, chapter);

                // Update book's total chapters count
                await UpdateBookChapterCountAsync(connection, chapter.BookId);

                return chapterId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating chapter for book {BookId}", chapter.BookId);
                throw;
            }
        }

        public async Task<bool> UpdateChapterAsync(BookChapter chapter)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    UPDATE BookChapters 
                    SET ChapterTitle = @ChapterTitle,
                        ChapterSubtitle = @ChapterSubtitle,
                        ContentHtml = @ContentHtml,
                        Summary = @Summary,
                        WordCount = @WordCount,
                        ReadingTimeMinutes = @ReadingTimeMinutes,
                        DisplayOrder = @DisplayOrder,
                        UpdatedAt = @UpdatedAt,
                        IsActive = @IsActive
                    WHERE Id = @Id";

                chapter.UpdatedAt = DateTime.UtcNow;
                var rowsAffected = await connection.ExecuteAsync(sql, chapter);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating chapter with ID {ChapterId}", chapter.Id);
                throw;
            }
        }

        public async Task<bool> DeleteChapterAsync(int id)
        {
            try
            {
                using var connection = CreateConnection();
                
                // Get the BookId before deleting
                var bookId = await connection.ExecuteScalarAsync<int>(
                    "SELECT BookId FROM BookChapters WHERE Id = @Id", 
                    new { Id = id });

                var sql = "DELETE FROM BookChapters WHERE Id = @Id";
                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });

                if (rowsAffected > 0 && bookId > 0)
                {
                    // Update book's total chapters count
                    await UpdateBookChapterCountAsync(connection, bookId);
                }

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting chapter with ID {ChapterId}", id);
                throw;
            }
        }

        public async Task<int> GetChapterCountByBookIdAsync(int bookId)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = "SELECT COUNT(*) FROM BookChapters WHERE BookId = @BookId AND IsActive = 1";
                return await connection.ExecuteScalarAsync<int>(sql, new { BookId = bookId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting chapter count for book {BookId}", bookId);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = "SELECT COUNT(1) FROM BookChapters WHERE Id = @Id";
                var count = await connection.ExecuteScalarAsync<int>(sql, new { Id = id });
                return count > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking existence of chapter with ID {ChapterId}", id);
                throw;
            }
        }

        private async Task UpdateBookChapterCountAsync(IDbConnection connection, int bookId)
        {
            var updateSql = @"
                UPDATE Books 
                SET TotalChapters = (
                    SELECT COUNT(*) 
                    FROM BookChapters 
                    WHERE BookId = @BookId AND IsActive = 1
                )
                WHERE Id = @BookId";

            await connection.ExecuteAsync(updateSql, new { BookId = bookId });
        }
    }
}

