namespace VedicAPI.API.Models
{
    /// <summary>
    /// Represents a book entity in the database
    /// </summary>
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public int TotalChapters { get; set; }
        public string? Category { get; set; }
        public string? Language { get; set; }
        public int? PublicationYear { get; set; }
        public string? ISBN { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation property
        public ICollection<BookChapter>? Chapters { get; init; }
    }
}

