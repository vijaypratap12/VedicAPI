using VedicAPI.API.Models.DTOs;

namespace VedicAPI.API.Services.Interfaces
{
    /// <summary>
    /// Interface for Textbook business logic operations
    /// </summary>
    public interface ITextbookService
    {
        Task<TextbookResponseDto?> GetTextbookByIdAsync(int id);
        Task<TextbookWithChaptersDto?> GetTextbookWithChaptersAsync(int id);
        Task<IEnumerable<TextbookResponseDto>> GetAllTextbooksAsync();
        Task<IEnumerable<TextbookResponseDto>> GetActiveTextbooksAsync();
        Task<IEnumerable<TextbookResponseDto>> GetTextbooksByCategoryAsync(string category);
        Task<IEnumerable<TextbookResponseDto>> GetTextbooksByLevelAsync(string level);
        Task<IEnumerable<TextbookResponseDto>> SearchTextbooksAsync(string searchTerm);
        Task<TextbookResponseDto> CreateTextbookAsync(TextbookCreateDto textbookDto);
        Task<TextbookResponseDto?> UpdateTextbookAsync(int id, TextbookUpdateDto textbookDto);
        Task<bool> DeleteTextbookAsync(int id);
        Task<bool> IncrementDownloadCountAsync(int id);
        Task<bool> IncrementViewCountAsync(int id);
    }
}

