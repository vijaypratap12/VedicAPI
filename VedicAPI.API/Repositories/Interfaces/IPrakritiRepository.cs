using VedicAPI.API.Models;

namespace VedicAPI.API.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for Prakriti-related operations
    /// </summary>
    public interface IPrakritiRepository
    {
        Task<IEnumerable<PrakritiQuestion>> GetQuestionsAsync();
        Task<PrakritiAssessment?> GetAssessmentByIdAsync(long id);
        Task<PrakritiAssessment?> GetLatestAssessmentByPatientIdAsync(long patientId);
        Task<long> SaveAssessmentAsync(PrakritiAssessment assessment);
    }
}
