using VedicAPI.API.Models.DTOs;

namespace VedicAPI.API.Services.Interfaces
{
    /// <summary>
    /// Service interface for Prakriti assessment business logic
    /// </summary>
    public interface IPrakritiAssessmentService
    {
        Task<IEnumerable<PrakritiQuestionDto>> GetQuestionsAsync();
        Task<PrakritiAssessmentResponseDto> SaveAssessmentAsync(PrakritiAssessmentRequestDto assessmentDto);
        Task<PrakritiAssessmentResponseDto?> GetAssessmentByIdAsync(long id);
        Task<PrakritiAssessmentResponseDto?> GetLatestAssessmentByPatientIdAsync(long patientId);
    }
}
