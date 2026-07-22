namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// Request DTO for AI-driven treatment recommendation generation
    /// </summary>
    public class AiRecommendationRequestDto
    {
        public long PatientId { get; set; }
        public long ConditionId { get; set; }
        public bool UseAi { get; set; } = true;
        public string? CustomClinicalNotes { get; set; }
        public string? CustomConditionName { get; set; }
    }

    /// <summary>
    /// Response DTO exposing global AI feature flag status to the frontend
    /// </summary>
    public class AiConfigDto
    {
        public bool Enabled { get; set; }
        public string Provider { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public bool EnableFallback { get; set; }
        public Dictionary<string, bool> Features { get; set; } = new();
    }
}
