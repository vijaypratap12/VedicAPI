using VedicAPI.API.Models.DTOs;

namespace VedicAPI.API.Services.Interfaces
{
    /// <summary>
    /// Interface for Research Paper business logic operations
    /// </summary>
    public interface IResearchPaperService
    {
        Task<ResearchPaperDetailDto?> GetResearchPaperByIdAsync(int id);
        Task<IEnumerable<ResearchPaperResponseDto>> GetAllResearchPapersAsync();
        Task<IEnumerable<ResearchPaperResponseDto>> GetFeaturedResearchPapersAsync(int count = 3);
        Task<IEnumerable<ResearchPaperResponseDto>> GetResearchPapersByCategoryAsync(string category);
        Task<IEnumerable<ResearchPaperResponseDto>> GetResearchPapersByYearAsync(int year);
        Task<IEnumerable<ResearchPaperResponseDto>> GetResearchPapersByInstitutionAsync(string institution);
        Task<IEnumerable<ResearchPaperResponseDto>> SearchResearchPapersAsync(string searchTerm);
        Task<ResearchPaperResponseDto> CreateResearchPaperAsync(ResearchPaperCreateDto paperDto);
        Task<ResearchPaperResponseDto?> UpdateResearchPaperAsync(int id, ResearchPaperUpdateDto paperDto);
        Task<bool> DeleteResearchPaperAsync(int id);
        Task<bool> IncrementDownloadCountAsync(int id);
    }
}

