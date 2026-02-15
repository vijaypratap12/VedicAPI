namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for treatment statistics
    /// </summary>
    public class TreatmentStatisticsDto
    {
        public int TotalPatients { get; set; }
        public int TotalTreatmentPlans { get; set; }
        public int TotalConditions { get; set; }
        public int TotalHerbalMedicines { get; set; }
        public int TotalYogaAsanas { get; set; }
        public decimal AverageConfidenceScore { get; set; }
        public decimal AverageEffectivenessScore { get; set; }
        public Dictionary<string, int> PrakritiDistribution { get; set; } = new();
        public List<ConditionStatDto> TopConditions { get; set; } = new();
        public List<MedicineStatDto> TopMedicines { get; set; } = new();
    }

    public class ConditionStatDto
    {
        public string ConditionName { get; set; } = string.Empty;
        public int TreatmentCount { get; set; }
    }

    public class MedicineStatDto
    {
        public string CommonName { get; set; } = string.Empty;
        public string? SanskritName { get; set; }
        public int PrescriptionCount { get; set; }
    }
}
