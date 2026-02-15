-- =============================================
-- Vedic API Database - Treatment Recommendation System Seed Data
-- Description: Comprehensive seed data for all treatment tables
-- Date: 2026-02-15
-- =============================================

USE VedicDB;
GO

PRINT '========================================';
PRINT 'Seeding Treatment Recommendation System Data';
PRINT '========================================';
PRINT '';

-- =============================================
-- Seed Data 1: PRAKRITIQUESTIONS
-- Description: 20 Prakriti assessment questions
-- =============================================
PRINT 'Step 1/7: Seeding PRAKRITIQUESTIONS...';

IF NOT EXISTS (SELECT * FROM PRAKRITIQUESTIONS)
BEGIN
    INSERT INTO PRAKRITIQUESTIONS (Category, Question, VataOption, PittaOption, KaphaOption, DisplayOrder, IsActive)
    VALUES 
    -- Physical Characteristics
    (N'Physical', N'What is your body frame and build?', 
     N'Thin, light frame with difficulty gaining weight', 
     N'Medium build, muscular and well-proportioned', 
     N'Large, heavy frame with tendency to gain weight easily', 1, 1),
    
    (N'Physical', N'What is your skin type?', 
     N'Dry, rough, cool to touch, prone to cracking', 
     N'Warm, oily, soft, prone to rashes and inflammation', 
     N'Thick, oily, smooth, cool and pale', 2, 1),
    
    (N'Physical', N'How is your hair?', 
     N'Dry, brittle, thin with split ends', 
     N'Fine, oily, prone to early graying or balding', 
     N'Thick, oily, wavy, lustrous and strong', 3, 1),
    
    (N'Physical', N'What is your body temperature preference?', 
     N'Prefer warmth, dislike cold and wind', 
     N'Prefer cool, dislike heat and sun', 
     N'Adaptable, but prefer warm and dry weather', 4, 1),
    
    (N'Physical', N'How is your physical stamina?', 
     N'Low endurance, tire easily, need frequent rest', 
     N'Moderate stamina, good endurance with proper training', 
     N'High stamina, steady energy, excellent endurance', 5, 1),
    
    -- Digestive Characteristics
    (N'Digestive', N'How is your appetite?', 
     N'Irregular and variable, sometimes hungry, sometimes not', 
     N'Strong and sharp, cannot skip meals, get irritable when hungry', 
     N'Steady and moderate, can skip meals easily without discomfort', 6, 1),
    
    (N'Digestive', N'What is your digestion like?', 
     N'Irregular, prone to gas, bloating and constipation', 
     N'Strong and fast, prone to acidity and loose stools', 
     N'Slow and steady, prone to heaviness and sluggishness', 7, 1),
    
    (N'Digestive', N'How much water do you drink?', 
     N'Variable intake, often forget to drink water', 
     N'Drink frequently, always thirsty', 
     N'Moderate intake, rarely feel thirsty', 8, 1),
    
    (N'Digestive', N'What foods do you prefer?', 
     N'Warm, cooked, moist foods with sweet, sour, salty tastes', 
     N'Cool, raw foods with sweet, bitter, astringent tastes', 
     N'Warm, light, dry foods with pungent, bitter, astringent tastes', 9, 1),
    
    -- Mental Characteristics
    (N'Mental', N'How is your memory?', 
     N'Quick to learn but quick to forget, short-term memory good', 
     N'Sharp, clear and focused memory, good retention', 
     N'Slow to learn but excellent long-term retention', 10, 1),
    
    (N'Mental', N'How do you handle stress?', 
     N'Become anxious, worried, fearful and restless', 
     N'Become angry, irritable, critical and aggressive', 
     N'Remain calm, withdrawn, may become depressed', 11, 1),
    
    (N'Mental', N'What is your speech pattern?', 
     N'Fast, talkative, may ramble or change topics frequently', 
     N'Sharp, articulate, precise, may be argumentative', 
     N'Slow, steady, thoughtful, speak only when necessary', 12, 1),
    
    (N'Mental', N'How is your decision-making?', 
     N'Quick decisions but often change mind, indecisive', 
     N'Decisive, confident, analytical and logical', 
     N'Slow and deliberate, stick to decisions once made', 13, 1),
    
    -- Sleep Patterns
    (N'Sleep', N'How is your sleep pattern?', 
     N'Light sleeper, interrupted sleep, difficulty falling asleep', 
     N'Moderate, sound sleep, wake up refreshed', 
     N'Deep, prolonged sleep, difficulty waking up', 14, 1),
    
    (N'Sleep', N'How many hours do you sleep?', 
     N'5-6 hours, often wake up during night', 
     N'6-8 hours, regular sleep schedule', 
     N'8-10 hours or more, love sleeping', 15, 1),
    
    -- Emotional Characteristics
    (N'Emotional', N'What is your emotional nature?', 
     N'Enthusiastic, creative, changeable, anxious', 
     N'Passionate, ambitious, focused, intense', 
     N'Calm, steady, loving, attached', 16, 1),
    
    (N'Emotional', N'How do you express emotions?', 
     N'Quickly and openly, emotions change rapidly', 
     N'Intensely and directly, may be confrontational', 
     N'Slowly and steadily, suppress emotions', 17, 1),
    
    -- Activity Patterns
    (N'Activity', N'What is your activity level?', 
     N'Very active, restless, always on the move', 
     N'Moderately active, focused and goal-oriented', 
     N'Slow and steady, prefer sedentary activities', 18, 1),
    
    (N'Activity', N'How do you spend free time?', 
     N'Creative activities, travel, socializing, variety', 
     N'Competitive sports, intellectual pursuits, leadership', 
     N'Relaxing, reading, watching TV, eating', 19, 1),
    
    -- Financial Habits
    (N'Lifestyle', N'What are your spending habits?', 
     N'Impulsive buyer, spend on small things frequently', 
     N'Spend on quality and luxury items, calculated purchases', 
     N'Save money, reluctant to spend, accumulate wealth', 20, 1);
    
    PRINT '  ✓ Inserted 20 Prakriti assessment questions';
END
ELSE
BEGIN
    PRINT '  ⚠ PRAKRITIQUESTIONS already contains data';
END
GO

-- =============================================
-- Seed Data 2: CONDITIONS
-- Description: Common AYUSH conditions
-- =============================================
PRINT 'Step 2/7: Seeding CONDITIONS...';

IF NOT EXISTS (SELECT * FROM CONDITIONS)
BEGIN
    INSERT INTO CONDITIONS (Name, SanskritName, Category, Description, CommonSymptoms, AffectedDoshas, Severity)
    VALUES 
    (N'Kidney Stones', N'Vrukka Ashmari', N'Chronic', 
     N'Formation of hard crystalline deposits in the kidneys due to accumulation of minerals and salts. In Ayurveda, it is caused by aggravated Vata and Kapha doshas leading to crystallization.',
     N'Severe pain in back or side, blood in urine, nausea, vomiting, frequent urination, burning sensation while urinating',
     N'Vata-Kapha', N'Moderate'),
    
    (N'Urinary Stones', N'Mutrashmari', N'Chronic', 
     N'Stones formed in the urinary tract including bladder and urethra. Results from vitiation of Apana Vata and accumulation of Kapha.',
     N'Painful urination, frequent urination, lower abdominal pain, blood in urine, difficulty passing urine',
     N'Vata-Kapha', N'Moderate'),
    
    (N'Obesity', N'Sthaulya/Medoroga', N'Lifestyle', 
     N'Excessive accumulation of body fat due to imbalance in metabolism. Primarily caused by aggravated Kapha dosha and weakened Agni (digestive fire).',
     N'Excess body weight, fatigue, breathlessness, excessive sweating, joint pain, snoring, difficulty in movement',
     N'Kapha', N'Moderate'),
    
    (N'Hypertension', N'Rakta Gata Vata', N'Lifestyle', 
     N'Elevated blood pressure caused by vitiation of Vata dosha affecting blood circulation. Modern lifestyle and stress are major contributing factors.',
     N'Headache, dizziness, chest pain, fatigue, irregular heartbeat, vision problems, shortness of breath',
     N'Vata-Pitta', N'Severe'),
    
    (N'Type 2 Diabetes', N'Madhumeha', N'Lifestyle', 
     N'Metabolic disorder characterized by high blood sugar levels. Caused by aggravated Kapha dosha and weakened Agni affecting glucose metabolism.',
     N'Frequent urination, excessive thirst, increased hunger, fatigue, blurred vision, slow healing wounds, tingling in hands/feet',
     N'Kapha-Pitta', N'Severe'),
    
    (N'Arthritis', N'Sandhivata/Amavata', N'Chronic', 
     N'Inflammation and degeneration of joints caused by vitiated Vata dosha and accumulation of Ama (toxins) in the joints.',
     N'Joint pain, stiffness, swelling, reduced range of motion, morning stiffness, weakness',
     N'Vata', N'Moderate'),
    
    (N'Acidity', N'Amlapitta', N'Acute', 
     N'Excess acid production in stomach due to aggravated Pitta dosha. Common in modern stressful lifestyle and irregular eating habits.',
     N'Heartburn, sour belching, nausea, burning sensation in chest, loss of appetite, bad breath',
     N'Pitta', N'Mild'),
    
    (N'Constipation', N'Vibandha', N'Acute', 
     N'Difficulty in bowel movements caused by aggravated Vata dosha affecting Apana Vayu. Results from poor diet and lifestyle.',
     N'Infrequent bowel movements, hard stools, straining, abdominal pain, bloating, feeling of incomplete evacuation',
     N'Vata', N'Mild'),
    
    (N'Anxiety', N'Chittodvega', N'Mental', 
     N'Mental disorder characterized by excessive worry and fear. Caused by vitiated Vata dosha affecting the mind (Manas).',
     N'Excessive worry, restlessness, difficulty concentrating, sleep disturbances, rapid heartbeat, sweating',
     N'Vata', N'Moderate'),
    
    (N'Insomnia', N'Anidra', N'Mental', 
     N'Inability to fall asleep or maintain sleep due to aggravated Vata and Pitta doshas affecting the nervous system.',
     N'Difficulty falling asleep, frequent waking, early morning awakening, daytime fatigue, irritability',
     N'Vata-Pitta', N'Mild');
    
    PRINT '  ✓ Inserted 10 common conditions';
