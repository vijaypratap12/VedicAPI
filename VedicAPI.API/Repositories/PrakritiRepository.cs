using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using VedicAPI.API.Models;
using VedicAPI.API.Repositories.Interfaces;

namespace VedicAPI.API.Repositories
{
    /// <summary>
    /// Repository implementation for Prakriti-related operations using Dapper
    /// </summary>
    public class PrakritiRepository : IPrakritiRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<PrakritiRepository> _logger;

        public PrakritiRepository(IConfiguration configuration, ILogger<PrakritiRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string cannot be null");
            _logger = logger;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<PrakritiQuestion>> GetQuestionsAsync()
        {
            try
            {
                using var connection = CreateConnection();
                return await connection.QueryAsync<PrakritiQuestion>(
                    "sp_GetPrakritiQuestions",
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Prakriti questions");
                throw;
            }
        }

        public async Task<PrakritiAssessment?> GetAssessmentByIdAsync(long id)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT Id, PatientId, Responses, VataScore, PittaScore, KaphaScore,
                           DominantPrakriti, AssessedAt
                    FROM PRAKRITIASSESSMENTS
                    WHERE Id = @Id";

                var result = await connection.QueryAsync<PrakritiAssessment>(sql, new { Id = id });
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Prakriti assessment with ID {AssessmentId}", id);
                throw;
            }
        }

        public async Task<PrakritiAssessment?> GetLatestAssessmentByPatientIdAsync(long patientId)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"
                    SELECT TOP 1 Id, PatientId, Responses, VataScore, PittaScore, KaphaScore,
                           DominantPrakriti, AssessedAt
                    FROM PRAKRITIASSESSMENTS
                    WHERE PatientId = @PatientId
                    ORDER BY AssessedAt DESC";

                var result = await connection.QueryAsync<PrakritiAssessment>(sql, new { PatientId = patientId });
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving latest Prakriti assessment for patient {PatientId}", patientId);
                throw;
            }
        }

        public async Task<long> SaveAssessmentAsync(PrakritiAssessment assessment)
        {
            try
            {
                using var connection = CreateConnection();
                var result = await connection.QueryAsync<long>(
                    "sp_SavePrakritiAssessment",
                    new
                    {
                        assessment.PatientId,
                        assessment.Responses,
                        assessment.VataScore,
                        assessment.PittaScore,
                        assessment.KaphaScore,
                        assessment.DominantPrakriti
                    },
                    commandType: CommandType.StoredProcedure
                );
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving Prakriti assessment for patient {PatientId}", assessment.PatientId);
                throw;
            }
        }
    }
}
