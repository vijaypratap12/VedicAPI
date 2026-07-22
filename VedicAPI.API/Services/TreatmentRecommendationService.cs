using System.Text.Json;
using VedicAPI.API.Models;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Repositories.Interfaces;
using VedicAPI.API.Services.AI;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Services
{
    /// <summary>
    /// Service implementation for Treatment recommendation business logic with AI Engine support
    /// </summary>
    public class TreatmentRecommendationService : ITreatmentRecommendationService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IConditionRepository _conditionRepository;
        private readonly IHerbalMedicineRepository _herbalMedicineRepository;
        private readonly IYogaAsanaRepository _yogaAsanaRepository;
        private readonly ITreatmentRepository _treatmentRepository;
        private readonly VedicAiClientFactory _aiClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TreatmentRecommendationService> _logger;

        public TreatmentRecommendationService(
            IPatientRepository patientRepository,
            IConditionRepository conditionRepository,
            IHerbalMedicineRepository herbalMedicineRepository,
            IYogaAsanaRepository yogaAsanaRepository,
            ITreatmentRepository treatmentRepository,
            VedicAiClientFactory aiClientFactory,
            IConfiguration configuration,
            ILogger<TreatmentRecommendationService> logger)
        {
            _patientRepository = patientRepository;
            _conditionRepository = conditionRepository;
            _herbalMedicineRepository = herbalMedicineRepository;
            _yogaAsanaRepository = yogaAsanaRepository;
            _treatmentRepository = treatmentRepository;
            _aiClientFactory = aiClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public AiConfigDto GetAiConfig()
        {
            var enabled = _configuration.GetValue<bool>("AiSettings:Enabled", true);
            var provider = _configuration.GetValue<string>("AiSettings:Provider") ?? "Gemini";
            var model = _configuration.GetValue<string>("AiSettings:ModelName") ?? "gemini-1.5-flash";
            var enableFallback = _configuration.GetValue<bool>("AiSettings:EnableFallback", true);

            var features = new Dictionary<string, bool>
            {
                { "Recommendations", _configuration.GetValue<bool>("AiSettings:Features:Recommendations", true) },
                { "SushrutaAssistant", _configuration.GetValue<bool>("AiSettings:Features:SushrutaAssistant", false) },
                { "ResearchAnalyzer", _configuration.GetValue<bool>("AiSettings:Features:ResearchAnalyzer", false) }
            };

            return new AiConfigDto
            {
                Enabled = enabled,
                Provider = provider,
                ModelName = model,
                EnableFallback = enableFallback,
                Features = features
            };
        }

        public async Task<TreatmentRecommendationDto> GenerateRecommendationsAsync(
            long patientId, 
            long conditionId, 
            bool useAi = true, 
            string? customNotes = null,
            string? customConditionName = null)
        {
            try
            {
                var patient = await _patientRepository.GetByIdAsync(patientId);
                if (patient == null) throw new ArgumentException($"Patient with ID {patientId} not found");

                Condition condition;
                if (conditionId <= 0)
                {
                    if (string.IsNullOrWhiteSpace(customConditionName))
                    {
                        throw new ArgumentException("Condition ID or custom condition name must be provided");
                    }

                    var trimmedName = customConditionName.Trim();
                    var existing = await _conditionRepository.SearchAsync(trimmedName);
                    var match = existing.FirstOrDefault(c => c.Name.Equals(trimmedName, StringComparison.OrdinalIgnoreCase));
                    if (match != null)
                    {
                        condition = match;
                    }
                    else
                    {
                        var newCondition = new Condition
                        {
                            Name = trimmedName,
                            SanskritName = "Vyadhi",
                            Category = "General",
                            Description = "Custom patient-reported condition created via treatment recommendations workflow.",
                            CommonSymptoms = "Custom symptoms",
                            AffectedDoshas = "Tridosha",
                            Severity = "Moderate",
                            IsActive = true
                        };
                        var newId = await _conditionRepository.CreateAsync(newCondition);
                        newCondition.Id = newId;
                        condition = newCondition;
                    }
                }
                else
                {
                    var cond = await _conditionRepository.GetByIdAsync(conditionId);
                    if (cond == null) throw new ArgumentException($"Condition with ID {conditionId} not found");
                    condition = cond;
                }

                var prakriti = patient.Prakriti ?? "All";
                var hasPrakritiAssessment = !string.IsNullOrEmpty(patient.Prakriti);

                // Candidate items from DB
                var medicines = await _treatmentRepository.GetRecommendedMedicinesAsync(condition.Id, prakriti);
                var yogaAsanas = await _treatmentRepository.GetRecommendedYogaAsanasAsync(condition.Id, prakriti);
                var dietaryItems = await _treatmentRepository.GetDietaryItemsByPrakritiAsync(prakriti);

                var aiConfig = GetAiConfig();

                // Check Feature Flag for AI
                if (useAi && aiConfig.Enabled && aiConfig.Features.GetValueOrDefault("Recommendations", true))
                {
                    var provider = aiConfig.Provider;
                    var apiKey = _configuration.GetValue<string>("AiSettings:ApiKey") ?? "";
                    var modelName = aiConfig.ModelName;

                    var aiClient = _aiClientFactory.GetClient(provider);
                    if (aiClient != null)
                    {
                        _logger.LogInformation("Attempting AI recommendation generation using provider {Provider}...", provider);
                        var aiRecommendation = await aiClient.GenerateClinicalRecommendationAsync(
                            patient, condition, medicines, yogaAsanas, dietaryItems, apiKey, modelName, customNotes);

                        if (aiRecommendation != null)
                        {
                            return aiRecommendation;
                        }
                        _logger.LogWarning("AI Recommendation returned null. Falling back to DB rule calculation.");
                    }
                }

                // Rule-Based EF Core Calculation (Fallback / Default)
                var medicineList = medicines.Select(MapMedicineToDto).ToList();
                var yogaList = yogaAsanas.Select(MapYogaToDto).ToList();
                var dietaryList = dietaryItems.Select(MapDietaryToDto).ToList();
                var lifestyleModifications = GenerateLifestyleModifications(condition, prakriti);
                var confidenceScore = CalculateConfidenceScore(hasPrakritiAssessment, medicineList.Count, yogaList.Count, patient.Age);
                var explanation = GenerateExplanation(patient, condition, medicineList.Count, yogaList.Count, hasPrakritiAssessment);

                return new TreatmentRecommendationDto
                {
                    PatientId = patient.Id,
                    PatientName = patient.Name,
                    Prakriti = prakriti,
                    ConditionId = condition.Id,
                    ConditionName = condition.Name,
                    RecommendedMedicines = medicineList,
                    RecommendedYogaAsanas = yogaList,
                    RecommendedDietaryItems = dietaryList,
                    LifestyleModifications = lifestyleModifications,
                    ConfidenceScore = confidenceScore,
                    Explanation = explanation,
                    IsAiGenerated = false,
                    AiModelUsed = "Rule-Based Database Engine",
                    ClinicalRationale = "Recommendation derived from classical database mapping rules.",
                    PathyaList = dietaryList.Select(d => d.FoodName).ToList(),
                    ApathyaList = new List<string> { "Heavy, unctuous, or processed foods incompatible with condition" },
                    Warnings = new List<string> { "Consult practitioner if symptoms persist." }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating recommendations for patient {PatientId} and condition {ConditionId}", patientId, conditionId);
                throw;
            }
        }

        public async Task<IEnumerable<ConditionDto>> SearchConditionsAsync(string searchTerm, string? category = null)
        {
            var conditions = await _conditionRepository.SearchAsync(searchTerm, category);
            return conditions.Select(MapConditionToDto);
        }

        public async Task<IEnumerable<HerbalMedicineDto>> GetHerbalMedicinesAsync(string? searchTerm = null, string? prakritiEffect = null)
        {
            IEnumerable<HerbalMedicine> medicines;
            if (!string.IsNullOrEmpty(searchTerm)) medicines = await _herbalMedicineRepository.SearchAsync(searchTerm);
            else if (!string.IsNullOrEmpty(prakritiEffect)) medicines = await _herbalMedicineRepository.GetByPrakritiEffectAsync(prakritiEffect);
            else medicines = await _herbalMedicineRepository.GetAllAsync();
            return medicines.Select(MapMedicineToDto);
        }

        public async Task<IEnumerable<YogaAsanaDto>> GetYogaAsanasAsync(string? category = null, string? difficulty = null, string? prakritiEffect = null)
        {
            IEnumerable<YogaAsana> asanas;
            if (!string.IsNullOrEmpty(category)) asanas = await _yogaAsanaRepository.GetByCategoryAsync(category);
            else if (!string.IsNullOrEmpty(difficulty)) asanas = await _yogaAsanaRepository.GetByDifficultyAsync(difficulty);
            else if (!string.IsNullOrEmpty(prakritiEffect)) asanas = await _yogaAsanaRepository.GetByPrakritiEffectAsync(prakritiEffect);
            else asanas = await _yogaAsanaRepository.GetAllAsync();
            return asanas.Select(MapYogaToDto);
        }

        public async Task<IEnumerable<DietaryItemDto>> GetDietaryItemsAsync(string prakriti, string? category = null)
        {
            var items = await _treatmentRepository.GetDietaryItemsByPrakritiAsync(prakriti, category);
            return items.Select(MapDietaryToDto);
        }

        #region Helper Methods

        private decimal CalculateConfidenceScore(bool hasPrakritiAssessment, int medicineCount, int yogaCount, int patientAge)
        {
            decimal score = 60m;
            if (hasPrakritiAssessment) score += 15m;
            if (medicineCount >= 3 && yogaCount >= 3) score += 10m;
            else if (medicineCount >= 2 || yogaCount >= 2) score += 5m;
            score += 10m;
            if (patientAge >= 18 && patientAge <= 70) score += 5m;
            return Math.Min(score, 100m);
        }

        private string GenerateExplanation(Patient patient, Condition condition, int medicineCount, int yogaCount, bool hasPrakritiAssessment)
        {
            var explanation = $"Treatment plan for {condition.Name}";
            if (hasPrakritiAssessment) explanation += $" personalized for {patient.Prakriti} Prakriti constitution. ";
            else explanation += " with general recommendations. ";
            explanation += $"This plan includes {medicineCount} herbal medicine(s) and {yogaCount} yoga practice(s) selected based on traditional Ayurvedic principles.";
            return explanation;
        }

        private List<string> GenerateLifestyleModifications(Condition condition, string prakriti)
        {
            var modifications = new List<string>
            {
                "Maintain regular sleep schedule (10 PM - 6 AM)",
                "Practice stress management techniques daily",
                "Stay well hydrated with warm water"
            };

            if (prakriti.Contains("Vata")) modifications.Add("Keep warm and maintain daily routine regularity");
            else if (prakriti.Contains("Pitta")) modifications.Add("Avoid excessive heat and intense sun exposure");
            else if (prakriti.Contains("Kapha")) modifications.Add("Stay physically active and avoid sedentary lifestyle");

            return modifications;
        }

        private static ConditionDto MapConditionToDto(Condition c) => new() { Id = c.Id, Name = c.Name, SanskritName = c.SanskritName, Category = c.Category, Description = c.Description };
        private static HerbalMedicineDto MapMedicineToDto(HerbalMedicine m) => new() { Id = m.Id, CommonName = m.CommonName, SanskritName = m.SanskritName, ScientificName = m.ScientificName, Dosage = m.Dosage };
        private static YogaAsanaDto MapYogaToDto(YogaAsana a) => new() { Id = a.Id, AsanaName = a.AsanaName, SanskritName = a.SanskritName, Duration = a.Duration, Difficulty = a.Difficulty };
        private static DietaryItemDto MapDietaryToDto(DietaryItem i) => new() { Id = i.Id, FoodName = i.FoodName, Category = i.Category, Benefits = i.Benefits };

        public async Task<TreatmentRecommendationDto?> SuggestAdjustmentAsync(
            long patientId, 
            long conditionId, 
            TreatmentPlanResponseDto currentPlan, 
            IEnumerable<TreatmentOutcomeDto> outcomes)
        {
            try
            {
                var patient = await _patientRepository.GetByIdAsync(patientId);
                if (patient == null) throw new ArgumentException($"Patient with ID {patientId} not found");

                var condition = await _conditionRepository.GetByIdAsync(conditionId);
                if (condition == null) throw new ArgumentException($"Condition with ID {conditionId} not found");

                var aiConfig = GetAiConfig();

                if (aiConfig.Enabled && aiConfig.Features.GetValueOrDefault("Recommendations", true))
                {
                    var provider = aiConfig.Provider;
                    var apiKey = _configuration.GetValue<string>("AiSettings:ApiKey") ?? "";
                    var modelName = aiConfig.ModelName;

                    var aiClient = _aiClientFactory.GetClient(provider);
                    if (aiClient != null)
                    {
                        _logger.LogInformation("Attempting AI adjustment suggestion using provider {Provider}...", provider);
                        return await aiClient.SuggestAdjustmentAsync(patient, condition, currentPlan, outcomes, apiKey, modelName);
                    }
                }

                _logger.LogWarning("AI Service is disabled or client could not be loaded. Cannot suggest adjustment.");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error suggesting plan adjustments for patient {PatientId} and plan {PlanId}", patientId, currentPlan.Id);
                throw;
            }
        }

        #endregion
    }
}
