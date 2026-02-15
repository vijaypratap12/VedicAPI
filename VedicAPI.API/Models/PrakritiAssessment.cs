namespace VedicAPI.API.Models
{
    /// <summary>
    /// Represents a patient's Prakriti assessment response
    /// </summary>
    public class PrakritiAssessment
    {
        public long Id { get; set; }
        public long PatientId { get; set; }
        public string Responses { get; set; } = string.Empty;
        public int VataScore { get; set; }
        public int PittaScore { get; set; }
        public int KaphaScore { get; set; }
        public string DominantPrakriti { get; set; } = string.Empty;
        public DateTime AssessedAt { get; set; }
    }
}
