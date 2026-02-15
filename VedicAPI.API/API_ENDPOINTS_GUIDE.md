# Treatment Recommendation System API Endpoints Guide

## Overview

This guide provides a comprehensive overview of all API endpoints for the Treatment Recommendation System. All endpoints are accessible via Swagger UI at `/swagger` when the API is running.

## Base URL

```
https://localhost:7xxx/api
```

---

## 1. Patients API (`/api/patients`)

### Create Patient
**POST** `/api/patients`

**Request Body:**
```json
{
  "userId": 1,
  "name": "John Doe",
  "age": 35,
  "gender": "Male",
  "contactNumber": "9876543210",
  "email": "john@example.com",
  "address": "123 Main Street, City"
}
```

**Response:** `201 Created`
```json
{
  "success": true,
  "message": "Patient created successfully",
  "data": {
    "id": 1,
    "userId": 1,
    "name": "John Doe",
    "age": 35,
    "gender": "Male",
    "contactNumber": "9876543210",
    "email": "john@example.com",
    "address": "123 Main Street, City",
    "prakriti": null,
    "prakritiScore": null,
    "createdAt": "2024-02-15T10:30:00Z",
    "updatedAt": null
  }
}
```

### Get Patient by ID
**GET** `/api/patients/{id}`

### Get Patient by User ID
**GET** `/api/patients/user/{userId}`

### Get All Patients
**GET** `/api/patients`

### Update Patient
**PUT** `/api/patients/{id}`

### Delete Patient
**DELETE** `/api/patients/{id}`

---

## 2. Prakriti Assessment API (`/api/prakriti`)

### Get Assessment Questions
**GET** `/api/prakriti/questions`

**Response:** `200 OK`
```json
{
  "success": true,
  "message": "Prakriti questions retrieved successfully",
  "data": [
    {
      "id": 1,
      "category": "Physical",
      "question": "What is your body frame?",
      "vataOption": "Thin, light frame",
      "pittaOption": "Medium build",
      "kaphaOption": "Large, heavy frame",
      "displayOrder": 1
    }
  ]
}
```

### Submit Assessment
**POST** `/api/prakriti/assess`

**Request Body:**
```json
{
  "patientId": 1,
  "responses": "{\"q1\":\"vata\",\"q2\":\"pitta\"}",
  "vataScore": 45,
  "pittaScore": 35,
  "kaphaScore": 20,
  "dominantPrakriti": "Vata-Pitta"
}
```

**Response:** `201 Created`

### Get Assessment by ID
**GET** `/api/prakriti/assessment/{id}`

### Get Latest Assessment for Patient
**GET** `/api/prakriti/patient/{patientId}/latest`

---

## 3. Treatment Recommendations API (`/api/treatment-recommendations`)

### Generate Treatment Recommendations
**POST** `/api/treatment-recommendations/generate?patientId={patientId}&conditionId={conditionId}`

**Response:** `200 OK`
```json
{
  "success": true,
  "message": "Treatment recommendations generated successfully",
  "data": {
    "patientId": 1,
    "patientName": "John Doe",
    "prakriti": "Vata-Pitta",
    "conditionId": 1,
    "conditionName": "Kidney Stones",
    "recommendedMedicines": [
      {
        "id": 1,
        "commonName": "Punarnava",
        "sanskritName": "Punarnava",
        "scientificName": "Boerhavia diffusa",
        "properties": "Diuretic, Anti-inflammatory",
        "dosage": "500mg twice daily",
        "vataEffect": "Balancing",
        "pittaEffect": "Cooling",
        "kaphaEffect": "Reducing"
      }
    ],
    "recommendedYogaAsanas": [
      {
        "id": 1,
        "asanaName": "Pawanmuktasana",
        "sanskritName": "Pawanmuktasana",
        "category": "Supine",
        "benefits": "Improves digestion, relieves gas",
        "duration": "5-10 minutes",
        "difficulty": "Beginner"
      }
    ],
    "recommendedDietaryItems": [
      {
        "id": 1,
        "foodName": "Cucumber",
        "category": "Vegetable",
        "vataEffect": "Neutral",
        "pittaEffect": "Cooling",
        "kaphaEffect": "Neutral"
      }
    ],
    "lifestyleModifications": [
      "Maintain regular sleep schedule (10 PM - 6 AM)",
      "Drink plenty of water throughout the day",
      "Avoid holding urine for long periods"
    ],
    "confidenceScore": 85.5,
    "explanation": "Treatment plan for Kidney Stones personalized for Vata-Pitta Prakriti constitution..."
  }
}
```

### Search Conditions
**GET** `/api/treatment-recommendations/conditions/search?searchTerm={term}&category={category}`

### Get Herbal Medicines
**GET** `/api/treatment-recommendations/medicines?searchTerm={term}&prakritiEffect={prakriti}`

### Get Yoga Asanas
**GET** `/api/treatment-recommendations/yoga?category={category}&difficulty={difficulty}&prakritiEffect={prakriti}`

### Get Dietary Items
**GET** `/api/treatment-recommendations/dietary?prakriti={prakriti}&category={category}`

