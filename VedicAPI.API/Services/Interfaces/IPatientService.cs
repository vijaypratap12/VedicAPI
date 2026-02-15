using VedicAPI.API.Models.DTOs;

namespace VedicAPI.API.Services.Interfaces
{
    /// <summary>
    /// Service interface for Patient business logic
    /// </summary>
    public interface IPatientService
    {
        Task<PatientResponseDto?> GetPatientByIdAsync(long id);
        Task<PatientResponseDto?> GetPatientByUserIdAsync(int userId);
        Task<IEnumerable<PatientResponseDto>> GetAllPatientsAsync();
        Task<PatientResponseDto> CreatePatientAsync(PatientCreateDto patientDto);
        Task<PatientResponseDto?> UpdatePatientAsync(long id, PatientUpdateDto patientDto);
        Task<bool> DeletePatientAsync(long id);
    }
}
