using VedicAPI.API.Models;

namespace VedicAPI.API.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for BookChapter entity operations
    /// </summary>
    public interface IBookChapterRepository
    {
        Task<BookChapter?> GetChapterByIdAsync(int id);
        Task<BookChapter?> GetChapterByNumberAsync(int bookId, int chapterNumber);
        Task<IEnumerable<BookChapter>> GetChaptersByBookIdAsync(int bookId);
        Task<BookChapter?> GetNextChapterAsync(int bookId, int currentChapterNumber);
        Task<BookChapter?> GetPreviousChapterAsync(int bookId, int currentChapterNumber);
        Task<int> CreateChapterAsync(BookChapter chapter);
        Task<bool> UpdateChapterAsync(BookChapter chapter);
        Task<bool> DeleteChapterAsync(int id);
        Task<int> GetChapterCountByBookIdAsync(int bookId);
        Task<bool> ExistsAsync(int id);
    }
}

