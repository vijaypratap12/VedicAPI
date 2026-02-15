using VedicAPI.API.Models.DTOs;

namespace VedicAPI.API.Services.Interfaces
{
    /// <summary>
    /// Service interface for Treatment plan business logic
    /// </summary>
    public interface ITreatmentPlanService
    {
        Task<TreatmentPlanResponseDto?> GetTreatmentPlanByIdAsync(long id);
        Task<IEnumerable<TreatmentPlanResponseDto>> GetTreatmentPlansByPatientIdAsync(long patientId);
        Task<TreatmentPlanResponseDto> CreateTreatmentPlanAsync(TreatmentPlanCreateDto planDto);
        Task<TreatmentPlanResponseDto?> UpdateTreatmentPlanAsync(long id, TreatmentPlanCreateDto planDto);
        Task<long> SaveTreatmentOutcomeAsync(TreatmentOutcomeDto outcomeDto);
    }
}
