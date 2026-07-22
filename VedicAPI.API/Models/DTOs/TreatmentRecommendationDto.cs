namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for complete treatment recommendation (supporting both DB rules and AI Engine)
    /// </summary>
    public class TreatmentRecommendationDto
    {
        public long PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string Prakriti { get; set; } = string.Empty;
        public long ConditionId { get; set; }
        public string ConditionName { get; set; } = string.Empty;

        // Classical Treatment Components
        public List<HerbalMedicineDto> RecommendedMedicines { get; set; } = new();
        public List<YogaAsanaDto> RecommendedYogaAsanas { get; set; } = new();
        public List<DietaryItemDto> RecommendedDietaryItems { get; set; } = new();
        public List<string> LifestyleModifications { get; set; } = new();
        public decimal ConfidenceScore { get; set; }
        public string Explanation { get; set; } = string.Empty;

        // AI Feature-Flagged Clinical Extensions
        public bool IsAiGenerated { get; set; }
        public string AiModelUsed { get; set; } = string.Empty;
        public string ClinicalRationale { get; set; } = string.Empty;
        public Dictionary<string, string> AnupanaInstructions { get; set; } = new();
        public List<string> PathyaList { get; set; } = new();
        public List<string> ApathyaList { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
    }
}
