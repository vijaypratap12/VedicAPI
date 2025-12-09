-- =============================================
-- Add Sample ContentHtml to Research Papers and Thesis
-- This script adds sample HTML content to existing records
-- =============================================

USE VedicDB;
GO

PRINT '=============================================';
PRINT 'Adding Sample ContentHtml to Research Papers';
PRINT '=============================================';

-- Update Research Paper 1: Kshara Sutra Therapy
UPDATE RESEARCH_PAPERS
SET ContentHtml = N'
<h2>Introduction</h2>
<p>Fistula-in-ano is a challenging condition that has plagued surgical practice for centuries. This comprehensive clinical study evaluates the effectiveness of Kshara Sutra therapy, an ancient Ayurvedic technique, in treating anal fistula compared to conventional surgical methods.</p>

<h2>Methodology</h2>
<p>The study was conducted over 18 months at IPGT&RA Jamnagar, involving 120 patients diagnosed with fistula-in-ano. Patients were randomly divided into two groups:</p>
<ul>
  <li><strong>Group A (n=60):</strong> Treated with Kshara Sutra therapy</li>
  <li><strong>Group B (n=60):</strong> Treated with conventional fistulectomy</li>
</ul>

<h2>Results</h2>
<p>The Kshara Sutra group showed remarkable outcomes:</p>
<ul>
  <li>Complete healing rate: 93.3% (56 out of 60 patients)</li>
  <li>Average healing time: 6-8 weeks</li>
  <li>Recurrence rate: 3.3% (2 patients)</li>
  <li>Post-operative complications: Minimal</li>
  <li>Patient satisfaction: 95%</li>
</ul>

<h2>Discussion</h2>
<p>Kshara Sutra therapy offers several advantages over conventional surgery:</p>
<ol>
  <li>No need for general anesthesia</li>
  <li>Minimal post-operative pain</li>
  <li>Lower recurrence rates</li>
  <li>Better sphincter preservation</li>
  <li>Cost-effective treatment</li>
</ol>

<h2>Conclusion</h2>
<p>This study demonstrates that Kshara Sutra therapy is a highly effective, safe, and patient-friendly treatment option for fistula-in-ano. The technique combines ancient wisdom with modern clinical validation, offering a viable alternative to conventional surgical approaches.</p>

<h2>References</h2>
<ol>
  <li>Sushruta Samhita, Sutra Sthana, Chapter 11</li>
  <li>Modern Surgical Techniques in Ayurveda, 2023</li>
  <li>International Journal of Ayurvedic Surgery, Vol. 45, 2024</li>
</ol>
'
WHERE Title = N'Efficacy of Kshara Sutra Therapy in Management of Fistula-in-Ano: A Clinical Study';

PRINT '✓ Updated: Kshara Sutra Therapy paper';

-- Update Research Paper 2: Agnikarma
UPDATE RESEARCH_PAPERS
SET ContentHtml = N'
<h2>Abstract</h2>
<p>Agnikarma, the ancient practice of therapeutic cauterization, has been used in Ayurvedic medicine for over 2000 years. This research explores its application in modern pain management, particularly for chronic musculoskeletal conditions.</p>

<h2>Historical Context</h2>
<p>Sushruta, the father of surgery, described Agnikarma as a superior treatment modality that provides immediate and lasting relief from pain. The technique involves controlled application of heat to specific points on the body.</p>

<h2>Study Design</h2>
<p>A prospective study involving 80 patients with chronic pain conditions:</p>
<ul>
  <li>Chronic lower back pain (30 patients)</li>
  <li>Knee osteoarthritis (25 patients)</li>
  <li>Frozen shoulder (15 patients)</li>
  <li>Plantar fasciitis (10 patients)</li>
</ul>

<h2>Procedure</h2>
<p>The Agnikarma procedure was performed using:</p>
<ol>
  <li>Traditional metal rods (Loha Shalaka) heated to optimal temperature</li>
  <li>Precise point identification based on Marma science</li>
  <li>Quick application with immediate cooling</li>
  <li>Post-procedure care with herbal applications</li>
</ol>

<h2>Results</h2>
<table border="1" style="width:100%; border-collapse: collapse;">
  <tr>
    <th>Condition</th>
    <th>Pain Reduction</th>
    <th>Functional Improvement</th>
  </tr>
  <tr>
    <td>Lower Back Pain</td>
    <td>85%</td>
    <td>78%</td>
  </tr>
  <tr>
    <td>Knee Osteoarthritis</td>
    <td>82%</td>
    <td>75%</td>
  </tr>
  <tr>
    <td>Frozen Shoulder</td>
    <td>88%</td>
    <td>80%</td>
  </tr>
  <tr>
    <td>Plantar Fasciitis</td>
    <td>90%</td>
    <td>85%</td>
  </tr>
