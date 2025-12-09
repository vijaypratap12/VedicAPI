using Dapper;
using Microsoft.Data.SqlClient;
using VedicAPI.API.Models;
using VedicAPI.API.Repositories.Interfaces;

namespace VedicAPI.API.Repositories
{
    /// <summary>
    /// Repository for Thesis data access using Dapper
    /// </summary>
    public class ThesisRepository : IThesisRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ThesisRepository> _logger;

        public ThesisRepository(IConfiguration configuration, ILogger<ThesisRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string not found");
            _logger = logger;
        }

        public async Task<Thesis?> GetByIdAsync(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    SELECT * FROM THESIS_REPOSITORY 
                    WHERE Id = @Id AND IsActive = 1";
                
                var thesis = await connection.QueryFirstOrDefaultAsync<Thesis>(sql, new { Id = id });
                
                // Increment view count
                if (thesis != null)
                {
                    await IncrementViewCountAsync(id);
                }
                
                return thesis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting thesis by ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Thesis>> GetAllAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    SELECT * FROM THESIS_REPOSITORY 
                    WHERE IsActive = 1 AND Status = 'published'
                    ORDER BY IsFeatured DESC, ViewCount DESC, Rating DESC";
                
                return await connection.QueryAsync<Thesis>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all thesis");
                throw;
            }
        }

        public async Task<IEnumerable<Thesis>> GetFeaturedAsync(int count = 3)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    SELECT TOP (@Count) * FROM THESIS_REPOSITORY 
                    WHERE IsActive = 1 AND IsFeatured = 1 AND Status = 'published'
                    ORDER BY Rating DESC, ViewCount DESC";
                
                return await connection.QueryAsync<Thesis>(sql, new { Count = count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting featured thesis");
                throw;
            }
        }

        public async Task<IEnumerable<Thesis>> GetByCategoryAsync(string category)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    SELECT * FROM THESIS_REPOSITORY 
                    WHERE IsActive = 1 AND Category = @Category AND Status = 'published'
                    ORDER BY IsFeatured DESC, ViewCount DESC, Rating DESC";
                
                return await connection.QueryAsync<Thesis>(sql, new { Category = category });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting thesis by category: {Category}", category);
                throw;
            }
        }

        public async Task<IEnumerable<Thesis>> GetByYearAsync(int year)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    SELECT * FROM THESIS_REPOSITORY 
                    WHERE IsActive = 1 AND Year = @Year AND Status = 'published'
                    ORDER BY IsFeatured DESC, ViewCount DESC, Rating DESC";
                
                return await connection.QueryAsync<Thesis>(sql, new { Year = year });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting thesis by year: {Year}", year);
                throw;
            }
        }

        public async Task<IEnumerable<Thesis>> GetByInstitutionAsync(string institution)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    SELECT * FROM THESIS_REPOSITORY 
                    WHERE IsActive = 1 AND Institution = @Institution AND Status = 'published'
                    ORDER BY IsFeatured DESC, ViewCount DESC, Rating DESC";
                
                return await connection.QueryAsync<Thesis>(sql, new { Institution = institution });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting thesis by institution: {Institution}", institution);
                throw;
            }
        }

        public async Task<IEnumerable<Thesis>> SearchAsync(string searchTerm)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    SELECT * FROM THESIS_REPOSITORY 
                    WHERE IsActive = 1 AND Status = 'published'
                        AND (Title LIKE @SearchTerm 
                             OR Author LIKE @SearchTerm 
                             OR Keywords LIKE @SearchTerm 
                             OR Abstract LIKE @SearchTerm)
                    ORDER BY IsFeatured DESC, ViewCount DESC, Rating DESC";
                
                var searchPattern = $"%{searchTerm}%";
                return await connection.QueryAsync<Thesis>(sql, new { SearchTerm = searchPattern });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching thesis with term: {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<Thesis> CreateAsync(Thesis thesis)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    INSERT INTO THESIS_REPOSITORY (
                        Title, Author, GuideNames, Institution, Department, Year, Category,
                        ThesisType, Abstract, ContentHtml, Keywords, Pages, DownloadCount,
                        ViewCount, Rating, Status, SubmissionDate, ApprovalDate, DefenseDate,
                        Grade, PdfUrl, CoverImageUrl, UniversityRegistrationNumber,
                        IsFeatured, CreatedAt, UpdatedAt, IsActive
                    )
                    VALUES (
                        @Title, @Author, @GuideNames, @Institution, @Department, @Year, @Category,
                        @ThesisType, @Abstract, @ContentHtml, @Keywords, @Pages, @DownloadCount,
                        @ViewCount, @Rating, @Status, @SubmissionDate, @ApprovalDate, @DefenseDate,
                        @Grade, @PdfUrl, @CoverImageUrl, @UniversityRegistrationNumber,
                        @IsFeatured, GETUTCDATE(), GETUTCDATE(), 1
                    );
                    SELECT CAST(SCOPE_IDENTITY() as int);";
                
                var id = await connection.ExecuteScalarAsync<int>(sql, thesis);
                thesis.Id = id;
                return thesis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating thesis");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Thesis thesis)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    UPDATE THESIS_REPOSITORY SET
                        Title = @Title,
                        Author = @Author,
                        GuideNames = @GuideNames,
                        Institution = @Institution,
                        Department = @Department,
                        Year = @Year,
                        Category = @Category,
                        ThesisType = @ThesisType,
                        Abstract = @Abstract,
                        ContentHtml = @ContentHtml,
                        Keywords = @Keywords,
                        Pages = @Pages,
                        Rating = @Rating,
                        Status = @Status,
                        SubmissionDate = @SubmissionDate,
                        ApprovalDate = @ApprovalDate,
                        DefenseDate = @DefenseDate,
                        Grade = @Grade,
                        PdfUrl = @PdfUrl,
                        CoverImageUrl = @CoverImageUrl,
                        UniversityRegistrationNumber = @UniversityRegistrationNumber,
                        IsFeatured = @IsFeatured,
                        IsActive = @IsActive,
                        UpdatedAt = GETUTCDATE()
                    WHERE Id = @Id";
                
                var rowsAffected = await connection.ExecuteAsync(sql, thesis);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating thesis: {Id}", thesis.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    UPDATE THESIS_REPOSITORY 
                    SET IsActive = 0, UpdatedAt = GETUTCDATE() 
                    WHERE Id = @Id";
                
                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting thesis: {Id}", id);
                throw;
            }
        }

        public async Task<bool> IncrementViewCountAsync(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    UPDATE THESIS_REPOSITORY 
                    SET ViewCount = ViewCount + 1, UpdatedAt = GETUTCDATE() 
                    WHERE Id = @Id";
                
                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error incrementing view count for thesis: {Id}", id);
                throw;
            }
        }

        public async Task<bool> IncrementDownloadCountAsync(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    UPDATE THESIS_REPOSITORY 
                    SET DownloadCount = DownloadCount + 1, UpdatedAt = GETUTCDATE() 
                    WHERE Id = @Id";
                
                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error incrementing download count for thesis: {Id}", id);
                throw;
            }
        }
    }
}

