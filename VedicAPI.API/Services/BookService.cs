using VedicAPI.API.Models;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Repositories.Interfaces;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Services
{
    /// <summary>
    /// Service implementation for Book business logic
    /// </summary>
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepository bookRepository, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<BookResponseDto?> GetBookByIdAsync(int id)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(id);
                return book != null ? MapToResponseDto(book) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetBookByIdAsync for ID {BookId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<BookResponseDto>> GetAllBooksAsync()
        {
            try
            {
                var books = await _bookRepository.GetAllAsync();
                return books.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllBooksAsync");
                throw;
            }
        }

        public async Task<IEnumerable<BookResponseDto>> GetActiveBooksAsync()
        {
            try
            {
                var books = await _bookRepository.GetActiveAsync();
                return books.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetActiveBooksAsync");
                throw;
            }
        }

        public async Task<IEnumerable<BookResponseDto>> GetBooksByCategoryAsync(string category)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(category))
                {
                    throw new ArgumentException("Category cannot be empty", nameof(category));
                }

                var books = await _bookRepository.GetByCategoryAsync(category);
                return books.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetBooksByCategoryAsync for category {Category}", category);
                throw;
            }
        }

        public async Task<IEnumerable<BookResponseDto>> SearchBooksAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return await GetActiveBooksAsync();
                }

                var books = await _bookRepository.SearchAsync(searchTerm);
                return books.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SearchBooksAsync with term {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<BookResponseDto> CreateBookAsync(BookCreateDto bookDto)
        {
            try
            {
                var book = MapToEntity(bookDto);
                var bookId = await _bookRepository.CreateAsync(book);
                book.Id = bookId;

                _logger.LogInformation("Book created successfully with ID {BookId}", bookId);
                return MapToResponseDto(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateBookAsync for book {BookTitle}", bookDto.Title);
                throw;
            }
        }

        public async Task<BookResponseDto?> UpdateBookAsync(int id, BookUpdateDto bookDto)
        {
            try
            {
                var existingBook = await _bookRepository.GetByIdAsync(id);
                if (existingBook == null)
                {
                    _logger.LogWarning("Book with ID {BookId} not found for update", id);
                    return null;
                }

                var updatedBook = MapToEntity(bookDto, id);
                updatedBook.CreatedAt = existingBook.CreatedAt;

                var success = await _bookRepository.UpdateAsync(updatedBook);
                if (success)
                {
                    _logger.LogInformation("Book with ID {BookId} updated successfully", id);
                    return MapToResponseDto(updatedBook);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateBookAsync for book ID {BookId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            try
            {
                var exists = await _bookRepository.ExistsAsync(id);
                if (!exists)
                {
                    _logger.LogWarning("Book with ID {BookId} not found for deletion", id);
                    return false;
                }

                var success = await _bookRepository.DeleteAsync(id);
                if (success)
                {
                    _logger.LogInformation("Book with ID {BookId} deleted successfully", id);
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteBookAsync for book ID {BookId}", id);
                throw;
            }
        }

        #region Mapping Methods

        private static BookResponseDto MapToResponseDto(Book book)
        {
            return new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                CoverImageUrl = book.CoverImageUrl,
                TotalChapters = book.TotalChapters,
                Category = book.Category,
                Language = book.Language,
                PublicationYear = book.PublicationYear,
                ISBN = book.ISBN,
                CreatedAt = book.CreatedAt,
                UpdatedAt = book.UpdatedAt,
                IsActive = book.IsActive
            };
        }

        private static Book MapToEntity(BookCreateDto dto)
        {
            return new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Description = dto.Description,
                CoverImageUrl = dto.CoverImageUrl,
                TotalChapters = 0,
                Category = dto.Category,
                Language = dto.Language,
                PublicationYear = dto.PublicationYear,
                ISBN = dto.ISBN,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }

        private static Book MapToEntity(BookUpdateDto dto, int id)
        {
            return new Book
            {
                Id = id,
                Title = dto.Title,
                Author = dto.Author,
                Description = dto.Description,
                CoverImageUrl = dto.CoverImageUrl,
                TotalChapters = dto.TotalChapters,
                Category = dto.Category,
                Language = dto.Language,
                PublicationYear = dto.PublicationYear,
                ISBN = dto.ISBN,
                IsActive = dto.IsActive,
                UpdatedAt = DateTime.UtcNow
            };
        }

        #endregion
    }
}

