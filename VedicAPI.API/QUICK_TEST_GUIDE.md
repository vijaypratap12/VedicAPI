# Quick Test Guide - Treatment Recommendation System API

## Getting Started

### 1. Run the API

```bash
cd C:\Users\vijay.singh\source\repos\vedic_ai\VedicAPI.API\VedicAPI\VedicAPI.API
dotnet run
```

The API will start at `https://localhost:7xxx`

### 2. Open Swagger UI

Navigate to: `https://localhost:7xxx/swagger`

---

## Quick Test Sequence

### Step 1: Create a Patient

**Endpoint:** `POST /api/patients`

**Body:**
```json
{
  "name": "Test Patient",
  "age": 35,
  "gender": "Male",
  "contactNumber": "9876543210",
  "email": "test@example.com"
}
```

**Expected:** Patient created with ID (e.g., ID: 4)

---

### Step 2: Get Prakriti Questions

**Endpoint:** `GET /api/prakriti/questions`

**Expected:** List of 20 questions

---

### Step 3: Submit Prakriti Assessment

**Endpoint:** `POST /api/prakriti/assess`

**Body:**
```json
{
  "patientId": 4,
  "responses": "{\"q1\":\"vata\",\"q2\":\"pitta\",\"q3\":\"vata\"}",
  "vataScore": 45,
  "pittaScore": 35,
  "kaphaScore": 20,
  "dominantPrakriti": "Vata-Pitta"
}
```

**Expected:** Assessment saved successfully

---

### Step 4: Search for Conditions

**Endpoint:** `GET /api/treatment-recommendations/conditions/search?searchTerm=kidney`

**Expected:** List of kidney-related conditions (e.g., Kidney Stones - ID: 1)

---

### Step 5: Generate Treatment Recommendations

**Endpoint:** `POST /api/treatment-recommendations/generate?patientId=4&conditionId=1`

**Expected:** Complete recommendation with:
- Herbal medicines (e.g., Punarnava, Gokshura)
- Yoga asanas (e.g., Pawanmuktasana, Bhujangasana)
- Dietary items (e.g., Cucumber, Watermelon)
- Lifestyle modifications
- Confidence score (e.g., 85.5)
- Detailed explanation

---

### Step 6: Create Treatment Plan

**Endpoint:** `POST /api/treatment-plans`

**Body:**
```json
{
  "patientId": 4,
  "conditionId": 1,
  "prakriti": "Vata-Pitta",
  "herbalMedicines": "[{\"id\":1,\"name\":\"Punarnava\",\"dosage\":\"500mg twice daily\"}]",
  "yogaAsanas": "[{\"id\":1,\"name\":\"Pawanmuktasana\",\"duration\":\"10 minutes\"}]",
  "dietaryRecommendations": "[{\"id\":1,\"name\":\"Cucumber\",\"quantity\":\"200g daily\"}]",
  "lifestyleModifications": "Drink 3L water daily",
  "duration": "3 months",
  "confidenceScore": 85.5,
  "explanation": "Personalized treatment plan for kidney stones"
}
```

**Expected:** Treatment plan created with ID

---

### Step 7: Get Treatment Plans for Patient

**Endpoint:** `GET /api/treatment-plans/patient/4`

**Expected:** List of all treatment plans for the patient

---

### Step 8: Submit Treatment Outcome

**Endpoint:** `POST /api/treatment-plans/1/outcome`

**Body:**
```json
{
  "treatmentPlanId": 1,
  "patientId": 4,
  "effectivenessScore": 8,
  "sideEffects": "None",
  "patientFeedback": "Feeling much better",
  "doctorNotes": "Continue for another month",
  "followUpDate": "2024-03-15T10:00:00Z"
}
```

**Expected:** Outcome saved successfully

---

### Step 9: View Statistics

**Endpoint:** `GET /api/catalog/statistics`

**Expected:** Dashboard statistics including:
- Total patients
- Total treatment plans
- Average confidence score
- Average effectiveness score
- Prakriti distribution
- Top conditions
- Top medicines

---

## Additional Test Endpoints

### Browse Catalog

**Herbal Medicines:**
```
GET /api/catalog/medicines
GET /api/catalog/medicines/1
```

**Yoga Asanas:**
```
GET /api/catalog/yoga
GET /api/catalog/yoga/1
GET /api/catalog/yoga?category=Supine
GET /api/catalog/yoga?difficulty=Beginner
```

