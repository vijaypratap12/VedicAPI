using Dapper;
using Microsoft.Data.SqlClient;
using VedicAPI.API.Models;
using VedicAPI.API.Repositories.Interfaces;

namespace VedicAPI.API.Repositories
{
    /// <summary>
    /// Repository for Research Paper data access using Dapper
    /// </summary>
    public class ResearchPaperRepository : IResearchPaperRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ResearchPaperRepository> _logger;

        public ResearchPaperRepository(IConfiguration configuration, ILogger<ResearchPaperRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string not found");
            _logger = logger;
        }

        public async Task<ResearchPaper?> GetByIdAsync(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    SELECT * FROM RESEARCH_PAPERS 
                    WHERE Id = @Id AND IsActive = 1";
                
                var paper = await connection.QueryFirstOrDefaultAsync<ResearchPaper>(sql, new { Id = id });
                
                // Increment view count
                if (paper != null)
                {
                    await IncrementViewCountAsync(id);
                }
                
                return paper;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting research paper by ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<ResearchPaper>> GetAllAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    SELECT * FROM RESEARCH_PAPERS 
                    WHERE IsActive = 1 AND Status = 'published'
                    ORDER BY IsFeatured DESC, ViewCount DESC, Rating DESC";
                
                return await connection.QueryAsync<ResearchPaper>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all research papers");
                throw;
            }
        }

        public async Task<IEnumerable<ResearchPaper>> GetFeaturedAsync(int count = 3)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    SELECT TOP (@Count) * FROM RESEARCH_PAPERS 
                    WHERE IsActive = 1 AND IsFeatured = 1 AND Status = 'published'
                    ORDER BY Rating DESC, ViewCount DESC";
                
                return await connection.QueryAsync<ResearchPaper>(sql, new { Count = count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting featured research papers");
                throw;
            }
        }

        public async Task<IEnumerable<ResearchPaper>> GetByCategoryAsync(string category)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    SELECT * FROM RESEARCH_PAPERS 
                    WHERE IsActive = 1 AND Category = @Category AND Status = 'published'
                    ORDER BY IsFeatured DESC, ViewCount DESC, Rating DESC";
                
                return await connection.QueryAsync<ResearchPaper>(sql, new { Category = category });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting research papers by category: {Category}", category);
                throw;
            }
        }

        public async Task<IEnumerable<ResearchPaper>> GetByYearAsync(int year)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    SELECT * FROM RESEARCH_PAPERS 
                    WHERE IsActive = 1 AND Year = @Year AND Status = 'published'
                    ORDER BY IsFeatured DESC, ViewCount DESC, Rating DESC";
                
                return await connection.QueryAsync<ResearchPaper>(sql, new { Year = year });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting research papers by year: {Year}", year);
                throw;
            }
        }

        public async Task<IEnumerable<ResearchPaper>> GetByInstitutionAsync(string institution)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    SELECT * FROM RESEARCH_PAPERS 
                    WHERE IsActive = 1 AND Institution = @Institution AND Status = 'published'
                    ORDER BY IsFeatured DESC, ViewCount DESC, Rating DESC";
                
                return await connection.QueryAsync<ResearchPaper>(sql, new { Institution = institution });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting research papers by institution: {Institution}", institution);
                throw;
            }
        }

        public async Task<IEnumerable<ResearchPaper>> SearchAsync(string searchTerm)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    SELECT * FROM RESEARCH_PAPERS 
                    WHERE IsActive = 1 AND Status = 'published'
                        AND (Title LIKE @SearchTerm 
                             OR Authors LIKE @SearchTerm 
                             OR Keywords LIKE @SearchTerm 
                             OR Abstract LIKE @SearchTerm)
                    ORDER BY IsFeatured DESC, ViewCount DESC, Rating DESC";
                
                var searchPattern = $"%{searchTerm}%";
                return await connection.QueryAsync<ResearchPaper>(sql, new { SearchTerm = searchPattern });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching research papers with term: {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<ResearchPaper> CreateAsync(ResearchPaper paper)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    INSERT INTO RESEARCH_PAPERS (
                        Title, Authors, Institution, Year, Category, Abstract, ContentHtml,
                        Keywords, Pages, DownloadCount, ViewCount, Rating, Status, DOI,
                        JournalName, Volume, IssueNumber, PublicationDate, PdfUrl, CoverImageUrl,
                        IsFeatured, CreatedAt, UpdatedAt, IsActive
                    )
                    VALUES (
                        @Title, @Authors, @Institution, @Year, @Category, @Abstract, @ContentHtml,
                        @Keywords, @Pages, @DownloadCount, @ViewCount, @Rating, @Status, @DOI,
                        @JournalName, @Volume, @IssueNumber, @PublicationDate, @PdfUrl, @CoverImageUrl,
                        @IsFeatured, GETUTCDATE(), GETUTCDATE(), 1
                    );
                    SELECT CAST(SCOPE_IDENTITY() as int);";
                
                var id = await connection.ExecuteScalarAsync<int>(sql, paper);
                paper.Id = id;
                return paper;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating research paper");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(ResearchPaper paper)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    UPDATE RESEARCH_PAPERS SET
                        Title = @Title,
                        Authors = @Authors,
                        Institution = @Institution,
                        Year = @Year,
                        Category = @Category,
                        Abstract = @Abstract,
                        ContentHtml = @ContentHtml,
                        Keywords = @Keywords,
                        Pages = @Pages,
                        Rating = @Rating,
                        Status = @Status,
                        DOI = @DOI,
                        JournalName = @JournalName,
                        Volume = @Volume,
                        IssueNumber = @IssueNumber,
                        PublicationDate = @PublicationDate,
                        PdfUrl = @PdfUrl,
                        CoverImageUrl = @CoverImageUrl,
                        IsFeatured = @IsFeatured,
                        IsActive = @IsActive,
                        UpdatedAt = GETUTCDATE()
                    WHERE Id = @Id";
                
                var rowsAffected = await connection.ExecuteAsync(sql, paper);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating research paper: {Id}", paper.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    UPDATE RESEARCH_PAPERS 
                    SET IsActive = 0, UpdatedAt = GETUTCDATE() 
                    WHERE Id = @Id";
                
                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting research paper: {Id}", id);
                throw;
            }
        }

        public async Task<bool> IncrementViewCountAsync(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    UPDATE RESEARCH_PAPERS 
                    SET ViewCount = ViewCount + 1, UpdatedAt = GETUTCDATE() 
                    WHERE Id = @Id";
                
                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error incrementing view count for research paper: {Id}", id);
                throw;
            }
        }

        public async Task<bool> IncrementDownloadCountAsync(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    UPDATE RESEARCH_PAPERS 
                    SET DownloadCount = DownloadCount + 1, UpdatedAt = GETUTCDATE() 
                    WHERE Id = @Id";
                
                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error incrementing download count for research paper: {Id}", id);
                throw;
            }
        }
    }
}

