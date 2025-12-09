using VedicAPI.API.Models;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Repositories.Interfaces;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Services
{
    /// <summary>
    /// Service for Research Paper business logic
    /// </summary>
    public class ResearchPaperService : IResearchPaperService
    {
        private readonly IResearchPaperRepository _repository;
        private readonly ILogger<ResearchPaperService> _logger;

        public ResearchPaperService(
            IResearchPaperRepository repository,
            ILogger<ResearchPaperService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ResearchPaperDetailDto?> GetResearchPaperByIdAsync(int id)
        {
            var paper = await _repository.GetByIdAsync(id);
            return paper == null ? null : MapToDetailDto(paper);
        }

        public async Task<IEnumerable<ResearchPaperResponseDto>> GetAllResearchPapersAsync()
        {
            var papers = await _repository.GetAllAsync();
            return papers.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<ResearchPaperResponseDto>> GetFeaturedResearchPapersAsync(int count = 3)
        {
            var papers = await _repository.GetFeaturedAsync(count);
            return papers.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<ResearchPaperResponseDto>> GetResearchPapersByCategoryAsync(string category)
        {
            var papers = await _repository.GetByCategoryAsync(category);
            return papers.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<ResearchPaperResponseDto>> GetResearchPapersByYearAsync(int year)
        {
            var papers = await _repository.GetByYearAsync(year);
            return papers.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<ResearchPaperResponseDto>> GetResearchPapersByInstitutionAsync(string institution)
        {
            var papers = await _repository.GetByInstitutionAsync(institution);
            return papers.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<ResearchPaperResponseDto>> SearchResearchPapersAsync(string searchTerm)
        {
            var papers = await _repository.SearchAsync(searchTerm);
            return papers.Select(MapToResponseDto);
        }

        public async Task<ResearchPaperResponseDto> CreateResearchPaperAsync(ResearchPaperCreateDto paperDto)
        {
            var paper = new ResearchPaper
            {
                Title = paperDto.Title,
                Authors = paperDto.Authors,
                Institution = paperDto.Institution,
                Year = paperDto.Year,
                Category = paperDto.Category,
                Abstract = paperDto.Abstract,
                ContentHtml = paperDto.ContentHtml,
                Keywords = paperDto.Keywords,
                Pages = paperDto.Pages,
                DOI = paperDto.DOI,
                JournalName = paperDto.JournalName,
                Volume = paperDto.Volume,
                IssueNumber = paperDto.IssueNumber,
                PublicationDate = paperDto.PublicationDate,
                PdfUrl = paperDto.PdfUrl,
                CoverImageUrl = paperDto.CoverImageUrl,
                Rating = paperDto.Rating,
                Status = paperDto.Status,
                IsFeatured = paperDto.IsFeatured,
                DownloadCount = 0,
                ViewCount = 0,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdPaper = await _repository.CreateAsync(paper);
            return MapToResponseDto(createdPaper);
        }

        public async Task<ResearchPaperResponseDto?> UpdateResearchPaperAsync(int id, ResearchPaperUpdateDto paperDto)
        {
            var existingPaper = await _repository.GetByIdAsync(id);
            if (existingPaper == null)
                return null;

            // Update only provided fields
            if (paperDto.Title != null) existingPaper.Title = paperDto.Title;
            if (paperDto.Authors != null) existingPaper.Authors = paperDto.Authors;
            if (paperDto.Institution != null) existingPaper.Institution = paperDto.Institution;
            if (paperDto.Year.HasValue) existingPaper.Year = paperDto.Year.Value;
            if (paperDto.Category != null) existingPaper.Category = paperDto.Category;
            if (paperDto.Abstract != null) existingPaper.Abstract = paperDto.Abstract;
            if (paperDto.ContentHtml != null) existingPaper.ContentHtml = paperDto.ContentHtml;
            if (paperDto.Keywords != null) existingPaper.Keywords = paperDto.Keywords;
            if (paperDto.Pages.HasValue) existingPaper.Pages = paperDto.Pages.Value;
            if (paperDto.DOI != null) existingPaper.DOI = paperDto.DOI;
            if (paperDto.JournalName != null) existingPaper.JournalName = paperDto.JournalName;
            if (paperDto.Volume != null) existingPaper.Volume = paperDto.Volume;
            if (paperDto.IssueNumber != null) existingPaper.IssueNumber = paperDto.IssueNumber;
            if (paperDto.PublicationDate.HasValue) existingPaper.PublicationDate = paperDto.PublicationDate;
            if (paperDto.PdfUrl != null) existingPaper.PdfUrl = paperDto.PdfUrl;
            if (paperDto.CoverImageUrl != null) existingPaper.CoverImageUrl = paperDto.CoverImageUrl;
            if (paperDto.Rating.HasValue) existingPaper.Rating = paperDto.Rating.Value;
            if (paperDto.Status != null) existingPaper.Status = paperDto.Status;
            if (paperDto.IsFeatured.HasValue) existingPaper.IsFeatured = paperDto.IsFeatured.Value;
            if (paperDto.IsActive.HasValue) existingPaper.IsActive = paperDto.IsActive.Value;

            existingPaper.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(existingPaper);
            return updated ? MapToResponseDto(existingPaper) : null;
        }

        public async Task<bool> DeleteResearchPaperAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> IncrementDownloadCountAsync(int id)
        {
            return await _repository.IncrementDownloadCountAsync(id);
        }

        // Helper methods for mapping
        private ResearchPaperResponseDto MapToResponseDto(ResearchPaper paper)
        {
            return new ResearchPaperResponseDto
            {
                Id = paper.Id,
                Title = paper.Title,
                Authors = ParseCommaSeparated(paper.Authors),
                Institution = paper.Institution,
                Year = paper.Year,
                Category = paper.Category,
                Abstract = paper.Abstract,
                Keywords = ParseCommaSeparated(paper.Keywords),
                Pages = paper.Pages,
                DownloadCount = paper.DownloadCount,
                ViewCount = paper.ViewCount,
                Rating = paper.Rating,
                Status = paper.Status,
                DOI = paper.DOI,
                JournalName = paper.JournalName,
                Volume = paper.Volume,
                IssueNumber = paper.IssueNumber,
                PublicationDate = paper.PublicationDate,
                PdfUrl = paper.PdfUrl,
                CoverImageUrl = paper.CoverImageUrl,
                IsFeatured = paper.IsFeatured,
                CreatedAt = paper.CreatedAt
            };
        }

        private ResearchPaperDetailDto MapToDetailDto(ResearchPaper paper)
        {
            return new ResearchPaperDetailDto
            {
                Id = paper.Id,
                Title = paper.Title,
                Authors = ParseCommaSeparated(paper.Authors),
                Institution = paper.Institution,
                Year = paper.Year,
                Category = paper.Category,
                Abstract = paper.Abstract,
                ContentHtml = paper.ContentHtml,
                Keywords = ParseCommaSeparated(paper.Keywords),
                Pages = paper.Pages,
                DownloadCount = paper.DownloadCount,
                ViewCount = paper.ViewCount,
                Rating = paper.Rating,
                Status = paper.Status,
                DOI = paper.DOI,
                JournalName = paper.JournalName,
                Volume = paper.Volume,
                IssueNumber = paper.IssueNumber,
                PublicationDate = paper.PublicationDate,
                PdfUrl = paper.PdfUrl,
                CoverImageUrl = paper.CoverImageUrl,
                IsFeatured = paper.IsFeatured,
                CreatedAt = paper.CreatedAt,
                UpdatedAt = paper.UpdatedAt
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

