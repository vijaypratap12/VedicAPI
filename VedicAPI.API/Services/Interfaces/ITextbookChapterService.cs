using VedicAPI.API.Models.DTOs;

namespace VedicAPI.API.Services.Interfaces
{
    /// <summary>
    /// Interface for TextbookChapter business logic operations
    /// </summary>
    public interface ITextbookChapterService
    {
        Task<TextbookChapterDetailDto?> GetChapterByIdAsync(int id);
        Task<IEnumerable<TextbookChapterSummaryDto>> GetChaptersByTextbookIdAsync(int textbookId);
        Task<TextbookChapterDetailDto> CreateChapterAsync(int textbookId, TextbookChapterCreateDto chapterDto);
        Task<TextbookChapterDetailDto?> UpdateChapterAsync(int id, TextbookChapterUpdateDto chapterDto);
        Task<bool> DeleteChapterAsync(int id);
    }
}

