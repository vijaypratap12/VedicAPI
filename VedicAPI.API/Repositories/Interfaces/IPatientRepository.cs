using VedicAPI.API.Models;

namespace VedicAPI.API.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for Patient entity
    /// </summary>
    public interface IPatientRepository
    {
        Task<Patient?> GetByIdAsync(long id);
        Task<Patient?> GetByUserIdAsync(int userId);
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<long> CreateAsync(Patient patient);
        Task<bool> UpdateAsync(Patient patient);
        Task<bool> DeleteAsync(long id);
        Task<bool> ExistsAsync(long id);
    }
}
