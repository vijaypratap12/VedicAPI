using VedicAPI.API.Models;

namespace VedicAPI.API.Repositories.Interfaces
{
    /// <summary>
    /// Interface for Thesis data access operations
    /// </summary>
    public interface IThesisRepository
    {
        Task<Thesis?> GetByIdAsync(int id);
        Task<IEnumerable<Thesis>> GetAllAsync();
        Task<IEnumerable<Thesis>> GetFeaturedAsync(int count = 3);
        Task<IEnumerable<Thesis>> GetByCategoryAsync(string category);
        Task<IEnumerable<Thesis>> GetByYearAsync(int year);
        Task<IEnumerable<Thesis>> GetByInstitutionAsync(string institution);
        Task<IEnumerable<Thesis>> SearchAsync(string searchTerm);
        Task<Thesis> CreateAsync(Thesis thesis);
        Task<bool> UpdateAsync(Thesis thesis);
        Task<bool> DeleteAsync(int id);
        Task<bool> IncrementViewCountAsync(int id);
        Task<bool> IncrementDownloadCountAsync(int id);
    }
}

