using VedicAPI.API.Models;

namespace VedicAPI.API.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for YogaAsana entity
    /// </summary>
    public interface IYogaAsanaRepository
    {
        Task<YogaAsana?> GetByIdAsync(long id);
        Task<IEnumerable<YogaAsana>> GetAllAsync();
        Task<IEnumerable<YogaAsana>> GetByCategoryAsync(string category);
        Task<IEnumerable<YogaAsana>> GetByDifficultyAsync(string difficulty);
        Task<IEnumerable<YogaAsana>> GetByPrakritiEffectAsync(string prakriti);
    }
}
