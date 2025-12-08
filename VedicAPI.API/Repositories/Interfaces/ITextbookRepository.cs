using VedicAPI.API.Models;

namespace VedicAPI.API.Repositories.Interfaces
{
    /// <summary>
    /// Interface for Textbook repository operations
    /// </summary>
    public interface ITextbookRepository
    {
        Task<Textbook?> GetByIdAsync(int id);
        Task<IEnumerable<Textbook>> GetAllAsync();
        Task<IEnumerable<Textbook>> GetActiveAsync();
        Task<IEnumerable<Textbook>> GetByCategoryAsync(string category);
        Task<IEnumerable<Textbook>> GetByLevelAsync(string level);
        Task<IEnumerable<Textbook>> SearchAsync(string searchTerm);
        Task<int> CreateAsync(Textbook textbook);
        Task<bool> UpdateAsync(Textbook textbook);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> IncrementDownloadCountAsync(int id);
        Task<bool> IncrementViewCountAsync(int id);
    }
}