END
ELSE
BEGIN
    PRINT '  ⚠ CONDITIONS already contains data';
END
GO

-- =============================================
-- Seed Data 3: HERBALMEDICINES
-- Description: 35 Essential herbal medicines
-- =============================================
PRINT 'Step 3/7: Seeding HERBALMEDICINES...';

IF NOT EXISTS (SELECT * FROM HERBALMEDICINES)
BEGIN
    INSERT INTO HERBALMEDICINES (CommonName, SanskritName, ScientificName, HindiName, Properties, Indications, Dosage, Contraindications, VataEffect, PittaEffect, KaphaEffect)
    VALUES 
    -- For Kidney/Urinary Stones
    (N'Gokshura', N'Gokshura', N'Tribulus terrestris', N'Gokhru',
     N'Rasa: Madhura (Sweet), Guna: Guru, Snigdha, Virya: Sheeta (Cold), Vipaka: Madhura',
     N'Kidney stones, urinary disorders, hypertension, reproductive health, general weakness',
     N'3-6g powder twice daily with water or milk',
     N'Not recommended during pregnancy',
     N'Balancing', N'Balancing', N'Neutral'),
    
    (N'Punarnava', N'Punarnava', N'Boerhavia diffusa', N'Punarnava',
     N'Rasa: Tikta, Kashaya, Madhura, Guna: Laghu, Ruksha, Virya: Ushna, Vipaka: Madhura',
     N'Kidney disorders, edema, obesity, liver problems, rejuvenation',
     N'5-10ml juice or 3-6g powder daily',
     N'Use cautiously in pregnancy',
     N'Balancing', N'Neutral', N'Balancing'),
    
    (N'Varuna', N'Varuna', N'Crataeva nurvala', N'Varun',
     N'Rasa: Tikta, Kashaya, Guna: Laghu, Ruksha, Virya: Ushna, Vipaka: Katu',
     N'Kidney stones, urinary calculi, prostate disorders, bladder problems',
     N'500mg-1g twice daily',
     N'None known',
     N'Neutral', N'Neutral', N'Balancing'),
    
    (N'Pashanbheda', N'Pashanbheda', N'Bergenia ligulata', N'Pashanbhed',
     N'Rasa: Tikta, Kashaya, Guna: Laghu, Ruksha, Virya: Sheeta, Vipaka: Katu',
     N'Kidney stones (stone breaker), urinary disorders, diabetes',
     N'3-6g powder twice daily',
     N'Not for long-term use without supervision',
     N'Neutral', N'Balancing', N'Balancing'),
    
    -- For Obesity
    (N'Guggulu', N'Guggulu', N'Commiphora mukul', N'Guggul',
     N'Rasa: Tikta, Katu, Kashaya, Guna: Laghu, Ruksha, Virya: Ushna, Vipaka: Katu',
     N'Obesity, high cholesterol, arthritis, skin diseases',
     N'500mg-1g twice daily',
     N'Pregnancy, bleeding disorders',
     N'Balancing', N'Aggravating', N'Balancing'),
    
    (N'Triphala', N'Triphala', N'Three fruits combination', N'Triphala',
     N'Rasa: All six tastes, Guna: Laghu, Ruksha, Virya: Ushna, Vipaka: Madhura',
     N'Obesity, digestive disorders, detoxification, eye health, rejuvenation',
     N'3-6g at bedtime with warm water',
     N'Pregnancy, diarrhea, severe weakness',
     N'Balancing', N'Balancing', N'Balancing'),
    
    (N'Vrikshamla', N'Vrikshamla', N'Garcinia cambogia', N'Garcinia',
     N'Rasa: Amla (Sour), Guna: Laghu, Virya: Ushna, Vipaka: Amla',
     N'Obesity, weight management, appetite control, fat metabolism',
     N'500mg twice daily before meals',
     N'Pregnancy, lactation, diabetes patients should consult doctor',
     N'Neutral', N'Aggravating', N'Balancing'),
    
    (N'Shilajit', N'Shilajit', N'Asphaltum punjabianum', N'Shilajeet',
     N'Rasa: Tikta, Katu, Kashaya, Guna: Laghu, Virya: Ushna, Vipaka: Katu',
     N'Obesity, diabetes, urinary disorders, rejuvenation, strength',
     N'300-500mg twice daily with milk or water',
     N'High Pitta conditions, pregnancy',
     N'Balancing', N'Aggravating', N'Balancing'),
    
    -- For Hypertension
    (N'Arjuna', N'Arjuna', N'Terminalia arjuna', N'Arjun',
     N'Rasa: Kashaya, Guna: Laghu, Ruksha, Virya: Sheeta, Vipaka: Katu',
     N'Hypertension, heart disease, high cholesterol, cardiac weakness',
     N'500mg-1g twice daily with milk or water',
     N'None known',
     N'Balancing', N'Balancing', N'Neutral'),
    
    (N'Sarpagandha', N'Sarpagandha', N'Rauwolfia serpentina', N'Sarpagandha',
     N'Rasa: Tikta, Katu, Guna: Laghu, Virya: Ushna, Vipaka: Katu',
     N'Hypertension, insomnia, anxiety, mental disorders',
     N'250-500mg twice daily',
     N'Pregnancy, depression, peptic ulcer',
     N'Balancing', N'Neutral', N'Neutral'),
    
    (N'Jatamansi', N'Jatamansi', N'Nardostachys jatamansi', N'Jatamansi',
     N'Rasa: Tikta, Kashaya, Madhura, Guna: Laghu, Snigdha, Virya: Sheeta, Vipaka: Katu',
     N'Hypertension, anxiety, insomnia, memory enhancement',
     N'1-3g powder twice daily',
     N'None known',
     N'Balancing', N'Balancing', N'Neutral'),
    
    (N'Ashwagandha', N'Ashwagandha', N'Withania somnifera', N'Ashwagandha',
     N'Rasa: Tikta, Kashaya, Madhura, Guna: Laghu, Snigdha, Virya: Ushna, Vipaka: Madhura',
     N'Stress, anxiety, weakness, immunity, rejuvenation',
     N'3-6g powder twice daily with milk',
     N'Pregnancy (high doses), hyperthyroidism',
     N'Balancing', N'Neutral', N'Aggravating'),
    
    -- For Diabetes
    (N'Gudmar', N'Meshashringi', N'Gymnema sylvestre', N'Gudmar',
     N'Rasa: Tikta, Kashaya, Guna: Laghu, Ruksha, Virya: Ushna, Vipaka: Katu',
     N'Diabetes, obesity, sugar cravings, metabolic disorders',
     N'2-4g powder twice daily',
     N'Hypoglycemia, pregnancy',
     N'Neutral', N'Neutral', N'Balancing'),
    
    (N'Jamun', N'Jambu', N'Syzygium cumini', N'Jamun',
     N'Rasa: Kashaya, Madhura, Amla, Guna: Laghu, Ruksha, Virya: Sheeta, Vipaka: Katu',
     N'Diabetes, diarrhea, digestive disorders',
     N'3-6g seed powder twice daily',
     N'None known',
     N'Neutral', N'Balancing', N'Balancing'),
    
    (N'Karela', N'Karavellaka', N'Momordica charantia', N'Karela',
     N'Rasa: Tikta, Guna: Laghu, Ruksha, Virya: Ushna, Vipaka: Katu',
     N'Diabetes, skin diseases, liver disorders, blood purification',
     N'10-20ml juice daily or 3-6g powder',
     N'Pregnancy, hypoglycemia',
     N'Neutral', N'Balancing', N'Balancing'),
    
    (N'Methi', N'Methika', N'Trigonella foenum-graecum', N'Methi',
     N'Rasa: Katu, Tikta, Guna: Laghu, Snigdha, Virya: Ushna, Vipaka: Katu',
     N'Diabetes, high cholesterol, digestive disorders, lactation',
     N'5-10g seeds soaked overnight or powder',
     N'Pregnancy (high doses)',
     N'Balancing', N'Aggravating', N'Balancing'),
    
    -- For Arthritis
    (N'Shallaki', N'Shallaki', N'Boswellia serrata', N'Salai Guggul',
     N'Rasa: Tikta, Katu, Kashaya, Guna: Laghu, Ruksha, Virya: Sheeta, Vipaka: Katu',
     N'Arthritis, joint pain, inflammation, asthma',
     N'500mg-1g twice daily',
     N'Pregnancy, lactation',
     N'Balancing', N'Balancing', N'Neutral'),
    
    (N'Nirgundi', N'Nirgundi', N'Vitex negundo', N'Nirgundi',
     N'Rasa: Tikta, Katu, Guna: Laghu, Ruksha, Virya: Ushna, Vipaka: Katu',
     N'Arthritis, joint pain, inflammation, wounds',
     N'3-6g powder or oil for external application',
     N'None known',
     N'Balancing', N'Neutral', N'Balancing'),
    
    (N'Rasna', N'Rasna', N'Pluchea lanceolata', N'Rasna',
     N'Rasa: Tikta, Katu, Guna: Laghu, Ruksha, Virya: Ushna, Vipaka: Katu',
     N'Arthritis, joint pain, sciatica, rheumatism',
     N'3-6g powder twice daily',
     N'High Pitta conditions',
     N'Balancing', N'Aggravating', N'Balancing'),
    
    -- For Digestive Issues
    (N'Haritaki', N'Haritaki', N'Terminalia chebula', N'Harad',
     N'Rasa: All except Lavana, Guna: Laghu, Ruksha, Virya: Ushna, Vipaka: Madhura',
     N'Constipation, digestive disorders, detoxification, rejuvenation',
     N'3-6g powder at bedtime',
     N'Pregnancy, severe weakness, dehydration',
     N'Balancing', N'Neutral', N'Balancing'),
    
    (N'Amalaki', N'Amalaki', N'Emblica officinalis', N'Amla',
     N'Rasa: All except Lavana, Guna: Laghu, Ruksha, Virya: Sheeta, Vipaka: Madhura',
     N'Acidity, digestive disorders, immunity, rejuvenation, eye health',
     N'3-6g powder twice daily',
     N'None known',
     N'Balancing', N'Balancing', N'Neutral'),
    
    (N'Ajwain', N'Yavani', N'Trachyspermum ammi', N'Ajwain',
     N'Rasa: Katu, Tikta, Guna: Laghu, Ruksha, Virya: Ushna, Vipaka: Katu',
     N'Indigestion, gas, bloating, colic pain, respiratory disorders',
     N'1-3g powder with warm water after meals',
     N'High Pitta, acidity, pregnancy (high doses)',
     N'Balancing', N'Aggravating', N'Balancing'),
    
    (N'Jeera', N'Jiraka', N'Cuminum cyminum', N'Jeera',
     N'Rasa: Katu, Tikta, Guna: Laghu, Ruksha, Virya: Sheeta, Vipaka: Katu',
     N'Digestive disorders, loss of appetite, gas, diarrhea',
     N'3-6g powder or as spice in food',
     N'None known',
     N'Balancing', N'Balancing', N'Balancing'),
    
    (N'Saunf', N'Mishreya', N'Foeniculum vulgare', N'Saunf',
     N'Rasa: Madhura, Tikta, Guna: Laghu, Snigdha, Virya: Sheeta, Vipaka: Madhura',
     N'Digestive disorders, gas, bloating, lactation, eye health',
     N'3-6g powder or chew seeds after meals',
     N'None known',
     N'Balancing', N'Balancing', N'Neutral'),
    
    -- For Mental Health
    (N'Brahmi', N'Brahmi', N'Bacopa monnieri', N'Brahmi',
     N'Rasa: Tikta, Kashaya, Madhura, Guna: Laghu, Virya: Sheeta, Vipaka: Madhura',
     N'Memory enhancement, anxiety, insomnia, mental clarity, epilepsy',
     N'3-6g powder twice daily with ghee or milk',
     N'None known',
     N'Balancing', N'Balancing', N'Neutral'),
    
    (N'Shankhpushpi', N'Shankhpushpi', N'Convolvulus pluricaulis', N'Shankhpushpi',
     N'Rasa: Tikta, Kashaya, Madhura, Guna: Laghu, Virya: Sheeta, Vipaka: Madhura',
     N'Memory, anxiety, insomnia, hypertension, mental disorders',
     N'3-6g powder twice daily',
     N'None known',
     N'Balancing', N'Balancing', N'Neutral'),
    
    (N'Vacha', N'Vacha', N'Acorus calamus', N'Bach',
     N'Rasa: Katu, Tikta, Guna: Laghu, Tikshna, Virya: Ushna, Vipaka: Katu',
     N'Memory, speech disorders, epilepsy, mental clarity, cough',
     N'500mg-1g powder twice daily',
     N'Pregnancy, bleeding disorders, high Pitta',
     N'Balancing', N'Aggravating', N'Balancing'),
    
    -- General Health
    (N'Tulsi', N'Tulsi', N'Ocimum sanctum', N'Tulsi',
     N'Rasa: Katu, Tikta, Guna: Laghu, Ruksha, Virya: Ushna, Vipaka: Katu',
     N'Immunity, respiratory disorders, fever, stress, skin diseases',
     N'5-10 leaves daily or 3-6g powder',
     N'Pregnancy (high doses)',
     N'Balancing', N'Neutral', N'Balancing'),
    
    (N'Neem', N'Nimba', N'Azadirachta indica', N'Neem',
     N'Rasa: Tikta, Kashaya, Guna: Laghu, Ruksha, Virya: Sheeta, Vipaka: Katu',
     N'Skin diseases, blood purification, diabetes, fever, wounds',
     N'2-4g powder or 10-20ml juice',
     N'Pregnancy, lactation, children, weakness',
     N'Aggravating', N'Balancing', N'Balancing'),
    
    (N'Haldi', N'Haridra', N'Curcuma longa', N'Haldi',
     N'Rasa: Tikta, Katu, Guna: Laghu, Ruksha, Virya: Ushna, Vipaka: Katu',
     N'Inflammation, wounds, skin diseases, liver health, immunity',
     N'1-3g powder with milk or warm water',
     N'Gallstones, bile duct obstruction, pregnancy (high doses)',
     N'Neutral', N'Neutral', N'Balancing'),
    
    (N'Dalchini', N'Tvak', N'Cinnamomum zeylanicum', N'Dalchini',
     N'Rasa: Katu, Tikta, Madhura, Guna: Laghu, Ruksha, Virya: Ushna, Vipaka: Katu',
     N'Diabetes, digestive disorders, respiratory issues, circulation',
     N'1-3g powder or as spice',
     N'Pregnancy (high doses), bleeding disorders',
     N'Balancing', N'Aggravating', N'Balancing'),
    
    (N'Giloy', N'Guduchi', N'Tinospora cordifolia', N'Giloy',
     N'Rasa: Tikta, Kashaya, Guna: Laghu, Snigdha, Virya: Ushna, Vipaka: Madhura',
     N'Immunity, fever, diabetes, liver health, rejuvenation',
     N'3-6g powder or 10-20ml juice',
     N'Pregnancy, lactation, autoimmune diseases',
     N'Balancing', N'Balancing', N'Neutral'),
    
    (N'Bala', N'Bala', N'Sida cordifolia', N'Bala',
     N'Rasa: Madhura, Guna: Snigdha, Picchila, Virya: Sheeta, Vipaka: Madhura',
     N'Weakness, nervous disorders, heart health, rejuvenation',
     N'3-6g powder with milk',
     N'High Kapha conditions',
     N'Balancing', N'Balancing', N'Aggravating'),
    
    (N'Shatavari', N'Shatavari', N'Asparagus racemosus', N'Shatavari',
     N'Rasa: Madhura, Tikta, Guna: Guru, Snigdha, Virya: Sheeta, Vipaka: Madhura',
     N'Female reproductive health, lactation, acidity, immunity',
     N'3-6g powder with milk',
     N'High Kapha, congestion',
     N'Balancing', N'Balancing', N'Aggravating'),
    
    (N'Yashtimadhu', N'Yashtimadhu', N'Glycyrrhiza glabra', N'Mulethi',
     N'Rasa: Madhura, Guna: Guru, Snigdha, Virya: Sheeta, Vipaka: Madhura',
     N'Cough, acidity, ulcers, voice, respiratory health',
     N'1-3g powder with honey or milk',
     N'Hypertension, edema, pregnancy (high doses)',
     N'Balancing', N'Balancing', N'Aggravating');
    
    PRINT '  ✓ Inserted 35 herbal medicines';
