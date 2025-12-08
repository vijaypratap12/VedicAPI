using VedicAPI.API.Models;

namespace VedicAPI.API.Repositories.Interfaces
{
    /// <summary>
    /// Interface for TextbookChapter repository operations
    /// </summary>
    public interface ITextbookChapterRepository
    {
        Task<TextbookChapter?> GetByIdAsync(int id);
        Task<IEnumerable<TextbookChapter>> GetByTextbookIdAsync(int textbookId);
        Task<int> CreateAsync(TextbookChapter chapter);
        Task<bool> UpdateAsync(TextbookChapter chapter);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ChapterNumberExistsAsync(int textbookId, int chapterNumber, int? excludeChapterId = null);
    }
}