</table>

<h2>Safety Profile</h2>
<p>The procedure was found to be extremely safe with:</p>
<ul>
  <li>No major adverse events</li>
  <li>Minimal discomfort during procedure</li>
  <li>Rapid healing of cauterization points (3-5 days)</li>
  <li>No long-term complications</li>
</ul>

<h2>Conclusion</h2>
<p>Agnikarma represents a valuable addition to modern pain management protocols. Its effectiveness, safety, and cost-efficiency make it an excellent option for patients seeking alternatives to pharmaceutical interventions.</p>
'
WHERE Title = N'Agnikarma in Pain Management: Ancient Technique for Modern Practice';

PRINT '✓ Updated: Agnikarma paper';

-- Update Research Paper 3: Sushruta''s Principles
UPDATE RESEARCH_PAPERS
SET ContentHtml = N'
<h2>Introduction</h2>
<p>Sushruta Samhita, composed around 600 BCE, contains detailed descriptions of surgical techniques, instruments, and principles that remain relevant in modern surgical practice. This review article explores how these ancient principles can be integrated into contemporary operating theaters.</p>

<h2>Surgical Instruments</h2>
<p>Sushruta described 121 surgical instruments, many of which have modern equivalents:</p>
<ul>
  <li><strong>Shastra (Sharp Instruments):</strong> Scalpels, scissors, needles</li>
  <li><strong>Yantra (Blunt Instruments):</strong> Forceps, retractors, probes</li>
  <li><strong>Anushastra (Accessories):</strong> Catheters, syringes, tubes</li>
</ul>

<h2>Pre-operative Preparation</h2>
<p>Ancient principles that align with modern practice:</p>
<ol>
  <li>Patient assessment and counseling</li>
  <li>Proper positioning on the operating table</li>
  <li>Adequate lighting and ventilation</li>
  <li>Sterilization of instruments (using fire and boiling)</li>
  <li>Preparation of the surgical site</li>
</ol>

<h2>Surgical Techniques</h2>
<h3>Eight Types of Surgical Procedures (Ashtavidha Shastra Karma)</h3>
<ol>
  <li><strong>Chedana:</strong> Excision</li>
  <li><strong>Bhedana:</strong> Incision</li>
  <li><strong>Lekhana:</strong> Scraping</li>
  <li><strong>Vedhana:</strong> Puncturing</li>
  <li><strong>Eshana:</strong> Probing</li>
  <li><strong>Aharana:</strong> Extraction</li>
  <li><strong>Vsravana:</strong> Drainage</li>
  <li><strong>Sivana:</strong> Suturing</li>
</ol>

<h2>Wound Management</h2>
<p>Sushruta''s wound classification and management principles:</p>
<ul>
  <li>Classification based on causative factors</li>
  <li>Proper wound cleaning and debridement</li>
  <li>Application of herbal dressings</li>
  <li>Prevention of infection</li>
  <li>Promotion of healing</li>
</ul>

<h2>Modern Applications</h2>
<p>Contemporary surgeons can benefit from:</p>
<ul>
  <li>Holistic patient assessment</li>
  <li>Emphasis on minimal tissue trauma</li>
  <li>Natural wound healing promotion</li>
  <li>Integration of herbal antimicrobials</li>
  <li>Patient-centered care approach</li>
</ul>

<h2>Conclusion</h2>
<p>The surgical principles outlined in Sushruta Samhita demonstrate remarkable foresight and scientific understanding. Modern surgeons can enhance their practice by integrating these time-tested principles with contemporary techniques.</p>
'
WHERE Title = N'Modern Integration of Sushruta''s Surgical Principles in Contemporary Operating Theaters';

PRINT '✓ Updated: Sushruta''s Principles paper';

PRINT '';
PRINT '=============================================';
PRINT 'Adding Sample ContentHtml to Thesis';
PRINT '=============================================';

-- Update Thesis 1: AI-Assisted Diagnosis
UPDATE THESIS_REPOSITORY
SET ContentHtml = N'
<h1>AI-Assisted Diagnosis in Ayurvedic Surgery</h1>

<h2>Chapter 1: Introduction</h2>
<p>The integration of Artificial Intelligence (AI) in medical diagnosis has revolutionized healthcare delivery. This thesis explores the application of AI technologies in Ayurvedic surgical diagnosis, creating a bridge between ancient wisdom and modern technology.</p>

<h3>1.1 Background</h3>
<p>Ayurvedic surgery relies heavily on clinical examination and practitioner expertise. The integration of AI can enhance diagnostic accuracy, reduce human error, and provide decision support to practitioners.</p>

