using VedicAPI.API.Models.DTOs;

namespace VedicAPI.API.Services.Interfaces
{
    /// <summary>
    /// Service interface for Book business logic
    /// </summary>
    public interface IBookService
    {
        Task<BookResponseDto?> GetBookByIdAsync(int id);
        Task<IEnumerable<BookResponseDto>> GetAllBooksAsync();
        Task<IEnumerable<BookResponseDto>> GetActiveBooksAsync();
        Task<IEnumerable<BookResponseDto>> GetBooksByCategoryAsync(string category);
        Task<IEnumerable<BookResponseDto>> SearchBooksAsync(string searchTerm);
        Task<BookResponseDto> CreateBookAsync(BookCreateDto bookDto);
        Task<BookResponseDto?> UpdateBookAsync(int id, BookUpdateDto bookDto);
        Task<bool> DeleteBookAsync(int id);
    }
}

