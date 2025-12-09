using VedicAPI.API.Models;

namespace VedicAPI.API.Repositories.Interfaces
{
    /// <summary>
    /// Interface for Research Paper data access operations
    /// </summary>
    public interface IResearchPaperRepository
    {
        Task<ResearchPaper?> GetByIdAsync(int id);
        Task<IEnumerable<ResearchPaper>> GetAllAsync();
        Task<IEnumerable<ResearchPaper>> GetFeaturedAsync(int count = 3);
        Task<IEnumerable<ResearchPaper>> GetByCategoryAsync(string category);
        Task<IEnumerable<ResearchPaper>> GetByYearAsync(int year);
        Task<IEnumerable<ResearchPaper>> GetByInstitutionAsync(string institution);
        Task<IEnumerable<ResearchPaper>> SearchAsync(string searchTerm);
        Task<ResearchPaper> CreateAsync(ResearchPaper paper);
        Task<bool> UpdateAsync(ResearchPaper paper);
        Task<bool> DeleteAsync(int id);
        Task<bool> IncrementViewCountAsync(int id);
        Task<bool> IncrementDownloadCountAsync(int id);
    }
}