END
ELSE
BEGIN
    PRINT '  ⚠ HERBALMEDICINES already contains data';
END
GO

-- =============================================
-- Seed Data 4: YOGAASANAS
-- Description: 25 Yoga asanas across categories
-- =============================================
PRINT 'Step 4/7: Seeding YOGAASANAS...';

IF NOT EXISTS (SELECT * FROM YOGAASANAS)
BEGIN
    INSERT INTO YOGAASANAS (AsanaName, SanskritName, Category, Benefits, Duration, Difficulty, Instructions, Precautions, VataEffect, PittaEffect, KaphaEffect)
    VALUES 
    -- Standing Poses
    (N'Mountain Pose', N'Tadasana', N'Standing',
     N'Improves posture, strengthens legs, enhances balance, increases awareness',
     N'1-2 minutes', N'Beginner',
     N'Stand with feet together, distribute weight evenly, engage thighs, lift chest, relax shoulders, breathe deeply',
     N'None, safe for all',
     N'Balancing', N'Balancing', N'Balancing'),
    
    (N'Tree Pose', N'Vrikshasana', N'Standing',
     N'Improves balance, strengthens legs, enhances concentration, stretches hips',
     N'30-60 seconds each side', N'Beginner',
     N'Stand on one leg, place other foot on inner thigh or calf, hands in prayer position or overhead',
     N'Avoid if you have severe balance issues',
     N'Balancing', N'Balancing', N'Balancing'),
    
    (N'Warrior I', N'Virabhadrasana I', N'Standing',
     N'Strengthens legs, opens hips and chest, improves focus, builds stamina',
     N'30-60 seconds each side', N'Beginner',
     N'Step one foot back, bend front knee, raise arms overhead, gaze upward',
     N'Avoid with knee or hip injuries',
     N'Balancing', N'Neutral', N'Balancing'),
    
    (N'Triangle Pose', N'Trikonasana', N'Standing',
     N'Stretches legs and hips, improves digestion, reduces stress, strengthens core',
     N'30-60 seconds each side', N'Beginner',
     N'Stand wide, turn one foot out, extend arms, reach side and down, gaze up',
     N'Avoid with low blood pressure',
     N'Balancing', N'Balancing', N'Balancing'),
    
    -- Sitting Poses
    (N'Easy Pose', N'Sukhasana', N'Sitting',
     N'Calms mind, opens hips, strengthens back, improves posture',
     N'5-10 minutes', N'Beginner',
     N'Sit cross-legged, spine straight, hands on knees, breathe deeply',
     N'Use cushion if hips are tight',
     N'Balancing', N'Balancing', N'Neutral'),
    
    (N'Lotus Pose', N'Padmasana', N'Sitting',
     N'Calms mind, opens hips, improves circulation, enhances meditation',
     N'5-10 minutes', N'Advanced',
     N'Sit with each foot on opposite thigh, spine straight, hands in mudra',
     N'Avoid with knee or ankle injuries',
     N'Balancing', N'Balancing', N'Neutral'),
    
    (N'Seated Forward Bend', N'Paschimottanasana', N'Sitting',
     N'Stretches spine and hamstrings, calms mind, improves digestion, reduces stress',
     N'1-3 minutes', N'Intermediate',
     N'Sit with legs extended, fold forward from hips, reach for feet',
     N'Avoid with back injuries or herniated disc',
     N'Balancing', N'Balancing', N'Balancing'),
    
    (N'Boat Pose', N'Navasana', N'Sitting',
     N'Strengthens core and hip flexors, improves balance, stimulates kidneys',
     N'30-60 seconds', N'Intermediate',
     N'Sit, lift legs and torso to form V shape, arms parallel to floor',
     N'Avoid with neck or back injuries',
     N'Neutral', N'Neutral', N'Balancing'),
    
    -- Lying Poses
    (N'Corpse Pose', N'Shavasana', N'Lying',
     N'Deep relaxation, reduces stress, lowers blood pressure, calms nervous system',
     N'5-15 minutes', N'Beginner',
     N'Lie on back, arms at sides, palms up, legs slightly apart, close eyes, relax completely',
     N'None, safe for all',
     N'Balancing', N'Balancing', N'Neutral'),
    
    (N'Cobra Pose', N'Bhujangasana', N'Lying',
     N'Strengthens spine, opens chest, stimulates kidneys, reduces stress',
     N'30-60 seconds', N'Beginner',
     N'Lie on stomach, hands under shoulders, lift chest, keep elbows bent',
     N'Avoid with back injuries or pregnancy',
     N'Balancing', N'Neutral', N'Balancing'),
    
    (N'Bridge Pose', N'Setu Bandhasana', N'Lying',
     N'Strengthens back, opens chest, stimulates kidneys, reduces hypertension',
     N'30-60 seconds', N'Beginner',
     N'Lie on back, bend knees, lift hips, clasp hands under back',
     N'Avoid with neck injuries',
     N'Balancing', N'Balancing', N'Balancing'),
    
    (N'Locust Pose', N'Shalabhasana', N'Lying',
     N'Strengthens back, improves posture, stimulates digestion, reduces stress',
     N'30-60 seconds', N'Intermediate',
     N'Lie on stomach, lift chest and legs, arms alongside body or extended',
     N'Avoid with back injuries or pregnancy',
     N'Balancing', N'Neutral', N'Balancing'),
    
    (N'Supine Twist', N'Supta Matsyendrasana', N'Lying',
     N'Stretches spine, improves digestion, releases tension, detoxifies',
     N'1-2 minutes each side', N'Beginner',
     N'Lie on back, bring knee to chest, twist to side, arms extended',
     N'Avoid with spine injuries',
     N'Balancing', N'Balancing', N'Balancing'),
    
    -- Inversions
    (N'Legs Up the Wall', N'Viparita Karani', N'Inversion',
     N'Reduces swelling in legs, calms mind, lowers blood pressure, improves circulation',
     N'5-15 minutes', N'Beginner',
     N'Lie with hips close to wall, extend legs up wall, arms at sides',
     N'Avoid during menstruation or with glaucoma',
     N'Balancing', N'Balancing', N'Balancing'),
    
    (N'Shoulder Stand', N'Sarvangasana', N'Inversion',
     N'Improves circulation, stimulates thyroid, calms nervous system, reduces stress',
     N'1-5 minutes', N'Advanced',
     N'Lie on back, lift legs and hips, support back with hands',
     N'Avoid with neck injuries, high blood pressure, menstruation',
     N'Balancing', N'Balancing', N'Neutral'),
    
    -- Pranayama (Breathing)
    (N'Alternate Nostril Breathing', N'Anulom Vilom', N'Pranayama',
     N'Reduces blood pressure, calms mind, balances doshas, improves lung capacity',
     N'5-10 minutes', N'Beginner',
     N'Sit comfortably, close right nostril, inhale left, close left, exhale right, repeat',
     N'Avoid if you have cold or nasal congestion',
     N'Balancing', N'Balancing', N'Balancing'),
    
    (N'Bellows Breath', N'Bhastrika', N'Pranayama',
     N'Increases energy, clears respiratory system, improves digestion, reduces Kapha',
     N'3-5 minutes', N'Intermediate',
     N'Sit comfortably, forceful inhale and exhale through nose, equal duration',
     N'Avoid with high blood pressure, heart disease, pregnancy',
     N'Balancing', N'Aggravating', N'Balancing'),
    
    (N'Cooling Breath', N'Shitali Pranayama', N'Pranayama',
     N'Reduces body heat, calms mind, reduces Pitta, lowers blood pressure',
     N'5-10 minutes', N'Beginner',
     N'Sit comfortably, curl tongue, inhale through tongue, exhale through nose',
     N'Avoid in cold weather or with respiratory issues',
     N'Neutral', N'Balancing', N'Aggravating'),
    
    (N'Skull Shining Breath', N'Kapalabhati', N'Pranayama',
     N'Cleanses respiratory system, increases energy, improves digestion, reduces Kapha',
     N'3-5 minutes', N'Intermediate',
     N'Sit comfortably, forceful exhale through nose, passive inhale, focus on exhalation',
     N'Avoid with high blood pressure, heart disease, pregnancy, hernia',
     N'Balancing', N'Aggravating', N'Balancing'),
    
    (N'Bee Breath', N'Bhramari Pranayama', N'Pranayama',
     N'Calms mind, reduces anxiety, lowers blood pressure, improves concentration',
     N'5-10 minutes', N'Beginner',
     N'Sit comfortably, close ears with fingers, inhale, exhale making humming sound',
     N'Avoid with ear infections',
     N'Balancing', N'Balancing', N'Neutral'),
    
    -- Twists
    (N'Seated Spinal Twist', N'Ardha Matsyendrasana', N'Sitting',
     N'Improves digestion, stretches spine, stimulates liver and kidneys, detoxifies',
     N'30-60 seconds each side', N'Intermediate',
     N'Sit with one leg bent, twist torso, place opposite elbow outside knee',
     N'Avoid with spine injuries or pregnancy',
     N'Balancing', N'Balancing', N'Balancing'),
    
    -- Core Strengthening
    (N'Plank Pose', N'Phalakasana', N'Core',
     N'Strengthens core, arms, and legs, improves posture, builds endurance',
     N'30-60 seconds', N'Beginner',
     N'From hands and knees, extend legs back, body in straight line, hold',
     N'Avoid with wrist or shoulder injuries',
     N'Neutral', N'Neutral', N'Balancing'),
    
    (N'Cat-Cow Pose', N'Marjaryasana-Bitilasana', N'Core',
     N'Stretches spine, improves flexibility, massages organs, relieves back pain',
     N'1-2 minutes', N'Beginner',
     N'On hands and knees, arch back (cow), round back (cat), flow with breath',
     N'Avoid with neck injuries',
     N'Balancing', N'Balancing', N'Balancing'),
    
    (N'Downward Facing Dog', N'Adho Mukha Svanasana', N'Core',
     N'Stretches entire body, strengthens arms and legs, energizes, improves circulation',
     N'1-3 minutes', N'Beginner',
     N'From hands and knees, lift hips up and back, form inverted V shape',
     N'Avoid with wrist injuries, high blood pressure, late pregnancy',
     N'Balancing', N'Neutral', N'Balancing'),
    
    (N'Child Pose', N'Balasana', N'Resting',
     N'Calms mind, stretches back and hips, relieves stress, aids digestion',
     N'1-5 minutes', N'Beginner',
     N'Kneel, sit on heels, fold forward, arms extended or alongside body',
     N'Avoid with knee injuries or pregnancy',
     N'Balancing', N'Balancing', N'Neutral');
    
    PRINT '  ✓ Inserted 25 yoga asanas';
