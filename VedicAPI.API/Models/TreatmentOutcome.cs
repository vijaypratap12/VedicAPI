namespace VedicAPI.API.Models
{
    /// <summary>
    /// Represents a treatment outcome entity
    /// </summary>
    public class TreatmentOutcome
    {
        public long Id { get; set; }
        public long TreatmentPlanId { get; set; }
        public long PatientId { get; set; }
        public int? EffectivenessScore { get; set; }
        public string? SideEffects { get; set; }
        public string? PatientFeedback { get; set; }
        public string? DoctorNotes { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public DateTime RecordedAt { get; set; }
        public int? RecordedBy { get; set; }
    }
}
