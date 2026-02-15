using System.Text.Json;
using VedicAPI.API.Models;
using VedicAPI.API.Models.DTOs;
using VedicAPI.API.Repositories.Interfaces;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Services
{
    /// <summary>
    /// Service implementation for Treatment recommendation business logic
    /// </summary>
    public class TreatmentRecommendationService : ITreatmentRecommendationService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IConditionRepository _conditionRepository;
        private readonly IHerbalMedicineRepository _herbalMedicineRepository;
        private readonly IYogaAsanaRepository _yogaAsanaRepository;
        private readonly ITreatmentRepository _treatmentRepository;
        private readonly ILogger<TreatmentRecommendationService> _logger;

        public TreatmentRecommendationService(
            IPatientRepository patientRepository,
            IConditionRepository conditionRepository,
            IHerbalMedicineRepository herbalMedicineRepository,
            IYogaAsanaRepository yogaAsanaRepository,
            ITreatmentRepository treatmentRepository,
            ILogger<TreatmentRecommendationService> logger)
        {
            _patientRepository = patientRepository;
            _conditionRepository = conditionRepository;
            _herbalMedicineRepository = herbalMedicineRepository;
            _yogaAsanaRepository = yogaAsanaRepository;
            _treatmentRepository = treatmentRepository;
            _logger = logger;
        }

        public async Task<TreatmentRecommendationDto> GenerateRecommendationsAsync(long patientId, long conditionId)
        {
            try
            {
                // Get patient details
                var patient = await _patientRepository.GetByIdAsync(patientId);
                if (patient == null)
                {
                    throw new ArgumentException($"Patient with ID {patientId} not found");
                }

                // Get condition details
                var condition = await _conditionRepository.GetByIdAsync(conditionId);
                if (condition == null)
                {
                    throw new ArgumentException($"Condition with ID {conditionId} not found");
                }

                // Check if patient has Prakriti assessment
                var prakriti = patient.Prakriti ?? "All";
                var hasPrakritiAssessment = !string.IsNullOrEmpty(patient.Prakriti);

                // Get recommended medicines
                var medicines = await _treatmentRepository.GetRecommendedMedicinesAsync(conditionId, prakriti);
                var medicineList = medicines.Select(MapMedicineToDto).ToList();

                // Get recommended yoga asanas
                var yogaAsanas = await _treatmentRepository.GetRecommendedYogaAsanasAsync(conditionId, prakriti);
                var yogaList = yogaAsanas.Select(MapYogaToDto).ToList();

                // Get dietary recommendations
                var dietaryItems = await _treatmentRepository.GetDietaryItemsByPrakritiAsync(prakriti);
                var dietaryList = dietaryItems.Select(MapDietaryToDto).ToList();

                // Generate lifestyle modifications based on condition
                var lifestyleModifications = GenerateLifestyleModifications(condition, prakriti);

                // Calculate confidence score
                var confidenceScore = CalculateConfidenceScore(
                    hasPrakritiAssessment,
                    medicineList.Count,
                    yogaList.Count,
                    patient.Age
                );

                // Generate explanation
                var explanation = GenerateExplanation(
                    patient,
                    condition,
                    medicineList.Count,
                    yogaList.Count,
                    hasPrakritiAssessment
                );

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
                    Explanation = explanation
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating recommendations for patient {PatientId} and condition {ConditionId}",
                    patientId, conditionId);
                throw;
            }
        }

        public async Task<IEnumerable<ConditionDto>> SearchConditionsAsync(string searchTerm, string? category = null)
        {
            try
            {
                var conditions = await _conditionRepository.SearchAsync(searchTerm, category);
                return conditions.Select(MapConditionToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SearchConditionsAsync with term {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<IEnumerable<HerbalMedicineDto>> GetHerbalMedicinesAsync(
            string? searchTerm = null,
            string? prakritiEffect = null)
        {
            try
            {
                IEnumerable<HerbalMedicine> medicines;

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    medicines = await _herbalMedicineRepository.SearchAsync(searchTerm);
                }
                else if (!string.IsNullOrEmpty(prakritiEffect))
                {
                    medicines = await _herbalMedicineRepository.GetByPrakritiEffectAsync(prakritiEffect);
                }
                else
                {
                    medicines = await _herbalMedicineRepository.GetAllAsync();
                }

                return medicines.Select(MapMedicineToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetHerbalMedicinesAsync");
                throw;
            }
        }

        public async Task<IEnumerable<YogaAsanaDto>> GetYogaAsanasAsync(
            string? category = null,
            string? difficulty = null,
            string? prakritiEffect = null)
        {
            try
            {
                IEnumerable<YogaAsana> asanas;

                if (!string.IsNullOrEmpty(category))
                {
                    asanas = await _yogaAsanaRepository.GetByCategoryAsync(category);
                }
                else if (!string.IsNullOrEmpty(difficulty))
                {
                    asanas = await _yogaAsanaRepository.GetByDifficultyAsync(difficulty);
                }
                else if (!string.IsNullOrEmpty(prakritiEffect))
                {
                    asanas = await _yogaAsanaRepository.GetByPrakritiEffectAsync(prakritiEffect);
                }
                else
                {
                    asanas = await _yogaAsanaRepository.GetAllAsync();
                }

                return asanas.Select(MapYogaToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetYogaAsanasAsync");
                throw;
            }
        }

        public async Task<IEnumerable<DietaryItemDto>> GetDietaryItemsAsync(string prakriti, string? category = null)
        {
            try
            {
                var items = await _treatmentRepository.GetDietaryItemsByPrakritiAsync(prakriti, category);
                return items.Select(MapDietaryToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetDietaryItemsAsync for Prakriti {Prakriti}", prakriti);
                throw;
            }
        }

        #region Helper Methods

        private decimal CalculateConfidenceScore(
            bool hasPrakritiAssessment,
            int medicineCount,
            int yogaCount,
            int patientAge)
        {
            decimal score = 60m; // Base score

            // Prakriti assessment completed
            if (hasPrakritiAssessment)
            {
                score += 15m;
            }

            // Multiple treatment options available
            if (medicineCount >= 3 && yogaCount >= 3)
            {
                score += 10m;
            }
            else if (medicineCount >= 2 || yogaCount >= 2)
            {
                score += 5m;
            }

            // Historical success rate (simulated - in production, query from outcomes)
            score += 10m;

            // Patient demographics match (age-appropriate recommendations)
            if (patientAge >= 18 && patientAge <= 70)
            {
                score += 5m;
            }

            return Math.Min(score, 100m);
        }

        private string GenerateExplanation(
            Patient patient,
            Condition condition,
            int medicineCount,
            int yogaCount,
            bool hasPrakritiAssessment)
        {
            var explanation = $"Treatment plan for {condition.Name}";

            if (hasPrakritiAssessment)
            {
                explanation += $" personalized for {patient.Prakriti} Prakriti constitution. ";
            }
            else
            {
                explanation += " with general recommendations. ";
            }

            explanation += $"This plan includes {medicineCount} herbal medicine(s) and {yogaCount} yoga practice(s) ";
            explanation += "selected based on traditional Ayurvedic principles and proven efficacy. ";

            if (hasPrakritiAssessment)
            {
                explanation += $"The recommendations are specifically chosen to balance your {patient.Prakriti} dosha ";
                explanation += "and address the root cause of the condition. ";
            }

            explanation += "Regular practice and adherence to the treatment plan is essential for optimal results. ";
            explanation += "Please consult with an Ayurvedic practitioner before starting any new treatment.";

            return explanation;
        }

        private List<string> GenerateLifestyleModifications(Condition condition, string prakriti)
        {
            var modifications = new List<string>();

            // General recommendations
            modifications.Add("Maintain regular sleep schedule (10 PM - 6 AM)");
            modifications.Add("Practice stress management techniques daily");
            modifications.Add("Stay well hydrated (drink 2-3 liters of water daily)");

            // Condition-specific recommendations
            if (condition.Name.Contains("Kidney") || condition.Name.Contains("Urinary"))
            {
                modifications.Add("Avoid holding urine for long periods");
                modifications.Add("Reduce salt and calcium-rich foods");
                modifications.Add("Drink plenty of water throughout the day");
            }
            else if (condition.Name.Contains("Obesity"))
            {
                modifications.Add("Regular exercise for 45 minutes daily");
                modifications.Add("Avoid daytime sleep");
                modifications.Add("Eat light dinner before 7 PM");
                modifications.Add("Practice portion control");
            }
            else if (condition.Name.Contains("Hypertension"))
            {
                modifications.Add("Reduce salt intake significantly");
                modifications.Add("Practice meditation for 20 minutes daily");
                modifications.Add("Avoid anger and stressful situations");
                modifications.Add("Regular monitoring of blood pressure");
            }
            else if (condition.Name.Contains("Diabetes"))
            {
                modifications.Add("Regular exercise and physical activity");
                modifications.Add("Avoid refined sugars and processed foods");
                modifications.Add("Eat small, frequent meals");
                modifications.Add("Regular blood sugar monitoring");
            }

            // Prakriti-specific recommendations
            if (prakriti.Contains("Vata"))
            {
                modifications.Add("Maintain routine and regularity");
                modifications.Add("Keep warm and avoid cold, windy weather");
            }
            else if (prakriti.Contains("Pitta"))
            {
                modifications.Add("Avoid excessive heat and sun exposure");
                modifications.Add("Practice cooling activities");
            }
            else if (prakriti.Contains("Kapha"))
            {
                modifications.Add("Stay active and avoid sedentary lifestyle");
                modifications.Add("Prefer warm, light foods");
            }

            return modifications;
        }

        #endregion

        #region Mapping Methods

        private static ConditionDto MapConditionToDto(Condition condition)
        {
            return new ConditionDto
            {
                Id = condition.Id,
                Name = condition.Name,
                SanskritName = condition.SanskritName,
                Category = condition.Category,
                Description = condition.Description,
                CommonSymptoms = condition.CommonSymptoms,
                AffectedDoshas = condition.AffectedDoshas,
                Severity = condition.Severity
            };
        }

        private static HerbalMedicineDto MapMedicineToDto(HerbalMedicine medicine)
        {
            return new HerbalMedicineDto
            {
                Id = medicine.Id,
                CommonName = medicine.CommonName,
                SanskritName = medicine.SanskritName,
                ScientificName = medicine.ScientificName,
                HindiName = medicine.HindiName,
                Properties = medicine.Properties,
                Indications = medicine.Indications,
                Dosage = medicine.Dosage,
                Contraindications = medicine.Contraindications,
                SideEffects = medicine.SideEffects,
                ImageUrl = medicine.ImageUrl,
                VataEffect = medicine.VataEffect,
                PittaEffect = medicine.PittaEffect,
                KaphaEffect = medicine.KaphaEffect
            };
        }

        private static YogaAsanaDto MapYogaToDto(YogaAsana asana)
        {
            return new YogaAsanaDto
            {
                Id = asana.Id,
                AsanaName = asana.AsanaName,
                SanskritName = asana.SanskritName,
                Category = asana.Category,
                Benefits = asana.Benefits,
                Duration = asana.Duration,
                Difficulty = asana.Difficulty,
                Instructions = asana.Instructions,
                Precautions = asana.Precautions,
                ImageUrl = asana.ImageUrl,
                VideoUrl = asana.VideoUrl,
                VataEffect = asana.VataEffect,
                PittaEffect = asana.PittaEffect,
                KaphaEffect = asana.KaphaEffect
            };
        }

        private static DietaryItemDto MapDietaryToDto(DietaryItem item)
        {
            return new DietaryItemDto
            {
                Id = item.Id,
                FoodName = item.FoodName,
                Category = item.Category,
                VataEffect = item.VataEffect,
                PittaEffect = item.PittaEffect,
                KaphaEffect = item.KaphaEffect,
                Properties = item.Properties,
                Benefits = item.Benefits,
                Rasa = item.Rasa,
                Virya = item.Virya
            };
        }

        #endregion
    }
}
