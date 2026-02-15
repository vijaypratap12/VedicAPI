namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for herbal medicine
    /// </summary>
    public class HerbalMedicineDto
    {
        public long Id { get; set; }
        public string CommonName { get; set; } = string.Empty;
        public string? SanskritName { get; set; }
        public string? ScientificName { get; set; }
        public string? HindiName { get; set; }
        public string? Properties { get; set; }
        public string? Indications { get; set; }
        public string? Dosage { get; set; }
        public string? Contraindications { get; set; }
        public string? SideEffects { get; set; }
        public string? ImageUrl { get; set; }
        public string? VataEffect { get; set; }
        public string? PittaEffect { get; set; }
        public string? KaphaEffect { get; set; }
    }
}
