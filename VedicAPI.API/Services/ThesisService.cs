using VedicAPI.API.Models;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Repositories.Interfaces;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Services
{
    /// <summary>
    /// Service for Thesis business logic
    /// </summary>
    public class ThesisService : IThesisService
    {
        private readonly IThesisRepository _repository;
        private readonly ILogger<ThesisService> _logger;

        public ThesisService(
            IThesisRepository repository,
            ILogger<ThesisService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ThesisDetailDto?> GetThesisByIdAsync(int id)
        {
            var thesis = await _repository.GetByIdAsync(id);
            return thesis == null ? null : MapToDetailDto(thesis);
        }

        public async Task<IEnumerable<ThesisResponseDto>> GetAllThesisAsync()
        {
            var thesisList = await _repository.GetAllAsync();
            return thesisList.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<ThesisResponseDto>> GetFeaturedThesisAsync(int count = 3)
        {
            var thesisList = await _repository.GetFeaturedAsync(count);
            return thesisList.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<ThesisResponseDto>> GetThesisByCategoryAsync(string category)
        {
            var thesisList = await _repository.GetByCategoryAsync(category);
            return thesisList.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<ThesisResponseDto>> GetThesisByYearAsync(int year)
        {
            var thesisList = await _repository.GetByYearAsync(year);
            return thesisList.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<ThesisResponseDto>> GetThesisByInstitutionAsync(string institution)
        {
            var thesisList = await _repository.GetByInstitutionAsync(institution);
            return thesisList.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<ThesisResponseDto>> SearchThesisAsync(string searchTerm)
        {
            var thesisList = await _repository.SearchAsync(searchTerm);
            return thesisList.Select(MapToResponseDto);
        }

        public async Task<ThesisResponseDto> CreateThesisAsync(ThesisCreateDto thesisDto)
        {
            var thesis = new Thesis
            {
                Title = thesisDto.Title,
                Author = thesisDto.Author,
                GuideNames = thesisDto.GuideNames,
                Institution = thesisDto.Institution,
                Department = thesisDto.Department,
                Year = thesisDto.Year,
                Category = thesisDto.Category,
                ThesisType = thesisDto.ThesisType,
                Abstract = thesisDto.Abstract,
                ContentHtml = thesisDto.ContentHtml,
                Keywords = thesisDto.Keywords,
                Pages = thesisDto.Pages,
                SubmissionDate = thesisDto.SubmissionDate,
                ApprovalDate = thesisDto.ApprovalDate,
                DefenseDate = thesisDto.DefenseDate,
                Grade = thesisDto.Grade,
                PdfUrl = thesisDto.PdfUrl,
                CoverImageUrl = thesisDto.CoverImageUrl,
                UniversityRegistrationNumber = thesisDto.UniversityRegistrationNumber,
                Rating = thesisDto.Rating,
                Status = thesisDto.Status,
                IsFeatured = thesisDto.IsFeatured,
                DownloadCount = 0,
                ViewCount = 0,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdThesis = await _repository.CreateAsync(thesis);
            return MapToResponseDto(createdThesis);
        }

        public async Task<ThesisResponseDto?> UpdateThesisAsync(int id, ThesisUpdateDto thesisDto)
        {
            var existingThesis = await _repository.GetByIdAsync(id);
            if (existingThesis == null)
                return null;

            // Update only provided fields
            if (thesisDto.Title != null) existingThesis.Title = thesisDto.Title;
            if (thesisDto.Author != null) existingThesis.Author = thesisDto.Author;
            if (thesisDto.GuideNames != null) existingThesis.GuideNames = thesisDto.GuideNames;
            if (thesisDto.Institution != null) existingThesis.Institution = thesisDto.Institution;
            if (thesisDto.Department != null) existingThesis.Department = thesisDto.Department;
            if (thesisDto.Year.HasValue) existingThesis.Year = thesisDto.Year.Value;
            if (thesisDto.Category != null) existingThesis.Category = thesisDto.Category;
            if (thesisDto.ThesisType != null) existingThesis.ThesisType = thesisDto.ThesisType;
            if (thesisDto.Abstract != null) existingThesis.Abstract = thesisDto.Abstract;
            if (thesisDto.ContentHtml != null) existingThesis.ContentHtml = thesisDto.ContentHtml;
            if (thesisDto.Keywords != null) existingThesis.Keywords = thesisDto.Keywords;
            if (thesisDto.Pages.HasValue) existingThesis.Pages = thesisDto.Pages.Value;
            if (thesisDto.SubmissionDate.HasValue) existingThesis.SubmissionDate = thesisDto.SubmissionDate;
            if (thesisDto.ApprovalDate.HasValue) existingThesis.ApprovalDate = thesisDto.ApprovalDate;
            if (thesisDto.DefenseDate.HasValue) existingThesis.DefenseDate = thesisDto.DefenseDate;
            if (thesisDto.Grade != null) existingThesis.Grade = thesisDto.Grade;
            if (thesisDto.PdfUrl != null) existingThesis.PdfUrl = thesisDto.PdfUrl;
            if (thesisDto.CoverImageUrl != null) existingThesis.CoverImageUrl = thesisDto.CoverImageUrl;
            if (thesisDto.UniversityRegistrationNumber != null) existingThesis.UniversityRegistrationNumber = thesisDto.UniversityRegistrationNumber;
            if (thesisDto.Rating.HasValue) existingThesis.Rating = thesisDto.Rating.Value;
            if (thesisDto.Status != null) existingThesis.Status = thesisDto.Status;
            if (thesisDto.IsFeatured.HasValue) existingThesis.IsFeatured = thesisDto.IsFeatured.Value;
            if (thesisDto.IsActive.HasValue) existingThesis.IsActive = thesisDto.IsActive.Value;

            existingThesis.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(existingThesis);
            return updated ? MapToResponseDto(existingThesis) : null;
        }

        public async Task<bool> DeleteThesisAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> IncrementDownloadCountAsync(int id)
        {
            return await _repository.IncrementDownloadCountAsync(id);
        }

        // Helper methods for mapping
        private ThesisResponseDto MapToResponseDto(Thesis thesis)
        {
            return new ThesisResponseDto
            {
                Id = thesis.Id,
                Title = thesis.Title,
                Author = thesis.Author,
                GuideNames = ParseCommaSeparated(thesis.GuideNames),
                Institution = thesis.Institution,
                Department = thesis.Department,
                Year = thesis.Year,
                Category = thesis.Category,
                ThesisType = thesis.ThesisType,
                Abstract = thesis.Abstract,
                Keywords = ParseCommaSeparated(thesis.Keywords),
                Pages = thesis.Pages,
                DownloadCount = thesis.DownloadCount,
                ViewCount = thesis.ViewCount,
                Rating = thesis.Rating,
                Status = thesis.Status,
                SubmissionDate = thesis.SubmissionDate,
                ApprovalDate = thesis.ApprovalDate,
                DefenseDate = thesis.DefenseDate,
                Grade = thesis.Grade,
                PdfUrl = thesis.PdfUrl,
                CoverImageUrl = thesis.CoverImageUrl,
                IsFeatured = thesis.IsFeatured,
                CreatedAt = thesis.CreatedAt
            };
        }

        private ThesisDetailDto MapToDetailDto(Thesis thesis)
        {
            return new ThesisDetailDto
            {
                Id = thesis.Id,
                Title = thesis.Title,
                Author = thesis.Author,
                GuideNames = ParseCommaSeparated(thesis.GuideNames),
                Institution = thesis.Institution,
                Department = thesis.Department,
                Year = thesis.Year,
                Category = thesis.Category,
                ThesisType = thesis.ThesisType,
                Abstract = thesis.Abstract,
                ContentHtml = thesis.ContentHtml,
                Keywords = ParseCommaSeparated(thesis.Keywords),
                Pages = thesis.Pages,
                DownloadCount = thesis.DownloadCount,
                ViewCount = thesis.ViewCount,
                Rating = thesis.Rating,
                Status = thesis.Status,
                SubmissionDate = thesis.SubmissionDate,
                ApprovalDate = thesis.ApprovalDate,
                DefenseDate = thesis.DefenseDate,
                Grade = thesis.Grade,
                PdfUrl = thesis.PdfUrl,
                CoverImageUrl = thesis.CoverImageUrl,
                UniversityRegistrationNumber = thesis.UniversityRegistrationNumber,
                IsFeatured = thesis.IsFeatured,
                CreatedAt = thesis.CreatedAt,
                UpdatedAt = thesis.UpdatedAt
            };
        }

        private List<string> ParseCommaSeparated(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new List<string>();

            return value.Split(',', StringSplitOptions.RemoveEmptyEntries)
                       .Select(s => s.Trim())
                       .ToList();
        }
    }
}

