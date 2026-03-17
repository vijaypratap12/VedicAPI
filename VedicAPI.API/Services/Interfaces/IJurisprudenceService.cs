using VedicAPI.API.Models.Common;
using VedicAPI.API.Models.DTOs;

namespace VedicAPI.API.Services.Interfaces
{
    /// <summary>
    /// Interface for Jurisprudence business logic operations
    /// </summary>
    public interface IJurisprudenceService
    {
        Task<PagedResult<JurisprudenceItemDto>> GetPagedAsync(
            string? searchTerm,
            string? type,
            int pageNumber,
            int pageSize,
            bool includeInactive = false,
            CancellationToken cancellationToken = default);

        Task<JurisprudenceItemDto?> GetByIdAsync(
            int id,
            bool includeInactive = false,
            CancellationToken cancellationToken = default);

        Task<JurisprudenceItemDto> CreateAsync(
            JurisprudenceCreateDto dto,
            string? documentUrl = null,
            CancellationToken cancellationToken = default);

        Task<JurisprudenceItemDto?> UpdateAsync(
            int id,
            JurisprudenceUpdateDto dto,
            string? documentUrl = null,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}
