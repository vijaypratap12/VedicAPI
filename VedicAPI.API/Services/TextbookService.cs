using VedicAPI.API.Models;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Repositories.Interfaces;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Services
{
    /// <summary>
    /// Service implementation for Textbook business logic
    /// </summary>
    public class TextbookService : ITextbookService
    {
        private readonly ITextbookRepository _textbookRepository;
        private readonly ITextbookChapterRepository _chapterRepository;
        private readonly ILogger<TextbookService> _logger;

        public TextbookService(
            ITextbookRepository textbookRepository, 
            ITextbookChapterRepository chapterRepository,
            ILogger<TextbookService> logger)
        {
            _textbookRepository = textbookRepository;
            _chapterRepository = chapterRepository;
            _logger = logger;
        }

        public async Task<TextbookResponseDto?> GetTextbookByIdAsync(int id)
        {
            try
            {
                var textbook = await _textbookRepository.GetByIdAsync(id);
                return textbook != null ? MapToResponseDto(textbook) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTextbookByIdAsync for ID {TextbookId}", id);
                throw;
            }
        }

        public async Task<TextbookWithChaptersDto?> GetTextbookWithChaptersAsync(int id)
        {
            try
            {
                var textbook = await _textbookRepository.GetByIdAsync(id);
                if (textbook == null)
                {
                    return null;
                }

                var chapters = await _chapterRepository.GetByTextbookIdAsync(id);
                
                return new TextbookWithChaptersDto
                {
                    Id = textbook.Id,
                    Title = textbook.Title,
                    Author = textbook.Author,
                    Description = textbook.Description,
                    CoverImageUrl = textbook.CoverImageUrl,
                    TotalChapters = textbook.TotalChapters,
                    Category = textbook.Category,
                    Language = textbook.Language,
                    PublicationYear = textbook.PublicationYear,
                    ISBN = textbook.ISBN,
                    Rating = textbook.Rating,
                    DownloadCount = textbook.DownloadCount,
                    ViewCount = textbook.ViewCount,
                    Status = textbook.Status,
                    Tags = textbook.Tags,
                    Level = textbook.Level,
                    Year = textbook.Year,
                    PageCount = textbook.PageCount,
                    CreatedAt = textbook.CreatedAt,
                    Chapters = chapters.Select(c => new TextbookChapterSummaryDto
                    {
                        Id = c.Id,
                        ChapterNumber = c.ChapterNumber,
                        ChapterTitle = c.ChapterTitle,
                        ChapterSubtitle = c.ChapterSubtitle,
                        Summary = c.Summary,
                        WordCount = c.WordCount,
                        ReadingTimeMinutes = c.ReadingTimeMinutes,
                        DisplayOrder = c.DisplayOrder
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTextbookWithChaptersAsync for ID {TextbookId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<TextbookResponseDto>> GetAllTextbooksAsync()
        {
            try
            {
                var textbooks = await _textbookRepository.GetAllAsync();
                return textbooks.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllTextbooksAsync");
                throw;
            }
        }

        public async Task<IEnumerable<TextbookResponseDto>> GetActiveTextbooksAsync()
        {
            try
            {
                var textbooks = await _textbookRepository.GetActiveAsync();
                return textbooks.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetActiveTextbooksAsync");
                throw;
            }
        }

        public async Task<IEnumerable<TextbookResponseDto>> GetTextbooksByCategoryAsync(string category)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(category))
                {
                    throw new ArgumentException("Category cannot be empty", nameof(category));
                }

                var textbooks = await _textbookRepository.GetByCategoryAsync(category);
                return textbooks.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTextbooksByCategoryAsync for category {Category}", category);
                throw;
            }
        }

        public async Task<IEnumerable<TextbookResponseDto>> GetTextbooksByLevelAsync(string level)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(level))
                {
                    throw new ArgumentException("Level cannot be empty", nameof(level));
                }

                var textbooks = await _textbookRepository.GetByLevelAsync(level);
                return textbooks.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTextbooksByLevelAsync for level {Level}", level);
                throw;
            }
        }

        public async Task<IEnumerable<TextbookResponseDto>> SearchTextbooksAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return await GetActiveTextbooksAsync();
                }

                var textbooks = await _textbookRepository.SearchAsync(searchTerm);
                return textbooks.Select(MapToResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SearchTextbooksAsync with term {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<TextbookResponseDto> CreateTextbookAsync(TextbookCreateDto textbookDto)
        {
            if (textbookDto == null)
            {
                throw new ArgumentNullException(nameof(textbookDto));
            }

            try
            {
                var textbook = MapToEntity(textbookDto);
                var textbookId = await _textbookRepository.CreateAsync(textbook);
                textbook.Id = textbookId;

                _logger.LogInformation("Textbook created successfully with ID {TextbookId}", textbookId);
                return MapToResponseDto(textbook);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateTextbookAsync for textbook {TextbookTitle}", textbookDto.Title);
                throw;
            }
        }

        public async Task<TextbookResponseDto?> UpdateTextbookAsync(int id, TextbookUpdateDto textbookDto)
        {
            if (textbookDto == null)
            {
                throw new ArgumentNullException(nameof(textbookDto));
            }

            try
            {
                var existingTextbook = await _textbookRepository.GetByIdAsync(id);
                if (existingTextbook == null)
                {
                    _logger.LogWarning("Textbook with ID {TextbookId} not found for update", id);
                    return null;
                }

                var updatedTextbook = MapToEntity(textbookDto, id);
                updatedTextbook.CreatedAt = existingTextbook.CreatedAt;
                updatedTextbook.DownloadCount = existingTextbook.DownloadCount;
                updatedTextbook.ViewCount = existingTextbook.ViewCount;

                var success = await _textbookRepository.UpdateAsync(updatedTextbook);
                if (success)
                {
                    _logger.LogInformation("Textbook with ID {TextbookId} updated successfully", id);
                    return MapToResponseDto(updatedTextbook);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateTextbookAsync for textbook ID {TextbookId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteTextbookAsync(int id)
        {
            try
            {
                var exists = await _textbookRepository.ExistsAsync(id);
                if (!exists)
                {
                    _logger.LogWarning("Textbook with ID {TextbookId} not found for deletion", id);
                    return false;
                }

                var success = await _textbookRepository.DeleteAsync(id);
                if (success)
                {
                    _logger.LogInformation("Textbook with ID {TextbookId} deleted successfully", id);
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteTextbookAsync for textbook ID {TextbookId}", id);
                throw;
            }
        }

        public async Task<bool> IncrementDownloadCountAsync(int id)
        {
            try
            {
                return await _textbookRepository.IncrementDownloadCountAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in IncrementDownloadCountAsync for textbook ID {TextbookId}", id);
                throw;
            }
        }

        public async Task<bool> IncrementViewCountAsync(int id)
        {
            try
            {
                return await _textbookRepository.IncrementViewCountAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in IncrementViewCountAsync for textbook ID {TextbookId}", id);
                throw;
            }
        }

        #region Mapping Methods

        private static TextbookResponseDto MapToResponseDto(Textbook textbook)
        {
            return new TextbookResponseDto
            {
                Id = textbook.Id,
                Title = textbook.Title,
                Author = textbook.Author,
                Description = textbook.Description,
                CoverImageUrl = textbook.CoverImageUrl,
                TotalChapters = textbook.TotalChapters,
                Category = textbook.Category,
                Language = textbook.Language,
                PublicationYear = textbook.PublicationYear,
                ISBN = textbook.ISBN,
                Rating = textbook.Rating,
                DownloadCount = textbook.DownloadCount,
                ViewCount = textbook.ViewCount,
                Status = textbook.Status,
                Tags = textbook.Tags,
                Level = textbook.Level,
                Year = textbook.Year,
                PageCount = textbook.PageCount,
                CreatedAt = textbook.CreatedAt,
                UpdatedAt = textbook.UpdatedAt,
                IsActive = textbook.IsActive
            };
        }

        private static Textbook MapToEntity(TextbookCreateDto dto)
        {
            return new Textbook
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
                Rating = dto.Rating ?? 4.5m,
                DownloadCount = 0,
                ViewCount = 0,
                Status = dto.Status ?? "completed",
                Tags = dto.Tags,
                Level = dto.Level,
                Year = dto.Year,
                PageCount = dto.PageCount,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }

        private static Textbook MapToEntity(TextbookUpdateDto dto, int id)
        {
            return new Textbook
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
                Rating = dto.Rating,
                Status = dto.Status,
                Tags = dto.Tags,
                Level = dto.Level,
                Year = dto.Year,
                PageCount = dto.PageCount,
                IsActive = dto.IsActive,
                UpdatedAt = DateTime.UtcNow
            };
        }

        #endregion
    }
}

