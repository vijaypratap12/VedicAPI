namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for Prakriti assessment response
    /// </summary>
    public class PrakritiAssessmentResponseDto
    {
        public long Id { get; set; }
        public long PatientId { get; set; }
        public int VataScore { get; set; }
        public int PittaScore { get; set; }
        public int KaphaScore { get; set; }
        public string DominantPrakriti { get; set; } = string.Empty;
        public DateTime AssessedAt { get; set; }
    }
}