END
ELSE
BEGIN
    PRINT '  ⚠ YOGAASANAS already contains data';
END
GO

-- =============================================
-- Seed Data 5: DIETARYITEMS
-- Description: 60+ food items with Ayurvedic properties
-- =============================================
PRINT 'Step 5/7: Seeding DIETARYITEMS...';

IF NOT EXISTS (SELECT * FROM DIETARYITEMS)
BEGIN
    INSERT INTO DIETARYITEMS (FoodName, Category, VataEffect, PittaEffect, KaphaEffect, Properties, Benefits, Rasa, Virya)
    VALUES 
    -- Grains
    (N'Rice (White)', N'Grains', N'Balancing', N'Balancing', N'Aggravating',
     N'Easy to digest, cooling, nourishing', N'Provides energy, easy on digestion, calming',
     N'Sweet', N'Cooling'),
    (N'Rice (Brown)', N'Grains', N'Neutral', N'Balancing', N'Neutral',
     N'High fiber, nutritious', N'Improves digestion, provides sustained energy',
     N'Sweet', N'Cooling'),
    (N'Wheat', N'Grains', N'Balancing', N'Balancing', N'Aggravating',
     N'Nourishing, grounding', N'Builds strength, nourishes tissues',
     N'Sweet', N'Cooling'),
    (N'Barley', N'Grains', N'Neutral', N'Balancing', N'Balancing',
     N'Cooling, light, drying', N'Reduces Kapha, aids weight loss, cooling',
     N'Sweet', N'Cooling'),
    (N'Oats', N'Grains', N'Balancing', N'Neutral', N'Neutral',
     N'Nourishing, grounding', N'Provides sustained energy, heart healthy',
     N'Sweet', N'Heating'),
    (N'Quinoa', N'Grains', N'Balancing', N'Balancing', N'Neutral',
     N'Protein-rich, light', N'Complete protein, easy to digest',
     N'Sweet', N'Heating'),
    (N'Millet', N'Grains', N'Neutral', N'Balancing', N'Balancing',
     N'Light, drying, warming', N'Good for weight loss, easy to digest',
     N'Sweet', N'Heating'),
    
    -- Vegetables
    (N'Spinach', N'Vegetables', N'Neutral', N'Balancing', N'Balancing',
     N'Cooling, light, nutritious', N'Rich in iron, purifies blood',
     N'Bitter, Astringent', N'Cooling'),
    (N'Carrot', N'Vegetables', N'Balancing', N'Balancing', N'Neutral',
     N'Grounding, sweet, nourishing', N'Good for eyes, improves digestion',
     N'Sweet', N'Neutral'),
    (N'Beetroot', N'Vegetables', N'Balancing', N'Neutral', N'Neutral',
     N'Grounding, sweet, purifying', N'Purifies blood, improves stamina',
     N'Sweet', N'Heating'),
    (N'Cucumber', N'Vegetables', N'Balancing', N'Balancing', N'Aggravating',
     N'Cooling, hydrating, light', N'Reduces heat, hydrates body',
     N'Sweet', N'Cooling'),
    (N'Bitter Gourd', N'Vegetables', N'Neutral', N'Balancing', N'Balancing',
     N'Cooling, light, drying', N'Controls blood sugar, purifies blood',
     N'Bitter', N'Cooling'),
    (N'Bottle Gourd', N'Vegetables', N'Balancing', N'Balancing', N'Balancing',
     N'Cooling, light, easy to digest', N'Calming, easy on digestion',
     N'Sweet', N'Cooling'),
    (N'Pumpkin', N'Vegetables', N'Balancing', N'Balancing', N'Neutral',
     N'Sweet, grounding, nourishing', N'Easy to digest, nourishing',
     N'Sweet', N'Cooling'),
    (N'Tomato', N'Vegetables', N'Neutral', N'Aggravating', N'Balancing',
     N'Heating, acidic', N'Rich in antioxidants',
     N'Sour, Sweet', N'Heating'),
    (N'Broccoli', N'Vegetables', N'Neutral', N'Balancing', N'Balancing',
     N'Light, drying, nutritious', N'Detoxifying, anti-cancer properties',
     N'Bitter, Astringent', N'Cooling'),
    (N'Cauliflower', N'Vegetables', N'Aggravating', N'Neutral', N'Balancing',
     N'Light, drying', N'Good for weight loss',
     N'Sweet, Astringent', N'Heating'),
    (N'Green Beans', N'Vegetables', N'Balancing', N'Balancing', N'Neutral',
     N'Light, easy to digest', N'Nutritious, easy on digestion',
     N'Sweet, Astringent', N'Cooling'),
    (N'Radish', N'Vegetables', N'Aggravating', N'Aggravating', N'Balancing',
     N'Heating, pungent, drying', N'Improves digestion, clears congestion',
     N'Pungent', N'Heating'),
    (N'Sweet Potato', N'Vegetables', N'Balancing', N'Balancing', N'Aggravating',
     N'Sweet, grounding, nourishing', N'Provides energy, nourishing',
     N'Sweet', N'Heating'),
    
    -- Fruits
    (N'Apple', N'Fruits', N'Neutral', N'Balancing', N'Balancing',
     N'Cooling, astringent, cleansing', N'Good for digestion, heart health',
     N'Sweet, Astringent', N'Cooling'),
    (N'Banana', N'Fruits', N'Balancing', N'Neutral', N'Aggravating',
     N'Sweet, heavy, nourishing', N'Provides energy, easy to digest when ripe',
     N'Sweet', N'Heating'),
    (N'Mango', N'Fruits', N'Balancing', N'Aggravating', N'Aggravating',
     N'Sweet, heavy, nourishing', N'Nourishing, aphrodisiac',
     N'Sweet', N'Heating'),
    (N'Papaya', N'Fruits', N'Balancing', N'Balancing', N'Balancing',
     N'Sweet, light, easy to digest', N'Improves digestion, gentle laxative',
     N'Sweet', N'Heating'),
    (N'Pomegranate', N'Fruits', N'Balancing', N'Balancing', N'Balancing',
     N'Sweet, astringent, cooling', N'Purifies blood, good for heart',
     N'Sweet, Astringent', N'Cooling'),
    (N'Grapes', N'Fruits', N'Balancing', N'Balancing', N'Aggravating',
     N'Sweet, cooling, nourishing', N'Rejuvenating, good for blood',
     N'Sweet', N'Cooling'),
    (N'Orange', N'Fruits', N'Neutral', N'Aggravating', N'Balancing',
     N'Sour, heating', N'Rich in Vitamin C, boosts immunity',
     N'Sour, Sweet', N'Heating'),
    (N'Watermelon', N'Fruits', N'Balancing', N'Balancing', N'Aggravating',
     N'Sweet, cooling, hydrating', N'Hydrates, reduces heat',
     N'Sweet', N'Cooling'),
    (N'Pear', N'Fruits', N'Balancing', N'Balancing', N'Neutral',
     N'Sweet, cooling, light', N'Easy to digest, cooling',
     N'Sweet', N'Cooling'),
    (N'Dates', N'Fruits', N'Balancing', N'Neutral', N'Aggravating',
     N'Sweet, heavy, nourishing', N'Provides energy, laxative, strengthening',
     N'Sweet', N'Heating'),
    (N'Figs', N'Fruits', N'Balancing', N'Neutral', N'Neutral',
     N'Sweet, heavy, nourishing', N'Laxative, nourishing',
     N'Sweet', N'Heating'),
    (N'Berries (Mixed)', N'Fruits', N'Neutral', N'Balancing', N'Balancing',
     N'Astringent, cooling, light', N'Antioxidant-rich, good for urinary system',
     N'Sweet, Astringent', N'Cooling'),
    
    -- Legumes
    (N'Mung Beans', N'Legumes', N'Balancing', N'Balancing', N'Balancing',
     N'Light, easy to digest, cooling', N'Best legume for all doshas, detoxifying',
     N'Sweet', N'Cooling'),
    (N'Red Lentils', N'Legumes', N'Balancing', N'Balancing', N'Neutral',
     N'Light, easy to digest', N'Good protein source, easy to digest',
     N'Sweet', N'Cooling'),
    (N'Chickpeas', N'Legumes', N'Aggravating', N'Neutral', N'Balancing',
     N'Heavy, drying', N'Good protein, reduces Kapha',
     N'Sweet, Astringent', N'Cooling'),
    (N'Black Gram', N'Legumes', N'Balancing', N'Aggravating', N'Aggravating',
     N'Heavy, nourishing, heating', N'Strengthening, aphrodisiac',
     N'Sweet', N'Heating'),
    (N'Kidney Beans', N'Legumes', N'Aggravating', N'Neutral', N'Balancing',
     N'Heavy, drying', N'Good protein, fiber-rich',
     N'Sweet, Astringent', N'Neutral'),
    
    -- Dairy
    (N'Cow Milk', N'Dairy', N'Balancing', N'Balancing', N'Aggravating',
     N'Sweet, cooling, nourishing', N'Nourishing, calming, builds tissues',
     N'Sweet', N'Cooling'),
    (N'Ghee', N'Dairy', N'Balancing', N'Balancing', N'Aggravating',
     N'Sweet, cooling, nourishing', N'Rejuvenating, improves digestion, lubricating',
     N'Sweet', N'Cooling'),
    (N'Yogurt', N'Dairy', N'Aggravating', N'Aggravating', N'Aggravating',
     N'Sour, heating, heavy', N'Probiotic, good for digestion (in moderation)',
     N'Sour', N'Heating'),
    (N'Buttermilk', N'Dairy', N'Balancing', N'Balancing', N'Balancing',
     N'Astringent, light, cooling', N'Improves digestion, cooling',
     N'Astringent, Sour', N'Heating'),
    (N'Paneer', N'Dairy', N'Balancing', N'Neutral', N'Aggravating',
     N'Sweet, cooling, heavy', N'Good protein source',
     N'Sweet', N'Cooling'),
    
    -- Nuts and Seeds
    (N'Almonds', N'Nuts', N'Balancing', N'Aggravating', N'Aggravating',
     N'Sweet, heavy, nourishing', N'Brain tonic, strengthening, nourishing',
     N'Sweet', N'Heating'),
    (N'Walnuts', N'Nuts', N'Balancing', N'Aggravating', N'Aggravating',
     N'Sweet, heavy, oily', N'Brain tonic, good for heart',
     N'Sweet', N'Heating'),
    (N'Cashews', N'Nuts', N'Balancing', N'Aggravating', N'Aggravating',
     N'Sweet, heavy, oily', N'Nourishing, strengthening',
     N'Sweet', N'Heating'),
    (N'Pumpkin Seeds', N'Seeds', N'Balancing', N'Neutral', N'Neutral',
     N'Sweet, light, nutritious', N'Good for prostate, rich in zinc',
     N'Sweet', N'Neutral'),
    (N'Sunflower Seeds', N'Seeds', N'Balancing', N'Neutral', N'Neutral',
     N'Sweet, light, nutritious', N'Rich in Vitamin E',
     N'Sweet', N'Heating'),
    (N'Sesame Seeds', N'Seeds', N'Balancing', N'Aggravating', N'Aggravating',
     N'Sweet, heavy, oily, heating', N'Strengthening, good for bones',
     N'Sweet', N'Heating'),
    (N'Flax Seeds', N'Seeds', N'Balancing', N'Neutral', N'Balancing',
     N'Sweet, oily, heavy', N'Good for heart, omega-3 rich',
     N'Sweet', N'Heating'),
    
    -- Spices
    (N'Ginger (Fresh)', N'Spices', N'Balancing', N'Aggravating', N'Balancing',
     N'Pungent, heating, stimulating', N'Improves digestion, reduces nausea, anti-inflammatory',
     N'Pungent', N'Heating'),
    (N'Ginger (Dry)', N'Spices', N'Balancing', N'Aggravating', N'Balancing',
     N'Pungent, very heating', N'Improves digestion, reduces Kapha',
     N'Pungent', N'Heating'),
    (N'Turmeric', N'Spices', N'Neutral', N'Neutral', N'Balancing',
     N'Bitter, pungent, heating', N'Anti-inflammatory, purifies blood, heals wounds',
     N'Bitter, Pungent', N'Heating'),
    (N'Cumin', N'Spices', N'Balancing', N'Balancing', N'Balancing',
     N'Pungent, bitter, cooling', N'Improves digestion, cooling',
     N'Pungent, Bitter', N'Cooling'),
    (N'Coriander', N'Spices', N'Balancing', N'Balancing', N'Balancing',
     N'Sweet, cooling, light', N'Cooling, improves digestion, diuretic',
     N'Sweet', N'Cooling'),
    (N'Fennel', N'Spices', N'Balancing', N'Balancing', N'Neutral',
     N'Sweet, cooling, light', N'Improves digestion, cooling, freshens breath',
     N'Sweet', N'Cooling'),
    (N'Cardamom', N'Spices', N'Balancing', N'Balancing', N'Balancing',
     N'Sweet, pungent, cooling', N'Improves digestion, freshens breath, stimulating',
     N'Sweet, Pungent', N'Cooling'),
    (N'Cinnamon', N'Spices', N'Balancing', N'Aggravating', N'Balancing',
     N'Pungent, sweet, heating', N'Improves circulation, regulates blood sugar',
     N'Pungent, Sweet', N'Heating'),
    (N'Black Pepper', N'Spices', N'Balancing', N'Aggravating', N'Balancing',
     N'Pungent, heating, stimulating', N'Improves digestion, clears congestion',
     N'Pungent', N'Heating'),
    (N'Cloves', N'Spices', N'Neutral', N'Aggravating', N'Balancing',
     N'Pungent, heating, stimulating', N'Improves digestion, relieves toothache',
     N'Pungent', N'Heating'),
    (N'Fenugreek', N'Spices', N'Balancing', N'Aggravating', N'Balancing',
     N'Bitter, pungent, heating', N'Controls blood sugar, increases lactation',
     N'Bitter, Pungent', N'Heating'),
    (N'Mustard Seeds', N'Spices', N'Balancing', N'Aggravating', N'Balancing',
     N'Pungent, heating, stimulating', N'Improves digestion, clears congestion',
     N'Pungent', N'Heating'),
    (N'Asafoetida', N'Spices', N'Balancing', N'Aggravating', N'Balancing',
     N'Pungent, heating, strong', N'Reduces gas, improves digestion',
     N'Pungent', N'Heating'),
    
    -- Oils
    (N'Coconut Oil', N'Oils', N'Neutral', N'Balancing', N'Aggravating',
     N'Sweet, cooling, heavy', N'Cooling, nourishing, good for Pitta',
     N'Sweet', N'Cooling'),
    (N'Sesame Oil', N'Oils', N'Balancing', N'Aggravating', N'Aggravating',
     N'Sweet, heating, heavy', N'Warming, nourishing, good for Vata',
     N'Sweet', N'Heating'),
    (N'Olive Oil', N'Oils', N'Balancing', N'Neutral', N'Neutral',
     N'Sweet, neutral, light', N'Heart healthy, anti-inflammatory',
     N'Sweet', N'Neutral'),
    (N'Mustard Oil', N'Oils', N'Balancing', N'Aggravating', N'Balancing',
     N'Pungent, heating, stimulating', N'Warming, improves circulation',
     N'Pungent', N'Heating'),
    
    -- Sweeteners
    (N'Honey (Raw)', N'Sweeteners', N'Balancing', N'Aggravating', N'Balancing',
     N'Sweet, astringent, heating', N'Healing, reduces Kapha, do not heat',
     N'Sweet, Astringent', N'Heating'),
    (N'Jaggery', N'Sweeteners', N'Balancing', N'Neutral', N'Aggravating',
     N'Sweet, heating, heavy', N'Purifies blood, good for digestion',
     N'Sweet', N'Heating'),
    (N'Rock Sugar', N'Sweeteners', N'Balancing', N'Balancing', N'Aggravating',
     N'Sweet, cooling, light', N'Cooling, gentle sweetener',
     N'Sweet', N'Cooling'),
    
    -- Beverages
    (N'Green Tea', N'Beverages', N'Neutral', N'Balancing', N'Balancing',
     N'Bitter, astringent, cooling', N'Antioxidant, aids weight loss',
     N'Bitter, Astringent', N'Cooling'),
    (N'Ginger Tea', N'Beverages', N'Balancing', N'Aggravating', N'Balancing',
     N'Pungent, heating, stimulating', N'Improves digestion, warming',
     N'Pungent', N'Heating'),
    (N'Coconut Water', N'Beverages', N'Balancing', N'Balancing', N'Aggravating',
     N'Sweet, cooling, hydrating', N'Hydrating, cooling, electrolyte-rich',
     N'Sweet', N'Cooling'),
    (N'Lemon Water', N'Beverages', N'Neutral', N'Aggravating', N'Balancing',
     N'Sour, cooling (after digestion)', N'Detoxifying, alkalizing',
     N'Sour', N'Cooling');
    
    PRINT '  ✓ Inserted 65+ dietary items';
