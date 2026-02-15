using VedicAPI.API.Models;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Repositories.Interfaces;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Services
{
    /// <summary>
    /// Service implementation for Prakriti assessment business logic
    /// </summary>
    public class PrakritiAssessmentService : IPrakritiAssessmentService
    {
        private readonly IPrakritiRepository _prakritiRepository;
        private readonly ILogger<PrakritiAssessmentService> _logger;

        public PrakritiAssessmentService(
            IPrakritiRepository prakritiRepository,
            ILogger<PrakritiAssessmentService> logger)
        {
            _prakritiRepository = prakritiRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<PrakritiQuestionDto>> GetQuestionsAsync()
        {
            try
            {
                var questions = await _prakritiRepository.GetQuestionsAsync();
                return questions.Select(MapQuestionToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetQuestionsAsync");
                throw;
            }
        }

        public async Task<PrakritiAssessmentResponseDto> SaveAssessmentAsync(PrakritiAssessmentRequestDto assessmentDto)
        {
            try
            {
                var assessment = MapToEntity(assessmentDto);
                var assessmentId = await _prakritiRepository.SaveAssessmentAsync(assessment);
                assessment.Id = assessmentId;

                _logger.LogInformation("Prakriti assessment saved successfully for patient {PatientId}", assessmentDto.PatientId);
                return MapAssessmentToDto(assessment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SaveAssessmentAsync for patient {PatientId}", assessmentDto.PatientId);
                throw;
            }
        }

        public async Task<PrakritiAssessmentResponseDto?> GetAssessmentByIdAsync(long id)
        {
            try
            {
                var assessment = await _prakritiRepository.GetAssessmentByIdAsync(id);
                return assessment != null ? MapAssessmentToDto(assessment) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAssessmentByIdAsync for ID {AssessmentId}", id);
                throw;
            }
        }

        public async Task<PrakritiAssessmentResponseDto?> GetLatestAssessmentByPatientIdAsync(long patientId)
        {
            try
            {
                var assessment = await _prakritiRepository.GetLatestAssessmentByPatientIdAsync(patientId);
                return assessment != null ? MapAssessmentToDto(assessment) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetLatestAssessmentByPatientIdAsync for patient {PatientId}", patientId);
                throw;
            }
        }

        #region Mapping Methods

        private static PrakritiQuestionDto MapQuestionToDto(PrakritiQuestion question)
        {
            return new PrakritiQuestionDto
            {
                Id = question.Id,
                Category = question.Category,
                Question = question.Question,
                VataOption = question.VataOption,
                PittaOption = question.PittaOption,
                KaphaOption = question.KaphaOption,
                DisplayOrder = question.DisplayOrder
            };
        }

        private static PrakritiAssessmentResponseDto MapAssessmentToDto(PrakritiAssessment assessment)
        {
            return new PrakritiAssessmentResponseDto
            {
                Id = assessment.Id,
                PatientId = assessment.PatientId,
                VataScore = assessment.VataScore,
                PittaScore = assessment.PittaScore,
                KaphaScore = assessment.KaphaScore,
                DominantPrakriti = assessment.DominantPrakriti,
                AssessedAt = assessment.AssessedAt
            };
        }

        private static PrakritiAssessment MapToEntity(PrakritiAssessmentRequestDto dto)
        {
            return new PrakritiAssessment
            {
                PatientId = dto.PatientId,
                Responses = dto.Responses,
                VataScore = dto.VataScore,
                PittaScore = dto.PittaScore,
                KaphaScore = dto.KaphaScore,
                DominantPrakriti = dto.DominantPrakriti,
                AssessedAt = DateTime.UtcNow
            };
        }

        #endregion
    }
}