---

## 4. Treatment Plans API (`/api/treatment-plans`)

### Create Treatment Plan
**POST** `/api/treatment-plans`

**Request Body:**
```json
{
  "patientId": 1,
  "conditionId": 1,
  "prakriti": "Vata-Pitta",
  "herbalMedicines": "[{\"id\":1,\"name\":\"Punarnava\",\"dosage\":\"500mg twice daily\"}]",
  "yogaAsanas": "[{\"id\":1,\"name\":\"Pawanmuktasana\",\"duration\":\"10 minutes\"}]",
  "dietaryRecommendations": "[{\"id\":1,\"name\":\"Cucumber\",\"quantity\":\"200g daily\"}]",
  "lifestyleModifications": "Drink 3L water daily, Avoid salt",
  "duration": "3 months",
  "confidenceScore": 85.5,
  "explanation": "Personalized treatment plan...",
  "createdBy": 1
}
```

**Response:** `201 Created`

### Get Treatment Plan by ID
**GET** `/api/treatment-plans/{id}`

### Get Treatment Plans by Patient ID
**GET** `/api/treatment-plans/patient/{patientId}`

### Update Treatment Plan
**PUT** `/api/treatment-plans/{id}`

### Submit Treatment Outcome
**POST** `/api/treatment-plans/{id}/outcome`

**Request Body:**
```json
{
  "treatmentPlanId": 1,
  "patientId": 1,
  "effectivenessScore": 8,
  "sideEffects": "None",
  "patientFeedback": "Feeling much better after 2 months",
  "doctorNotes": "Continue for another month",
  "followUpDate": "2024-03-15T10:00:00Z",
  "recordedBy": 1
}
```

---

## 5. Catalog API (`/api/catalog`)

### Get All Medicines
**GET** `/api/catalog/medicines?searchTerm={term}`

### Get Medicine by ID
**GET** `/api/catalog/medicines/{id}`

### Get All Yoga Asanas
**GET** `/api/catalog/yoga?category={category}&difficulty={difficulty}`

### Get Yoga Asana by ID
**GET** `/api/catalog/yoga/{id}`

### Get Dietary Items
**GET** `/api/catalog/dietary?prakriti={prakriti}&category={category}`

### Get All Conditions
**GET** `/api/catalog/conditions?searchTerm={term}&category={category}`

### Get Treatment Statistics
**GET** `/api/catalog/statistics`

**Response:** `200 OK`
```json
{
  "success": true,
  "message": "Statistics retrieved successfully",
  "data": {
    "totalPatients": 150,
    "totalTreatmentPlans": 320,
    "totalConditions": 10,
    "totalHerbalMedicines": 35,
    "totalYogaAsanas": 25,
    "averageConfidenceScore": 82.5,
    "averageEffectivenessScore": 7.8,
    "prakritiDistribution": {
      "Vata": 45,
      "Pitta": 38,
      "Kapha": 32,
      "Vata-Pitta": 20,
      "Pitta-Kapha": 15
    },
    "topConditions": [
      {
        "conditionName": "Kidney Stones",
        "treatmentCount": 85
      }
    ],
    "topMedicines": [
      {
        "commonName": "Punarnava",
        "sanskritName": "Punarnava",
        "prescriptionCount": 120
      }
    ]
  }
}
```

---

## Testing Workflow

### 1. Create a Patient
```bash
POST /api/patients
```

### 2. Complete Prakriti Assessment
```bash
GET /api/prakriti/questions
POST /api/prakriti/assess
```

### 3. Search for Conditions
```bash
GET /api/treatment-recommendations/conditions/search?searchTerm=kidney
```

### 4. Generate Treatment Recommendations
```bash
POST /api/treatment-recommendations/generate?patientId=1&conditionId=1
```

### 5. Create Treatment Plan
```bash
POST /api/treatment-plans
```

### 6. Submit Treatment Outcome
```bash
POST /api/treatment-plans/1/outcome
```

### 7. View Statistics
```bash
GET /api/catalog/statistics
```

---

## Error Responses

All endpoints return consistent error responses:

```json
{
  "success": false,
  "message": "Error message",
  "data": null,
  "errors": [
    "Detailed error 1",
    "Detailed error 2"
  ]
}
```

**Common Status Codes:**
- `200 OK` - Success
- `201 Created` - Resource created successfully
- `400 Bad Request` - Invalid input data
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server error

---

## Notes

1. All timestamps are in UTC format
2. JSON fields for complex data (HerbalMedicines, YogaAsanas, etc.) are stored as JSON strings
3. Prakriti values: `Vata`, `Pitta`, `Kapha`, `Vata-Pitta`, `Pitta-Kapha`, `Vata-Kapha`, `Tridosha`
4. All IDs use `BIGINT` (long) type
5. Soft delete is used for patients (IsActive flag)

---

## Swagger UI

Access the interactive API documentation at:
```
https://localhost:7xxx/swagger
```

The Swagger UI provides:
- Complete API documentation
- Interactive testing interface
- Request/response schemas
- Authentication testing
- Example requests and responses
