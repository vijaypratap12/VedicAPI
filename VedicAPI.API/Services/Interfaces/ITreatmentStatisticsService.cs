using VedicAPI.API.Models.DTOs;

namespace VedicAPI.API.Services.Interfaces
{
    /// <summary>
    /// Service interface for Treatment statistics business logic
    /// </summary>
    public interface ITreatmentStatisticsService
    {
        Task<TreatmentStatisticsDto> GetStatisticsAsync();
    }
}