<h3>1.2 Research Objectives</h3>
<ul>
  <li>Develop an AI model for diagnosing common surgical conditions in Ayurveda</li>
  <li>Validate the model against expert Ayurvedic practitioners</li>
  <li>Assess the practical applicability in clinical settings</li>
  <li>Evaluate patient outcomes and satisfaction</li>
</ul>

<h2>Chapter 2: Literature Review</h2>
<p>Comprehensive review of existing literature on:</p>
<ul>
  <li>AI applications in medical diagnosis</li>
  <li>Traditional Ayurvedic diagnostic methods</li>
  <li>Integration challenges and opportunities</li>
  <li>Previous attempts at technology integration in Ayurveda</li>
</ul>

<h2>Chapter 3: Methodology</h2>
<h3>3.1 Data Collection</h3>
<p>Clinical data from 500 patients collected over 24 months including:</p>
<ul>
  <li>Patient demographics and medical history</li>
  <li>Clinical examination findings</li>
  <li>Diagnostic test results</li>
  <li>Final diagnosis and treatment outcomes</li>
</ul>

<h3>3.2 AI Model Development</h3>
<p>Development of machine learning models using:</p>
<ul>
  <li>Neural networks for pattern recognition</li>
  <li>Decision trees for diagnostic pathways</li>
  <li>Natural language processing for symptom analysis</li>
  <li>Image recognition for physical examination findings</li>
</ul>

<h2>Chapter 4: Results</h2>
<h3>4.1 Model Performance</h3>
<table border="1" style="width:100%; border-collapse: collapse;">
  <tr>
    <th>Metric</th>
    <th>Value</th>
  </tr>
  <tr>
    <td>Diagnostic Accuracy</td>
    <td>92.5%</td>
  </tr>
  <tr>
    <td>Sensitivity</td>
    <td>89.3%</td>
  </tr>
  <tr>
    <td>Specificity</td>
    <td>94.7%</td>
  </tr>
  <tr>
    <td>Positive Predictive Value</td>
    <td>91.2%</td>
  </tr>
</table>

<h3>4.2 Clinical Validation</h3>
<p>Comparison with expert practitioners showed:</p>
<ul>
  <li>Agreement rate: 88%</li>
  <li>AI identified 12% additional diagnostic considerations</li>
  <li>Reduced diagnostic time by 40%</li>
  <li>Enhanced documentation quality</li>
</ul>

<h2>Chapter 5: Discussion</h2>
<p>The AI-assisted diagnostic system demonstrates significant potential in enhancing Ayurvedic surgical practice. Key findings include:</p>
<ol>
  <li>High diagnostic accuracy comparable to expert practitioners</li>
  <li>Ability to process complex multi-factorial diagnostic criteria</li>
  <li>Consistent performance across different conditions</li>
  <li>Valuable decision support for less experienced practitioners</li>
</ol>

<h2>Chapter 6: Conclusion</h2>
<p>This research successfully demonstrates that AI can be effectively integrated into Ayurvedic surgical diagnosis without compromising the fundamental principles of traditional medicine. The system serves as a valuable tool for enhancing diagnostic accuracy and supporting clinical decision-making.</p>

<h2>Future Scope</h2>
<ul>
  <li>Expansion to include more surgical conditions</li>
  <li>Integration with electronic health records</li>
  <li>Development of mobile applications for point-of-care use</li>
  <li>Continuous learning from new clinical data</li>
</ul>

<h2>References</h2>
<ol>
  <li>Artificial Intelligence in Medicine, 2023</li>
  <li>Sushruta Samhita: Modern Interpretation, 2022</li>
  <li>Machine Learning in Healthcare, 2024</li>
  <li>Traditional Medicine and Technology Integration, 2023</li>
</ol>
'
WHERE Title = N'AI-Assisted Diagnosis in Ayurvedic Surgery';

PRINT '✓ Updated: AI-Assisted Diagnosis thesis';

-- Update Thesis 2: Minimally Invasive Techniques
UPDATE THESIS_REPOSITORY
SET ContentHtml = N'
<h1>Minimally Invasive Techniques in Ayurvedic Surgery: A Modern Approach</h1>

<h2>Abstract</h2>
<p>This thesis explores the adaptation of minimally invasive surgical techniques within the framework of Ayurvedic surgery. The research demonstrates how traditional Ayurvedic principles can be enhanced with modern minimally invasive approaches to improve patient outcomes.</p>

<h2>Chapter 1: Introduction</h2>
<h3>1.1 Evolution of Surgical Techniques</h3>
<p>Surgery has evolved from large open procedures to minimally invasive techniques. This thesis examines how Ayurvedic surgery can benefit from this evolution while maintaining its core principles.</p>

