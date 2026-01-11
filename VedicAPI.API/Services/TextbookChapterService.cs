using VedicAPI.API.Models;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Repositories.Interfaces;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Services
{
    /// <summary>
    /// Service for TextbookChapter business logic
    /// </summary>
    public class TextbookChapterService : ITextbookChapterService
    {
        private readonly ITextbookChapterRepository _chapterRepository;
        private readonly ITextbookRepository _textbookRepository;
        private readonly ILogger<TextbookChapterService> _logger;

        public TextbookChapterService(
            ITextbookChapterRepository chapterRepository,
            ITextbookRepository textbookRepository,
            ILogger<TextbookChapterService> logger)
        {
            _chapterRepository = chapterRepository ?? throw new ArgumentNullException(nameof(chapterRepository));
            _textbookRepository = textbookRepository ?? throw new ArgumentNullException(nameof(textbookRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get a textbook chapter by ID
        /// </summary>
        public async Task<TextbookChapterDetailDto?> GetChapterByIdAsync(int id)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);

            var chapter = await _chapterRepository.GetByIdAsync(id);
            return chapter == null ? null : MapToDetailDto(chapter);
        }

        /// <summary>
        /// Get all chapters for a specific textbook
        /// </summary>
        public async Task<IEnumerable<TextbookChapterSummaryDto>> GetChaptersByTextbookIdAsync(int textbookId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(textbookId);

            var chapters = await _chapterRepository.GetByTextbookIdAsync(textbookId);
            return chapters.Select(MapToSummaryDto);
        }

        /// <summary>
        /// Create a new textbook chapter
        /// </summary>
        public async Task<TextbookChapterDetailDto> CreateChapterAsync(int textbookId, TextbookChapterCreateDto chapterDto)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(textbookId);
            ArgumentNullException.ThrowIfNull(chapterDto);

            // Verify textbook exists
            var textbookExists = await _textbookRepository.ExistsAsync(textbookId);
            if (!textbookExists)
            {
                throw new ArgumentException($"Textbook with ID {textbookId} not found", nameof(textbookId));
            }

            // Check if chapter number already exists for this textbook
            var chapterNumberExists = await _chapterRepository.ChapterNumberExistsAsync(
                textbookId, 
                chapterDto.ChapterNumber);

            if (chapterNumberExists)
            {
                throw new InvalidOperationException(
                    $"Chapter number {chapterDto.ChapterNumber} already exists for this textbook");
            }

            var chapter = new TextbookChapter
            {
                TextbookId = textbookId,
                ChapterNumber = chapterDto.ChapterNumber,
                ChapterTitle = chapterDto.ChapterTitle,
                ChapterSubtitle = chapterDto.ChapterSubtitle,
                ContentHtml = chapterDto.ContentHtml,
                Summary = chapterDto.Summary,
                WordCount = chapterDto.WordCount,
                ReadingTimeMinutes = chapterDto.ReadingTimeMinutes,
                DisplayOrder = chapterDto.DisplayOrder ?? chapterDto.ChapterNumber, // Default to chapter number if not provided
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var chapterId = await _chapterRepository.CreateAsync(chapter);
            chapter.Id = chapterId;

            _logger.LogInformation("Created textbook chapter {ChapterId} for textbook {TextbookId}", chapterId, textbookId);

            return MapToDetailDto(chapter);
        }

        /// <summary>
        /// Update an existing textbook chapter
        /// </summary>
        public async Task<TextbookChapterDetailDto?> UpdateChapterAsync(int id, TextbookChapterUpdateDto chapterDto)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
            ArgumentNullException.ThrowIfNull(chapterDto);

            var existingChapter = await _chapterRepository.GetByIdAsync(id);
            if (existingChapter == null)
            {
                return null;
            }

            // Check if chapter number already exists for this textbook (excluding current chapter)
            var chapterNumberExists = await _chapterRepository.ChapterNumberExistsAsync(
                existingChapter.TextbookId,
                chapterDto.ChapterNumber,
                id);

            if (chapterNumberExists)
            {
                throw new InvalidOperationException(
                    $"Chapter number {chapterDto.ChapterNumber} already exists for this textbook");
            }

            existingChapter.ChapterNumber = chapterDto.ChapterNumber;
            existingChapter.ChapterTitle = chapterDto.ChapterTitle;
            existingChapter.ChapterSubtitle = chapterDto.ChapterSubtitle;
            existingChapter.ContentHtml = chapterDto.ContentHtml;
            existingChapter.Summary = chapterDto.Summary;
            existingChapter.WordCount = chapterDto.WordCount;
            existingChapter.ReadingTimeMinutes = chapterDto.ReadingTimeMinutes;
            existingChapter.DisplayOrder = chapterDto.DisplayOrder;
            existingChapter.IsActive = chapterDto.IsActive;
            existingChapter.UpdatedAt = DateTime.UtcNow;

            var success = await _chapterRepository.UpdateAsync(existingChapter);
            
            if (success)
            {
                _logger.LogInformation("Updated textbook chapter {ChapterId}", id);
            }

            return success ? MapToDetailDto(existingChapter) : null;
        }

        /// <summary>
        /// Delete a textbook chapter
        /// </summary>
        public async Task<bool> DeleteChapterAsync(int id)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);

            var success = await _chapterRepository.DeleteAsync(id);
            
            if (success)
            {
                _logger.LogInformation("Deleted textbook chapter {ChapterId}", id);
            }

            return success;
        }

        /// <summary>
        /// Map TextbookChapter entity to TextbookChapterDetailDto
        /// </summary>
        private static TextbookChapterDetailDto MapToDetailDto(TextbookChapter chapter)
        {
            return new TextbookChapterDetailDto
            {
                Id = chapter.Id,
                TextbookId = chapter.TextbookId,
                ChapterNumber = chapter.ChapterNumber,
                ChapterTitle = chapter.ChapterTitle,
                ChapterSubtitle = chapter.ChapterSubtitle,
                ContentHtml = chapter.ContentHtml,
                Summary = chapter.Summary,
                WordCount = chapter.WordCount,
                ReadingTimeMinutes = chapter.ReadingTimeMinutes,
                DisplayOrder = chapter.DisplayOrder,
                CreatedAt = chapter.CreatedAt,
                UpdatedAt = chapter.UpdatedAt,
                IsActive = chapter.IsActive
            };
        }

        /// <summary>
        /// Map TextbookChapter entity to TextbookChapterSummaryDto
        /// </summary>
        private static TextbookChapterSummaryDto MapToSummaryDto(TextbookChapter chapter)
        {
            return new TextbookChapterSummaryDto
            {
                Id = chapter.Id,
                ChapterNumber = chapter.ChapterNumber,
                ChapterTitle = chapter.ChapterTitle,
                ChapterSubtitle = chapter.ChapterSubtitle,
                Summary = chapter.Summary,
                WordCount = chapter.WordCount,
                ReadingTimeMinutes = chapter.ReadingTimeMinutes,
                DisplayOrder = chapter.DisplayOrder
            };
        }
    }
}

