using Dapper;
using Microsoft.Data.SqlClient;
using VedicAPI.API.Models;
using VedicAPI.API.Repositories.Interfaces;

namespace VedicAPI.API.Repositories
{
    /// <summary>
    /// Repository for TextbookChapter data access operations
    /// </summary>
    public class TextbookChapterRepository : ITextbookChapterRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<TextbookChapterRepository> _logger;

        public TextbookChapterRepository(IConfiguration configuration, ILogger<TextbookChapterRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' not found");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get a textbook chapter by ID
        /// </summary>
        public async Task<TextbookChapter?> GetByIdAsync(int id)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);

            const string sql = @"
                SELECT Id, TextbookId, ChapterNumber, ChapterTitle, ChapterSubtitle, 
                       ContentHtml, Summary, WordCount, ReadingTimeMinutes, DisplayOrder,
                       CreatedAt, UpdatedAt, IsActive
                FROM TextbookChapters
                WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<TextbookChapter>(sql, new { Id = id });
        }

        /// <summary>
        /// Get all chapters for a specific textbook
        /// </summary>
        public async Task<IEnumerable<TextbookChapter>> GetByTextbookIdAsync(int textbookId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(textbookId);

            const string sql = @"
                SELECT Id, TextbookId, ChapterNumber, ChapterTitle, ChapterSubtitle, 
                       ContentHtml, Summary, WordCount, ReadingTimeMinutes, DisplayOrder,
                       CreatedAt, UpdatedAt, IsActive
                FROM TextbookChapters
                WHERE TextbookId = @TextbookId AND IsActive = 1
                ORDER BY DisplayOrder, ChapterNumber";

            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<TextbookChapter>(sql, new { TextbookId = textbookId });
        }

        /// <summary>
        /// Create a new textbook chapter
        /// </summary>
        public async Task<int> CreateAsync(TextbookChapter chapter)
        {
            ArgumentNullException.ThrowIfNull(chapter);

            const string sql = @"
                INSERT INTO TextbookChapters 
                (TextbookId, ChapterNumber, ChapterTitle, ChapterSubtitle, ContentHtml, 
                 Summary, WordCount, ReadingTimeMinutes, DisplayOrder, CreatedAt, IsActive)
                VALUES 
                (@TextbookId, @ChapterNumber, @ChapterTitle, @ChapterSubtitle, @ContentHtml, 
                 @Summary, @WordCount, @ReadingTimeMinutes, @DisplayOrder, @CreatedAt, @IsActive);
                
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var connection = new SqlConnection(_connectionString);
            var id = await connection.ExecuteScalarAsync<int>(sql, new
            {
                chapter.TextbookId,
                chapter.ChapterNumber,
                chapter.ChapterTitle,
                chapter.ChapterSubtitle,
                chapter.ContentHtml,
                chapter.Summary,
                chapter.WordCount,
                chapter.ReadingTimeMinutes,
                chapter.DisplayOrder,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            });

            // Update textbook's TotalChapters count
            await UpdateTextbookChapterCountAsync(connection, chapter.TextbookId);

            return id;
        }

        /// <summary>
        /// Update an existing textbook chapter
        /// </summary>
        public async Task<bool> UpdateAsync(TextbookChapter chapter)
        {
            ArgumentNullException.ThrowIfNull(chapter);

            const string sql = @"
                UPDATE TextbookChapters
                SET ChapterNumber = @ChapterNumber,
                    ChapterTitle = @ChapterTitle,
                    ChapterSubtitle = @ChapterSubtitle,
                    ContentHtml = @ContentHtml,
                    Summary = @Summary,
                    WordCount = @WordCount,
                    ReadingTimeMinutes = @ReadingTimeMinutes,
                    DisplayOrder = @DisplayOrder,
                    UpdatedAt = @UpdatedAt,
                    IsActive = @IsActive
                WHERE Id = @Id";

            using var connection = new SqlConnection(_connectionString);
            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                chapter.Id,
                chapter.ChapterNumber,
                chapter.ChapterTitle,
                chapter.ChapterSubtitle,
                chapter.ContentHtml,
                chapter.Summary,
                chapter.WordCount,
                chapter.ReadingTimeMinutes,
                chapter.DisplayOrder,
                UpdatedAt = DateTime.UtcNow,
                chapter.IsActive
            });

            return rowsAffected > 0;
        }

        /// <summary>
        /// Delete a textbook chapter
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);

            using var connection = new SqlConnection(_connectionString);
            
            // Get the textbook ID before deleting
            const string getTextbookIdSql = "SELECT TextbookId FROM TextbookChapters WHERE Id = @Id";
            var textbookId = await connection.ExecuteScalarAsync<int?>(getTextbookIdSql, new { Id = id });

            const string sql = "DELETE FROM TextbookChapters WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });

            // Update textbook's TotalChapters count if chapter was deleted
            if (rowsAffected > 0 && textbookId.HasValue)
            {
                await UpdateTextbookChapterCountAsync(connection, textbookId.Value);
            }

            return rowsAffected > 0;
        }

        /// <summary>
        /// Check if a textbook chapter exists
        /// </summary>
        public async Task<bool> ExistsAsync(int id)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);

            const string sql = "SELECT COUNT(1) FROM TextbookChapters WHERE Id = @Id";
            
            using var connection = new SqlConnection(_connectionString);
            var count = await connection.ExecuteScalarAsync<int>(sql, new { Id = id });
            return count > 0;
        }

        /// <summary>
        /// Check if a chapter number already exists for a textbook
        /// </summary>
        public async Task<bool> ChapterNumberExistsAsync(int textbookId, int chapterNumber, int? excludeChapterId = null)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(textbookId);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(chapterNumber);

            var sql = @"
                SELECT COUNT(1) 
                FROM TextbookChapters 
                WHERE TextbookId = @TextbookId 
                  AND ChapterNumber = @ChapterNumber";

            if (excludeChapterId.HasValue)
            {
                sql += " AND Id != @ExcludeChapterId";
            }

            using var connection = new SqlConnection(_connectionString);
            var count = await connection.ExecuteScalarAsync<int>(sql, new 
            { 
                TextbookId = textbookId, 
                ChapterNumber = chapterNumber,
                ExcludeChapterId = excludeChapterId
            });

            return count > 0;
        }

        /// <summary>
        /// Update the TotalChapters count for a textbook
        /// </summary>
        private static async Task UpdateTextbookChapterCountAsync(SqlConnection connection, int textbookId)
        {
            const string updateCountSql = @"
                UPDATE Textbooks
                SET TotalChapters = (
                    SELECT COUNT(*) 
                    FROM TextbookChapters 
                    WHERE TextbookId = @TextbookId AND IsActive = 1
                )
                WHERE Id = @TextbookId";

            await connection.ExecuteAsync(updateCountSql, new { TextbookId = textbookId });
        }
    }
}