<h3>1.2 Objectives</h3>
<ul>
  <li>Identify Ayurvedic procedures suitable for minimally invasive adaptation</li>
  <li>Develop protocols for minimally invasive Ayurvedic surgery</li>
  <li>Compare outcomes with traditional approaches</li>
  <li>Assess patient satisfaction and recovery times</li>
</ul>

<h2>Chapter 2: Theoretical Framework</h2>
<h3>2.1 Ayurvedic Surgical Principles</h3>
<p>Core principles that guide the adaptation:</p>
<ul>
  <li>Minimal tissue trauma (Alpa Shastra Karma)</li>
  <li>Preservation of vital structures (Marma Raksha)</li>
  <li>Natural healing promotion (Prakruti Anuvartana)</li>
  <li>Holistic patient care (Sarva Anga Chikitsa)</li>
</ul>

<h2>Chapter 3: Methodology</h2>
<h3>3.1 Study Design</h3>
<p>Prospective comparative study involving 200 patients:</p>
<ul>
  <li>Group A: Minimally invasive techniques (n=100)</li>
  <li>Group B: Traditional open procedures (n=100)</li>
</ul>

<h3>3.2 Procedures Studied</h3>
<ol>
  <li>Laparoscopic approach to Bhagandar (fistula)</li>
  <li>Endoscopic Kshara Sutra placement</li>
  <li>Minimally invasive Arbuda (tumor) excision</li>
  <li>Arthroscopic joint procedures</li>
</ol>

<h2>Chapter 4: Results</h2>
<h3>4.1 Surgical Outcomes</h3>
<table border="1" style="width:100%; border-collapse: collapse;">
  <tr>
    <th>Parameter</th>
    <th>Minimally Invasive</th>
    <th>Traditional</th>
  </tr>
  <tr>
    <td>Average Surgery Time</td>
    <td>45 minutes</td>
    <td>75 minutes</td>
  </tr>
  <tr>
    <td>Blood Loss</td>
    <td>Minimal (&lt;50ml)</td>
    <td>Moderate (100-200ml)</td>
  </tr>
  <tr>
    <td>Hospital Stay</td>
    <td>1-2 days</td>
    <td>4-5 days</td>
  </tr>
  <tr>
    <td>Return to Work</td>
    <td>7-10 days</td>
    <td>21-28 days</td>
  </tr>
  <tr>
    <td>Complication Rate</td>
    <td>3%</td>
    <td>8%</td>
  </tr>
</table>

<h3>4.2 Patient Satisfaction</h3>
<p>Patient satisfaction scores (scale 1-10):</p>
<ul>
  <li>Minimally Invasive Group: 9.2</li>
  <li>Traditional Group: 7.8</li>
</ul>

<h2>Chapter 5: Discussion</h2>
<p>The integration of minimally invasive techniques with Ayurvedic surgery offers several advantages:</p>
<ol>
  <li><strong>Reduced Trauma:</strong> Aligns with Ayurvedic principle of minimal tissue damage</li>
  <li><strong>Faster Recovery:</strong> Patients return to normal activities sooner</li>
  <li><strong>Better Cosmesis:</strong> Smaller scars improve aesthetic outcomes</li>
  <li><strong>Lower Complications:</strong> Reduced risk of infection and other complications</li>
  <li><strong>Cost-Effective:</strong> Shorter hospital stays reduce overall costs</li>
</ol>

<h2>Chapter 6: Conclusion</h2>
<p>This research demonstrates that minimally invasive techniques can be successfully integrated into Ayurvedic surgical practice. The approach maintains the fundamental principles of Ayurveda while leveraging modern technology to improve patient outcomes.</p>

<h2>Recommendations</h2>
<ul>
  <li>Training programs for Ayurvedic surgeons in minimally invasive techniques</li>
  <li>Development of specialized instruments for Ayurvedic procedures</li>
  <li>Establishment of centers of excellence for minimally invasive Ayurvedic surgery</li>
  <li>Further research on long-term outcomes</li>
</ul>
'
WHERE Title = N'Minimally Invasive Techniques in Ayurvedic Surgery: A Modern Approach';

PRINT '✓ Updated: Minimally Invasive Techniques thesis';

PRINT '';
PRINT '=============================================';
PRINT 'Sample ContentHtml Added Successfully!';
PRINT '=============================================';
PRINT '';
PRINT 'Summary:';
PRINT '  - 3 Research Papers updated with HTML content';
PRINT '  - 2 Thesis updated with HTML content';
PRINT '';
PRINT 'You can now view full content in the Research page!';
PRINT '';

GO

