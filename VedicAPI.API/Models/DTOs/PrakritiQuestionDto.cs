namespace VedicAPI.API.Models.DTOs
{
    /// <summary>
    /// DTO for Prakriti assessment question
    /// </summary>
    public class PrakritiQuestionDto
    {
        public long Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
        public string VataOption { get; set; } = string.Empty;
        public string PittaOption { get; set; } = string.Empty;
        public string KaphaOption { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}
