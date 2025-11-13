using VedicAPI.API.Models;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Repositories.Interfaces;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Services
{
    /// <summary>
    /// Service implementation for BookChapter business logic
    /// </summary>
    public class BookChapterService : IBookChapterService
    {
        private readonly IBookChapterRepository _chapterRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BookChapterService> _logger;

        public BookChapterService(
            IBookChapterRepository chapterRepository,
            IBookRepository bookRepository,
            ILogger<BookChapterService> logger)
        {
            _chapterRepository = chapterRepository;
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<ChapterDetailDto?> GetChapterByIdAsync(int id)
        {
            try
            {
                var chapter = await _chapterRepository.GetChapterByIdAsync(id);
                if (chapter == null) return null;

                var book = await _bookRepository.GetByIdAsync(chapter.BookId);
                if (book == null) return null;

                return await MapToDetailDtoAsync(chapter, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetChapterByIdAsync for ID {ChapterId}", id);
                throw;
            }
        }

        public async Task<ChapterDetailDto?> GetChapterByNumberAsync(int bookId, int chapterNumber)
        {
            try
            {
                var chapter = await _chapterRepository.GetChapterByNumberAsync(bookId, chapterNumber);
                if (chapter == null) return null;

                var book = await _bookRepository.GetByIdAsync(bookId);
                if (book == null) return null;

                return await MapToDetailDtoAsync(chapter, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetChapterByNumberAsync for book {BookId}, chapter {ChapterNumber}", 
                    bookId, chapterNumber);
                throw;
            }
        }

        public async Task<IEnumerable<ChapterSummaryDto>> GetChaptersByBookIdAsync(int bookId)
        {
            try
            {
                var chapters = await _chapterRepository.GetChaptersByBookIdAsync(bookId);
                return chapters.Select(MapToSummaryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetChaptersByBookIdAsync for book {BookId}", bookId);
                throw;
            }
        }

        public async Task<BookWithChaptersDto?> GetBookWithChaptersAsync(int bookId)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(bookId);
                if (book == null) return null;

                var chapters = await _chapterRepository.GetChaptersByBookIdAsync(bookId);

                return new BookWithChaptersDto
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
                    Chapters = chapters.Select(MapToSummaryDto).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetBookWithChaptersAsync for book {BookId}", bookId);
                throw;
            }
        }

        public async Task<ChapterDetailDto?> GetNextChapterAsync(int bookId, int currentChapterNumber)
        {
            try
            {
                var chapter = await _chapterRepository.GetNextChapterAsync(bookId, currentChapterNumber);
                if (chapter == null) return null;

                var book = await _bookRepository.GetByIdAsync(bookId);
                if (book == null) return null;

                return await MapToDetailDtoAsync(chapter, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetNextChapterAsync for book {BookId}, chapter {ChapterNumber}",
                    bookId, currentChapterNumber);
                throw;
            }
        }

        public async Task<ChapterDetailDto?> GetPreviousChapterAsync(int bookId, int currentChapterNumber)
        {
            try
            {
                var chapter = await _chapterRepository.GetPreviousChapterAsync(bookId, currentChapterNumber);
                if (chapter == null) return null;

                var book = await _bookRepository.GetByIdAsync(bookId);
                if (book == null) return null;

                return await MapToDetailDtoAsync(chapter, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetPreviousChapterAsync for book {BookId}, chapter {ChapterNumber}",
                    bookId, currentChapterNumber);
                throw;
            }
        }

        public async Task<ChapterDetailDto> CreateChapterAsync(int bookId, ChapterCreateDto chapterDto)
        {
            try
            {
                // Verify book exists
                var book = await _bookRepository.GetByIdAsync(bookId);
                if (book == null)
                {
                    throw new ArgumentException($"Book with ID {bookId} not found");
                }

                var chapter = new BookChapter
                {
                    BookId = bookId,
                    ChapterNumber = chapterDto.ChapterNumber,
                    ChapterTitle = chapterDto.ChapterTitle,
                    ChapterSubtitle = chapterDto.ChapterSubtitle,
                    ContentHtml = chapterDto.ContentHtml,
                    Summary = chapterDto.Summary,
                    WordCount = chapterDto.WordCount ?? CalculateWordCount(chapterDto.ContentHtml),
                    ReadingTimeMinutes = chapterDto.ReadingTimeMinutes ?? CalculateReadingTime(chapterDto.ContentHtml),
                    DisplayOrder = chapterDto.DisplayOrder ?? chapterDto.ChapterNumber,
                    IsActive = true
                };

                var chapterId = await _chapterRepository.CreateChapterAsync(chapter);
                chapter.Id = chapterId;

                _logger.LogInformation("Chapter created successfully with ID {ChapterId} for book {BookId}", 
                    chapterId, bookId);

                return await MapToDetailDtoAsync(chapter, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateChapterAsync for book {BookId}", bookId);
                throw;
            }
        }

        public async Task<ChapterDetailDto?> UpdateChapterAsync(int chapterId, ChapterUpdateDto chapterDto)
        {
            try
            {
                var existingChapter = await _chapterRepository.GetChapterByIdAsync(chapterId);
                if (existingChapter == null)
                {
                    _logger.LogWarning("Chapter with ID {ChapterId} not found for update", chapterId);
                    return null;
                }

                existingChapter.ChapterTitle = chapterDto.ChapterTitle;
                existingChapter.ChapterSubtitle = chapterDto.ChapterSubtitle;
                existingChapter.ContentHtml = chapterDto.ContentHtml;
                existingChapter.Summary = chapterDto.Summary;
                existingChapter.WordCount = chapterDto.WordCount ?? CalculateWordCount(chapterDto.ContentHtml);
                existingChapter.ReadingTimeMinutes = chapterDto.ReadingTimeMinutes ?? CalculateReadingTime(chapterDto.ContentHtml);
                existingChapter.DisplayOrder = chapterDto.DisplayOrder ?? existingChapter.DisplayOrder;
                existingChapter.IsActive = chapterDto.IsActive;

                var success = await _chapterRepository.UpdateChapterAsync(existingChapter);
                if (!success) return null;

                var book = await _bookRepository.GetByIdAsync(existingChapter.BookId);
                if (book == null) return null;

                _logger.LogInformation("Chapter with ID {ChapterId} updated successfully", chapterId);
                return await MapToDetailDtoAsync(existingChapter, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateChapterAsync for chapter ID {ChapterId}", chapterId);
                throw;
            }
        }

        public async Task<bool> DeleteChapterAsync(int chapterId)
        {
            try
            {
                var exists = await _chapterRepository.ExistsAsync(chapterId);
                if (!exists)
                {
                    _logger.LogWarning("Chapter with ID {ChapterId} not found for deletion", chapterId);
                    return false;
                }

                var success = await _chapterRepository.DeleteChapterAsync(chapterId);
                if (success)
                {
                    _logger.LogInformation("Chapter with ID {ChapterId} deleted successfully", chapterId);
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteChapterAsync for chapter ID {ChapterId}", chapterId);
                throw;
            }
        }

        #region Mapping Methods

        private ChapterSummaryDto MapToSummaryDto(BookChapter chapter)
        {
            return new ChapterSummaryDto
            {
                Id = chapter.Id,
                ChapterNumber = chapter.ChapterNumber,
                ChapterTitle = chapter.ChapterTitle,
                ChapterSubtitle = chapter.ChapterSubtitle,
                Summary = chapter.Summary,
                ReadingTimeMinutes = chapter.ReadingTimeMinutes,
                IsActive = chapter.IsActive
            };
        }

        private async Task<ChapterDetailDto> MapToDetailDtoAsync(BookChapter chapter, Book book)
        {
            var previousChapter = await _chapterRepository.GetPreviousChapterAsync(book.Id, chapter.ChapterNumber);
            var nextChapter = await _chapterRepository.GetNextChapterAsync(book.Id, chapter.ChapterNumber);

            return new ChapterDetailDto
            {
                Id = chapter.Id,
                BookId = book.Id,
                BookTitle = book.Title,
                BookAuthor = book.Author,
                ChapterNumber = chapter.ChapterNumber,
                ChapterTitle = chapter.ChapterTitle,
                ChapterSubtitle = chapter.ChapterSubtitle,
                ContentHtml = chapter.ContentHtml,
                Summary = chapter.Summary,
                WordCount = chapter.WordCount,
                ReadingTimeMinutes = chapter.ReadingTimeMinutes,
                HasPreviousChapter = previousChapter != null,
                HasNextChapter = nextChapter != null,
                PreviousChapterId = previousChapter?.Id,
                NextChapterId = nextChapter?.Id
            };
        }

        #endregion

        #region Helper Methods

        private int CalculateWordCount(string htmlContent)
        {
            // Simple word count - strips HTML tags and counts words
            var text = System.Text.RegularExpressions.Regex.Replace(htmlContent, "<.*?>", " ");
            var words = text.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }

        private int CalculateReadingTime(string htmlContent)
        {
            // Average reading speed: 200 words per minute
            var wordCount = CalculateWordCount(htmlContent);
            return Math.Max(1, (int)Math.Ceiling(wordCount / 200.0));
        }

        #endregion
    }
}

