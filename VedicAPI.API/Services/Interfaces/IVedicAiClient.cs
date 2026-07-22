using VedicAPI.API.Models;
using VedicAPI.API.Models.DTOs;

namespace VedicAPI.API.Services.Interfaces
{
    /// <summary>
    /// Generic interface for feature-flagged AI providers (Gemini, Groq, Ollama, etc.)
    /// </summary>
    public interface IVedicAiClient
    {
        string ProviderName { get; }
        
        /// <summary>
        /// Generates a structured clinical JSON response using the specified provider API
        /// </summary>
        Task<TreatmentRecommendationDto?> GenerateClinicalRecommendationAsync(
            Patient patient,
            Condition condition,
            IEnumerable<HerbalMedicine> candidateMedicines,
            IEnumerable<YogaAsana> candidateYoga,
            IEnumerable<DietaryItem> candidateDietary,
            string apiKey,
            string modelName,
            string? customClinicalNotes = null);

        Task<TreatmentRecommendationDto?> SuggestAdjustmentAsync(
            Patient patient,
            Condition condition,
            TreatmentPlanResponseDto currentPlan,
            IEnumerable<TreatmentOutcomeDto> outcomes,
            string apiKey,
            string modelName);
    }
}
