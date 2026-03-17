using VedicAPI.API.Models;

namespace VedicAPI.API.Repositories.Interfaces
{
    /// <summary>
    /// Interface for Jurisprudence data access operations
    /// </summary>
    public interface IJurisprudenceRepository
    {
        Task<(int TotalCount, IEnumerable<JurisprudenceItem> Items)> GetPagedAsync(
            string? searchTerm,
            string? type,
            int pageNumber,
            int pageSize,
            bool includeInactive = false,
            CancellationToken cancellationToken = default);

        Task<JurisprudenceItem?> GetByIdAsync(
            int id,
            bool includeInactive = false,
            CancellationToken cancellationToken = default);

        Task<int> CreateAsync(
            JurisprudenceItem entity,
            CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(
            JurisprudenceItem entity,
            CancellationToken cancellationToken = default);

        Task<bool> SoftDeleteAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}