**Dietary Items:**
```
GET /api/catalog/dietary?prakriti=Vata-Pitta
GET /api/catalog/dietary?prakriti=Vata&category=Vegetable
```

**Conditions:**
```
GET /api/catalog/conditions
GET /api/catalog/conditions?category=Urinary
```

---

## Sample Data Available

### Patients (3 pre-seeded)
- ID 1: Rajesh Kumar (Vata-Pitta)
- ID 2: Priya Sharma (Pitta-Kapha)
- ID 3: Amit Patel (Kapha)

### Conditions (10 pre-seeded)
1. Kidney Stones (Vrukka Ashmari)
2. Urinary Tract Infection
3. Obesity (Sthaulya)
4. Hypertension (Rakta Gata Vata)
5. Diabetes Mellitus Type 2
6. Chronic Kidney Disease
7. Metabolic Syndrome
8. Gout (Vatarakta)
9. Polycystic Kidney Disease
10. Nephrotic Syndrome

### Herbal Medicines (35 pre-seeded)
- Punarnava, Gokshura, Varuna, Pashanbheda, Shilajit, etc.

### Yoga Asanas (25 pre-seeded)
- Pawanmuktasana, Bhujangasana, Dhanurasana, Ustrasana, etc.

### Dietary Items (65+ pre-seeded)
- Vegetables, Fruits, Grains, Legumes, Dairy, Spices, Beverages

---

## Expected Response Format

### Success Response
```json
{
  "success": true,
  "message": "Operation successful",
  "data": { /* response data */ },
  "errors": null
}
```

### Error Response
```json
{
  "success": false,
  "message": "Error message",
  "data": null,
  "errors": ["Error detail 1", "Error detail 2"]
}
```

---

## Troubleshooting

### Issue: Cannot connect to database
**Solution:** Verify connection string in `appsettings.json`

### Issue: Stored procedure not found
**Solution:** Run the database scripts in order:
1. `CreateTreatmentRecommendationTables.sql`
2. `SeedTreatmentData.sql`
3. `CreateTreatmentStoredProcedures.sql`

### Issue: 500 Internal Server Error
**Solution:** Check the console logs for detailed error messages

### Issue: CORS error
**Solution:** Verify CORS is enabled in `Program.cs` (already configured)

---

## Performance Testing

### Test Concurrent Requests
Use tools like:
- Postman Collection Runner
- Apache JMeter
- k6

### Monitor Performance
- Check response times in Swagger
- Monitor database query execution
- Check memory usage

---

## Next Steps After Testing

1. ✅ Verify all endpoints work correctly
2. ✅ Test error scenarios (invalid data, missing IDs)
3. ✅ Check confidence score calculations
4. ✅ Verify Prakriti-based recommendations
5. ✅ Test treatment plan workflow end-to-end
6. Frontend integration
7. Add authentication to endpoints
8. Deploy to staging environment

---

## Support Files

- **API_ENDPOINTS_GUIDE.md** - Detailed API documentation
- **IMPLEMENTATION_SUMMARY.md** - Complete implementation details
- **TREATMENT_SYSTEM_README.md** - Database documentation

---

## Quick Commands

```bash
# Build the project
dotnet build

# Run the project
dotnet run

# Run with watch (auto-reload)
dotnet watch run

# Clean and rebuild
dotnet clean && dotnet build

# Check for errors
dotnet build --no-incremental
```

---

## Success Checklist

- [ ] API starts without errors
- [ ] Swagger UI loads successfully
- [ ] Can create a patient
- [ ] Can complete Prakriti assessment
- [ ] Can search conditions
- [ ] Can generate recommendations
- [ ] Can create treatment plan
- [ ] Can submit outcome
- [ ] Can view statistics
- [ ] All endpoints return proper responses
- [ ] Error handling works correctly

---

## Demo Scenario

**Scenario:** New patient with kidney stones

1. Create patient "John Doe" (age 35, male)
2. Complete Prakriti assessment → Result: Vata-Pitta
3. Search for "kidney" → Find "Kidney Stones"
4. Generate recommendations → Get personalized plan
5. Create treatment plan with recommendations
6. After 2 months, submit positive outcome
7. View updated statistics

**Expected Time:** 5-10 minutes

---

## Contact

For issues or questions, refer to the implementation documentation or check the console logs for detailed error messages.

**Happy Testing! 🎉**