END
ELSE
BEGIN
    PRINT '  ⚠ DIETARYITEMS already contains data';
END
GO

-- =============================================
-- Seed Data 6: CONDITIONTREATMENTMAPPING
-- Description: Pre-configured treatment mappings
-- =============================================
PRINT 'Step 6/7: Seeding CONDITIONTREATMENTMAPPING...';

IF NOT EXISTS (SELECT * FROM CONDITIONTREATMENTMAPPING)
BEGIN
    DECLARE @KidneyStoneId BIGINT, @UrinaryStoneId BIGINT, @ObesityId BIGINT, @HypertensionId BIGINT, @DiabetesId BIGINT;
    DECLARE @GokshuraId BIGINT, @PunarnavId BIGINT, @VarunaId BIGINT, @PashanbhedaId BIGINT;
    DECLARE @GugguluId BIGINT, @TriphalaId BIGINT, @VrikshamilaId BIGINT, @ShilajitId BIGINT;
    DECLARE @ArjunaId BIGINT, @SarpagandhaId BIGINT, @JatamansiId BIGINT, @AshwagandhaId BIGINT;
    DECLARE @GudmarId BIGINT, @JamunId BIGINT, @KarelaId BIGINT, @MethiId BIGINT;
    
    -- Get Condition IDs
    SELECT @KidneyStoneId = Id FROM CONDITIONS WHERE Name = N'Kidney Stones';
    SELECT @UrinaryStoneId = Id FROM CONDITIONS WHERE Name = N'Urinary Stones';
    SELECT @ObesityId = Id FROM CONDITIONS WHERE Name = N'Obesity';
    SELECT @HypertensionId = Id FROM CONDITIONS WHERE Name = N'Hypertension';
    SELECT @DiabetesId = Id FROM CONDITIONS WHERE Name = N'Type 2 Diabetes';
    
    -- Get Medicine IDs
    SELECT @GokshuraId = Id FROM HERBALMEDICINES WHERE CommonName = N'Gokshura';
    SELECT @PunarnavId = Id FROM HERBALMEDICINES WHERE CommonName = N'Punarnava';
    SELECT @VarunaId = Id FROM HERBALMEDICINES WHERE CommonName = N'Varuna';
    SELECT @PashanbhedaId = Id FROM HERBALMEDICINES WHERE CommonName = N'Pashanbheda';
    SELECT @GugguluId = Id FROM HERBALMEDICINES WHERE CommonName = N'Guggulu';
    SELECT @TriphalaId = Id FROM HERBALMEDICINES WHERE CommonName = N'Triphala';
    SELECT @VrikshamilaId = Id FROM HERBALMEDICINES WHERE CommonName = N'Vrikshamla';
    SELECT @ShilajitId = Id FROM HERBALMEDICINES WHERE CommonName = N'Shilajit';
    SELECT @ArjunaId = Id FROM HERBALMEDICINES WHERE CommonName = N'Arjuna';
    SELECT @SarpagandhaId = Id FROM HERBALMEDICINES WHERE CommonName = N'Sarpagandha';
    SELECT @JatamansiId = Id FROM HERBALMEDICINES WHERE CommonName = N'Jatamansi';
    SELECT @AshwagandhaId = Id FROM HERBALMEDICINES WHERE CommonName = N'Ashwagandha';
    SELECT @GudmarId = Id FROM HERBALMEDICINES WHERE CommonName = N'Gudmar';
    SELECT @JamunId = Id FROM HERBALMEDICINES WHERE CommonName = N'Jamun';
    SELECT @KarelaId = Id FROM HERBALMEDICINES WHERE CommonName = N'Karela';
    SELECT @MethiId = Id FROM HERBALMEDICINES WHERE CommonName = N'Methi';
    
    -- Kidney Stones Treatments
    INSERT INTO CONDITIONTREATMENTMAPPING (ConditionId, TreatmentType, TreatmentItemId, Prakriti, Priority, SuccessRate, Notes)
    VALUES 
    (@KidneyStoneId, N'Medicine', @GokshuraId, N'All', 1, 85.5, N'Primary medicine for kidney stones, lithotriptic action'),
    (@KidneyStoneId, N'Medicine', @VarunaId, N'All', 1, 82.0, N'Stone breaker, excellent for calculi'),
    (@KidneyStoneId, N'Medicine', @PashanbhedaId, N'All', 1, 80.0, N'Literally means stone breaker'),
    (@KidneyStoneId, N'Medicine', @PunarnavId, N'Kapha', 2, 75.0, N'Diuretic, reduces Kapha'),
    (@KidneyStoneId, N'Yoga', (SELECT Id FROM YOGAASANAS WHERE AsanaName = N'Cobra Pose'), N'All', 1, 78.0, N'Stimulates kidneys'),
    (@KidneyStoneId, N'Yoga', (SELECT Id FROM YOGAASANAS WHERE AsanaName = N'Bridge Pose'), N'All', 1, 76.0, N'Stimulates kidneys, reduces hypertension'),
    (@KidneyStoneId, N'Yoga', (SELECT Id FROM YOGAASANAS WHERE AsanaName = N'Boat Pose'), N'All', 2, 70.0, N'Strengthens core, stimulates kidneys'),
    
    -- Urinary Stones Treatments
    (@UrinaryStoneId, N'Medicine', @GokshuraId, N'All', 1, 84.0, N'Excellent for urinary disorders'),
    (@UrinaryStoneId, N'Medicine', @VarunaId, N'All', 1, 83.0, N'Specific for urinary calculi'),
    (@UrinaryStoneId, N'Medicine', @PunarnavId, N'All', 2, 76.0, N'Diuretic action'),
    (@UrinaryStoneId, N'Yoga', (SELECT Id FROM YOGAASANAS WHERE AsanaName = N'Cobra Pose'), N'All', 1, 75.0, N'Stimulates urinary system'),
    (@UrinaryStoneId, N'Yoga', (SELECT Id FROM YOGAASANAS WHERE AsanaName = N'Bridge Pose'), N'All', 1, 74.0, N'Improves pelvic circulation'),
    
    -- Obesity Treatments
    (@ObesityId, N'Medicine', @GugguluId, N'Kapha', 1, 78.5, N'Best for Kapha obesity, lipid metabolism'),
    (@ObesityId, N'Medicine', @TriphalaId, N'All', 1, 72.0, N'Detoxification, improves metabolism'),
    (@ObesityId, N'Medicine', @VrikshamilaId, N'Kapha', 1, 75.0, N'Appetite control, fat metabolism'),
    (@ObesityId, N'Medicine', @ShilajitId, N'Vata-Kapha', 2, 70.0, N'Improves metabolism, rejuvenation'),
    (@ObesityId, N'Yoga', (SELECT Id FROM YOGAASANAS WHERE AsanaName = N'Boat Pose'), N'All', 1, 80.0, N'Core strengthening, fat burning'),
    (@ObesityId, N'Yoga', (SELECT Id FROM YOGAASANAS WHERE AsanaName = N'Plank Pose'), N'All', 1, 78.0, N'Full body strengthening'),
    (@ObesityId, N'Yoga', (SELECT Id FROM YOGAASANAS WHERE AsanaName = N'Warrior I'), N'All', 1, 76.0, N'Builds stamina, burns calories'),
    (@ObesityId, N'Yoga', (SELECT Id FROM YOGAASANAS WHERE AsanaName = N'Skull Shining Breath'), N'Kapha', 1, 82.0, N'Increases metabolism, reduces Kapha'),
    
    -- Hypertension Treatments
    (@HypertensionId, N'Medicine', @ArjunaId, N'All', 1, 83.0, N'Cardioprotective, reduces blood pressure'),
    (@HypertensionId, N'Medicine', @SarpagandhaId, N'Vata-Pitta', 1, 80.0, N'Specific for hypertension'),
    (@HypertensionId, N'Medicine', @JatamansiId, N'All', 2, 75.0, N'Calming, reduces stress-induced hypertension'),
    (@HypertensionId, N'Medicine', @AshwagandhaId, N'Vata', 2, 72.0, N'Reduces stress, adaptogenic'),
    (@HypertensionId, N'Yoga', (SELECT Id FROM YOGAASANAS WHERE AsanaName = N'Alternate Nostril Breathing'), N'All', 1, 85.0, N'Directly reduces blood pressure'),
    (@HypertensionId, N'Yoga', (SELECT Id FROM YOGAASANAS WHERE AsanaName = N'Corpse Pose'), N'All', 1, 80.0, N'Deep relaxation, stress reduction'),
    (@HypertensionId, N'Yoga', (SELECT Id FROM YOGAASANAS WHERE AsanaName = N'Bridge Pose'), N'All', 2, 76.0, N'Reduces hypertension'),
    (@HypertensionId, N'Yoga', (SELECT Id FROM YOGAASANAS WHERE AsanaName = N'Bee Breath'), N'All', 2, 78.0, N'Calms mind, lowers blood pressure'),
    
    -- Diabetes Treatments
    (@DiabetesId, N'Medicine', @GudmarId, N'All', 1, 82.0, N'Sugar destroyer, reduces cravings'),
    (@DiabetesId, N'Medicine', @JamunId, N'All', 1, 78.0, N'Controls blood sugar'),
    (@DiabetesId, N'Medicine', @KarelaId, N'All', 1, 76.0, N'Reduces blood sugar'),
    (@DiabetesId, N'Medicine', @MethiId, N'All', 2, 74.0, N'Improves insulin sensitivity'),
    (@DiabetesId, N'Yoga', (SELECT Id FROM YOGAASANAS WHERE AsanaName = N'Boat Pose'), N'All', 1, 75.0, N'Stimulates pancreas'),
    (@DiabetesId, N'Yoga', (SELECT Id FROM YOGAASANAS WHERE AsanaName = N'Seated Forward Bend'), N'All', 1, 73.0, N'Stimulates abdominal organs'),
    (@DiabetesId, N'Yoga', (SELECT Id FROM YOGAASANAS WHERE AsanaName = N'Cobra Pose'), N'All', 2, 70.0, N'Stimulates pancreas');
    
    PRINT '  ✓ Inserted condition-treatment mappings';
