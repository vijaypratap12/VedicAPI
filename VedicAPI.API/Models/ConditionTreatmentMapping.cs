namespace VedicAPI.API.Models
{
    /// <summary>
    /// Represents a condition-treatment mapping entity
    /// </summary>
    public class ConditionTreatmentMapping
    {
        public long Id { get; set; }
        public long ConditionId { get; set; }
        public string TreatmentType { get; set; } = string.Empty;
        public long TreatmentItemId { get; set; }
        public string? Prakriti { get; set; }
        public int Priority { get; set; }
        public decimal? SuccessRate { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
