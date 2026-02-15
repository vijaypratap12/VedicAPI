using VedicAPI.API.Models;

namespace VedicAPI.API.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for HerbalMedicine entity
    /// </summary>
    public interface IHerbalMedicineRepository
    {
        Task<HerbalMedicine?> GetByIdAsync(long id);
        Task<IEnumerable<HerbalMedicine>> GetAllAsync();
        Task<IEnumerable<HerbalMedicine>> SearchAsync(string searchTerm);
        Task<IEnumerable<HerbalMedicine>> GetByPrakritiEffectAsync(string prakriti);
    }
}
