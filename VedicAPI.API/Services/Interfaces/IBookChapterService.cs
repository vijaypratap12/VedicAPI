using VedicAPI.API.Models.DTOs;

namespace VedicAPI.API.Services.Interfaces
{
    /// <summary>
    /// Service interface for BookChapter business logic
    /// </summary>
    public interface IBookChapterService
    {
        Task<ChapterDetailDto?> GetChapterByIdAsync(int id);
        Task<ChapterDetailDto?> GetChapterByNumberAsync(int bookId, int chapterNumber);
        Task<IEnumerable<ChapterSummaryDto>> GetChaptersByBookIdAsync(int bookId);
        Task<BookWithChaptersDto?> GetBookWithChaptersAsync(int bookId);
        Task<ChapterDetailDto?> GetNextChapterAsync(int bookId, int currentChapterNumber);
        Task<ChapterDetailDto?> GetPreviousChapterAsync(int bookId, int currentChapterNumber);
        Task<ChapterDetailDto> CreateChapterAsync(int bookId, ChapterCreateDto chapterDto);
        Task<ChapterDetailDto?> UpdateChapterAsync(int chapterId, ChapterUpdateDto chapterDto);
        Task<bool> DeleteChapterAsync(int chapterId);
    }
}

