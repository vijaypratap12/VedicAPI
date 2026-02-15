using VedicAPI.API.Models.DTOs;

namespace VedicAPI.API.Services.Interfaces
{
    /// <summary>
    /// Service interface for Treatment recommendation business logic
    /// </summary>
    public interface ITreatmentRecommendationService
    {
        Task<TreatmentRecommendationDto> GenerateRecommendationsAsync(long patientId, long conditionId);
        Task<IEnumerable<ConditionDto>> SearchConditionsAsync(string searchTerm, string? category = null);
        Task<IEnumerable<HerbalMedicineDto>> GetHerbalMedicinesAsync(string? searchTerm = null, string? prakritiEffect = null);
        Task<IEnumerable<YogaAsanaDto>> GetYogaAsanasAsync(string? category = null, string? difficulty = null, string? prakritiEffect = null);
        Task<IEnumerable<DietaryItemDto>> GetDietaryItemsAsync(string prakriti, string? category = null);
    }
}
