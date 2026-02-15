using VedicAPI.API.Models;

namespace VedicAPI.API.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for Treatment-related operations
    /// </summary>
    public interface ITreatmentRepository
    {
        // Treatment Plans
        Task<TreatmentPlan?> GetTreatmentPlanByIdAsync(long id);
        Task<IEnumerable<TreatmentPlan>> GetTreatmentPlansByPatientIdAsync(long patientId);
        Task<long> CreateTreatmentPlanAsync(TreatmentPlan plan);
        Task<bool> UpdateTreatmentPlanAsync(TreatmentPlan plan);

        // Treatment Outcomes
        Task<long> SaveTreatmentOutcomeAsync(TreatmentOutcome outcome);
        Task<IEnumerable<TreatmentOutcome>> GetOutcomesByTreatmentPlanIdAsync(long treatmentPlanId);

        // Treatment Recommendations
        Task<IEnumerable<HerbalMedicine>> GetRecommendedMedicinesAsync(long conditionId, string prakriti);
        Task<IEnumerable<YogaAsana>> GetRecommendedYogaAsanasAsync(long conditionId, string prakriti);
        Task<IEnumerable<DietaryItem>> GetDietaryItemsByPrakritiAsync(string prakriti, string? category = null);

        // Statistics
        Task<Dictionary<string, object>> GetStatisticsAsync();
    }
}