END
ELSE
BEGIN
    PRINT '  ⚠ CONDITIONTREATMENTMAPPING already contains data';
END
GO

-- =============================================
-- Seed Data 7: Sample PATIENTS and TREATMENTPLANS
-- Description: Sample data for testing
-- =============================================
PRINT 'Step 7/7: Seeding sample PATIENTS and TREATMENTPLANS...';

IF NOT EXISTS (SELECT * FROM PATIENTS)
BEGIN
    DECLARE @Patient1Id BIGINT, @Patient2Id BIGINT, @Patient3Id BIGINT;
    DECLARE @KidneyId BIGINT, @ObesityId BIGINT, @HypertensionId BIGINT;
    
    -- Insert Sample Patients
    INSERT INTO PATIENTS (Name, Age, Gender, ContactNumber, Email, Prakriti, PrakritiScore)
    VALUES 
    (N'Rajesh Kumar', 45, N'Male', N'9876543210', N'rajesh.kumar@example.com', N'Vata-Pitta', N'{"vata":45,"pitta":35,"kapha":20}'),
    (N'Priya Sharma', 38, N'Female', N'9876543211', N'priya.sharma@example.com', N'Kapha', N'{"vata":20,"pitta":25,"kapha":55}'),
    (N'Amit Patel', 52, N'Male', N'9876543212', N'amit.patel@example.com', N'Pitta-Kapha', N'{"vata":25,"pitta":40,"kapha":35}');
    
    SET @Patient1Id = SCOPE_IDENTITY() - 2;
    SET @Patient2Id = SCOPE_IDENTITY() - 1;
    SET @Patient3Id = SCOPE_IDENTITY();
    
    SELECT @KidneyId = Id FROM CONDITIONS WHERE Name = N'Kidney Stones';
    SELECT @ObesityId = Id FROM CONDITIONS WHERE Name = N'Obesity';
    SELECT @HypertensionId = Id FROM CONDITIONS WHERE Name = N'Hypertension';
    
    -- Insert Sample Treatment Plans
    INSERT INTO TREATMENTPLANS (PatientId, ConditionId, Prakriti, HerbalMedicines, YogaAsanas, DietaryRecommendations, LifestyleModifications, Duration, ConfidenceScore, Explanation)
    VALUES 
    (@Patient1Id, @KidneyId, N'Vata-Pitta', 
     N'[{"medicineId":1,"name":"Gokshura","dosage":"3-6g powder twice daily"},{"medicineId":3,"name":"Varuna","dosage":"500mg-1g twice daily"}]',
     N'[{"asanaId":10,"name":"Cobra Pose","frequency":"Daily morning"},{"asanaId":11,"name":"Bridge Pose","frequency":"Daily evening"}]',
     N'[{"item":"Increase water intake to 3-4 liters daily"},{"item":"Coconut water"},{"item":"Barley water"},{"item":"Avoid tomatoes and spinach"}]',
     N'Avoid holding urine, regular exercise, reduce salt intake, avoid calcium-rich foods in excess',
     N'3 months', 82.5,
     N'Treatment plan based on Vata-Pitta constitution with focus on lithotriptic herbs and kidney-stimulating yoga poses. High confidence due to proven efficacy of Gokshura and Varuna in kidney stone management.'),
    
    (@Patient2Id, @ObesityId, N'Kapha',
     N'[{"medicineId":5,"name":"Guggulu","dosage":"500mg-1g twice daily"},{"medicineId":6,"name":"Triphala","dosage":"3-6g at bedtime"}]',
     N'[{"asanaId":8,"name":"Boat Pose","frequency":"Daily"},{"asanaId":22,"name":"Plank Pose","frequency":"Daily"},{"asanaId":19,"name":"Skull Shining Breath","frequency":"Daily morning"}]',
     N'[{"item":"Barley"},{"item":"Millet"},{"item":"Bitter gourd"},{"item":"Green tea"},{"item":"Avoid heavy, oily foods"}]',
     N'Regular exercise 45 minutes daily, avoid daytime sleep, eat light dinner before 7 PM, practice intermittent fasting',
     N'6 months', 78.0,
     N'Kapha-reducing protocol with metabolism-boosting herbs and active yoga practice. Focus on light, dry foods and regular physical activity. Confidence based on traditional Kapha management principles.'),
    
    (@Patient3Id, @HypertensionId, N'Pitta-Kapha',
     N'[{"medicineId":9,"name":"Arjuna","dosage":"500mg-1g twice daily with milk"},{"medicineId":11,"name":"Jatamansi","dosage":"1-3g powder twice daily"}]',
     N'[{"asanaId":16,"name":"Alternate Nostril Breathing","frequency":"Morning and evening 10 minutes"},{"asanaId":9,"name":"Corpse Pose","frequency":"Daily 15 minutes"}]',
     N'[{"item":"Increase fruits and vegetables"},{"item":"Reduce salt"},{"item":"Coconut water"},{"item":"Pomegranate"},{"item":"Avoid spicy foods"}]',
     N'Stress management, regular meditation, avoid anger and stress, regular sleep schedule, reduce caffeine',
     N'Ongoing', 85.0,
     N'Comprehensive cardiovascular support with Arjuna and stress-reducing Jatamansi. Pranayama practice essential for blood pressure control. High confidence due to strong evidence for Arjuna in hypertension.');
    
    PRINT '  ✓ Inserted 3 sample patients and treatment plans';
END
ELSE
BEGIN
    PRINT '  ⚠ PATIENTS already contains data';
END
GO

-- =============================================
-- Final Summary
-- =============================================
PRINT '';
PRINT '========================================';
PRINT 'Treatment Recommendation System Data Seeded Successfully!';
PRINT '========================================';
PRINT '';
PRINT 'Summary of seeded data:';
PRINT '  ✓ 20 Prakriti assessment questions';
PRINT '  ✓ 10 Common conditions';
PRINT '  ✓ 35 Herbal medicines';
PRINT '  ✓ 25 Yoga asanas';
PRINT '  ✓ 65+ Dietary items';
PRINT '  ✓ 30+ Condition-treatment mappings';
PRINT '  ✓ 3 Sample patients with treatment plans';
PRINT '';
PRINT 'Database is ready for use!';
PRINT 'Next step: Run CreateTreatmentStoredProcedures.sql';
PRINT '';
GO
