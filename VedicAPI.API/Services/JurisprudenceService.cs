using VedicAPI.API.Models;
using VedicAPI.API.Models.Common;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Repositories.Interfaces;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Services
{
    /// <summary>
    /// Service for Jurisprudence business logic
    /// </summary>
    public class JurisprudenceService : IJurisprudenceService
    {
        private readonly IJurisprudenceRepository _repository;
        private readonly ILogger<JurisprudenceService> _logger;

        public JurisprudenceService(
            IJurisprudenceRepository repository,
            ILogger<JurisprudenceService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<PagedResult<JurisprudenceItemDto>> GetPagedAsync(
            string? searchTerm,
            string? type,
            int pageNumber,
            int pageSize,
            bool includeInactive = false,
            CancellationToken cancellationToken = default)
        {
            var (totalCount, items) = await _repository.GetPagedAsync(
                searchTerm,
                type,
                pageNumber,
                pageSize,
                includeInactive,
                cancellationToken);

            return new PagedResult<JurisprudenceItemDto>
            {
                Items = items.Select(MapToDto),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<JurisprudenceItemDto?> GetByIdAsync(
            int id,
            bool includeInactive = false,
            CancellationToken cancellationToken = default)
        {
            var item = await _repository.GetByIdAsync(id, includeInactive, cancellationToken);
            return item == null ? null : MapToDto(item);
        }

        public async Task<JurisprudenceItemDto> CreateAsync(
            JurisprudenceCreateDto dto,
            string? documentUrl = null,
            CancellationToken cancellationToken = default)
        {
            var entity = new JurisprudenceItem
            {
                Title = dto.Title,
                Type = dto.Type,
                Date = dto.Date,
                Description = dto.Description,
                DocumentUrl = documentUrl ?? dto.DocumentUrl,
                State = dto.State,
                DisplayOrder = dto.DisplayOrder,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var id = await _repository.CreateAsync(entity, cancellationToken);
            entity.Id = id;

            return MapToDto(entity);
        }

        public async Task<JurisprudenceItemDto?> UpdateAsync(
            int id,
            JurisprudenceUpdateDto dto,
            string? documentUrl = null,
            CancellationToken cancellationToken = default)
        {
            var existing = await _repository.GetByIdAsync(id, includeInactive: true, cancellationToken);
            if (existing == null)
                return null;

            existing.Title = dto.Title;
            existing.Type = dto.Type;
            existing.Date = dto.Date;
            existing.Description = dto.Description;
            if (documentUrl != null)
                existing.DocumentUrl = documentUrl;
            else if (dto.DocumentUrl != null)
                existing.DocumentUrl = dto.DocumentUrl;
            existing.State = dto.State;
            existing.DisplayOrder = dto.DisplayOrder;
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(existing, cancellationToken);
            return updated ? MapToDto(existing) : null;
        }

        public async Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            return await _repository.SoftDeleteAsync(id, cancellationToken);
        }

        private static JurisprudenceItemDto MapToDto(JurisprudenceItem item)
        {
            return new JurisprudenceItemDto
            {
                Id = item.Id,
                Title = item.Title,
                Type = item.Type,
                Date = item.Date,
                Description = item.Description,
                DocumentUrl = item.DocumentUrl,
                State = item.State,
                DisplayOrder = item.DisplayOrder,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt
            };
        }
    }
}
