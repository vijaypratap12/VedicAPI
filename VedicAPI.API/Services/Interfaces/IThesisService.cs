using VedicAPI.API.Models.DTOs;

namespace VedicAPI.API.Services.Interfaces
{
    /// <summary>
    /// Interface for Thesis business logic operations
    /// </summary>
    public interface IThesisService
    {
        Task<ThesisDetailDto?> GetThesisByIdAsync(int id);
        Task<IEnumerable<ThesisResponseDto>> GetAllThesisAsync();
        Task<IEnumerable<ThesisResponseDto>> GetFeaturedThesisAsync(int count = 3);
        Task<IEnumerable<ThesisResponseDto>> GetThesisByCategoryAsync(string category);
        Task<IEnumerable<ThesisResponseDto>> GetThesisByYearAsync(int year);
        Task<IEnumerable<ThesisResponseDto>> GetThesisByInstitutionAsync(string institution);
        Task<IEnumerable<ThesisResponseDto>> SearchThesisAsync(string searchTerm);
        Task<ThesisResponseDto> CreateThesisAsync(ThesisCreateDto thesisDto);
        Task<ThesisResponseDto?> UpdateThesisAsync(int id, ThesisUpdateDto thesisDto);
        Task<bool> DeleteThesisAsync(int id);
        Task<bool> IncrementDownloadCountAsync(int id);
    }
}

