using VedicAPI.API.Models;

namespace VedicAPI.API.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for Condition entity
    /// </summary>
    public interface IConditionRepository
    {
        Task<Condition?> GetByIdAsync(long id);
        Task<IEnumerable<Condition>> GetAllAsync();
        Task<IEnumerable<Condition>> SearchAsync(string searchTerm, string? category = null);
        Task<IEnumerable<Condition>> GetByCategoryAsync(string category);
    }
}
