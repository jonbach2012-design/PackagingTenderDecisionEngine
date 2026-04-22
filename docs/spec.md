# PackagingTenderTool Specification

## 1. Purpose

PackagingTenderTool is intended to support structured evaluation of packaging tenders in a way that is more reusable, transparent, and explainable than a spreadsheet-only process.

The solution should help transform tender input data into:
- validated and normalized line data
- structured supplier evaluation
- analytics and summary outputs
- reusable frontend-ready models for later UI presentation

Version 1 focuses on one packaging profile at a time, with **Labels** as the first supported profile.

---

## 2. Version 1 Scope

Version 1 includes:

- one tender at a time
- one packaging profile per tender
- Labels as first packaging profile
- line-level evaluation
- supplier-level aggregation
- spend-weighted supplier comparison
- manual review handling
- Excel import for tender input
- validation and cleaning of imported data
- analytics outputs based on imported and cleaned data
- reusable output models for future frontend use

Version 1 does **not** require:
- multiple packaging profiles in the same tender
- final advanced scoring logic for all dimensions
- knockout/exclusion rules
- M3-based supplier identity
- a completed modern frontend

---

## 3. Current Implementation Direction

The current implementation direction is:

- keep WinForms only as a temporary prototype/demo shell
- move core value into reusable architecture
- prepare the system for a future **Blazor frontend with Radzen**
- prioritize business logic, import, analytics, filters, export, and frontend-ready models over GUI cosmetics

This means the specification should not assume that WinForms is the long-term UI target.

---

## 4. Main Use Case

A user should be able to:

1. create or open a tender context
2. import Labels tender data from Excel
3. validate and parse the data
4. identify invalid, missing, or suspicious data
5. normalize the imported values
6. evaluate tender data at line level
7. aggregate results at supplier level
8. calculate analytics and summary outputs
9. expose results in a form that can later be presented in a richer frontend

---

## 5. Core Business Direction

The business direction remains:

- supplier evaluation starts at line level
- line-level results roll up to supplier level
- spend is important in aggregation
- missing or invalid data should trigger Manual Review rather than early automatic exclusion
- scoring should remain explainable
- decision support must be understandable both technically and commercially

---

## 6. Packaging Profile

### 6.1 Version 1 Packaging Profile
Version 1 supports:
- **Labels**

Additional packaging profiles may be introduced later, such as:
- trays
- cardboard
- other packaging formats

### 6.2 Packaging Profile Role
A packaging profile defines:
- relevant input fields
- validation rules
- scoring logic direction
- interpretation of technical and regulatory criteria

---

## 7. Input Data

## 7.1 Input Source
Version 1 uses Excel input as the main source for tender data.

The uploaded tender file should be treated as the primary real-world data reference for the current development direction.

## 7.2 Expected Input Characteristics
The system should support structured tender rows with fields such as:
- item number
- item name
- supplier name
- site / country / business location where relevant
- quantity
- spend
- price / theoretical spend related values
- label size
- material
- reel / roll information where relevant
- color-related fields
- free-text comments where useful

Exact column names may vary and should be validated explicitly by the import layer.

## 7.3 Detail Rows vs Summary Rows
The import process must distinguish between:
- detailed tender rows
- summary or report rows inside the same file

Summary blocks must not be treated as normal evaluation lines unless explicitly used for validation or comparison purposes.

---

## 8. Import and Validation

## 8.1 Import Goals
The import layer should:
- read tender rows from Excel
- validate required columns
- validate field formats and datatypes
- parse rows into raw import models
- report issues clearly
- support a path from raw rows to cleaned domain rows

## 8.2 Import Result Requirements
The import result should support reporting of:
- rows imported
- valid rows
- invalid rows
- skipped rows
- supplier count
- site count
- size count
- material count
- total spend where relevant

## 8.3 Data Quality Handling
Missing or invalid data should:
- trigger Manual Review where appropriate
- be captured as import issues
- not automatically exclude a supplier in version 1 unless a later rule explicitly requires that

## 8.4 Manual Review
Manual Review should be used for:
- missing required values
- invalid values
- uncertain interpretation
- suspicious but non-blocking data patterns

Manual Review is intended as a safety mechanism, not as a final decision on supplier exclusion.

---

## 9. Data Layers

The solution should keep the following data layers distinct.

## 9.1 Raw Import Data
Represents rows as read from the source file with minimal transformation.

Purpose:
- preserve imported structure
- support diagnostics
- isolate parsing concerns

## 9.2 Cleaned / Normalized Domain Data
Represents validated and normalized business data used for evaluation.

Purpose:
- standardize values
- reduce noise from import format differences
- provide consistent input to scoring and analytics

## 9.3 Analytics / Summary Results
Represents aggregated outputs and decision-support metrics.

Purpose:
- support ranking, comparison, and insight generation
- support later export and presentation

## 9.4 Frontend-ready View Models
Represents reusable output structures that later can be bound to a Blazor/Radzen UI.

Purpose:
- avoid coupling analytics directly to WinForms
- support later dashboard, table, and filter views

---

## 10. Normalization Rules

Version 1 should normalize where practical:

- label size values
- material names
- color-related values
- number formats
- spend and monetary fields
- site/country naming where useful

Normalization should be:
- conservative
- explainable
- testable

The system should not aggressively invent interpretations when source data is unclear.

---

## 11. Domain Model Direction

The domain model should support at least the following concepts:

- Tender
- TenderSettings
- PackagingProfile
- LabelLineItem
- Supplier
- LineEvaluation
- SupplierEvaluation
- ScoreBreakdown
- ManualReviewFlag
- TenderEvaluationResult

Supporting or adjacent models may include:
- raw import row models
- cleaned line item models
- import summary / issue models
- analytics summary models
- dashboard / output view models

The exact class design may evolve, but the responsibility boundaries should remain clear.

---

## 12. Evaluation Structure

## 12.1 Line-Level Evaluation
Evaluation begins at line level.

Each line may contribute:
- commercial interpretation
- technical interpretation
- regulatory interpretation
- manual review signals
- spend-weighted contribution to supplier-level result

## 12.2 Supplier-Level Aggregation
Supplier results are built by aggregating line results.

Aggregation should:
- group by Supplier name in version 1
- weight by Spend where relevant
- preserve visibility of issues and review flags

## 12.3 Supplier Identity
Version 1 groups by Supplier name.

Later versions may support:
- Supplier ID
- M3 integration
- stronger supplier master data identity handling

---

## 13. Scoring Direction

## 13.1 Baseline Weights
Version 1 scoring direction remains:

- Commercial: 30%
- Technical: 30%
- Regulatory: 40%

## 13.2 Commercial
Commercial scoring should reflect:
- price significance
- theoretical spend relevance
- relative competitiveness of supplier offers

Lowest price should generally lead to the strongest commercial direction, but exact formula details may evolve.

## 13.3 Technical
Technical scoring remains version 1 placeholder / evolving logic.

Expected future direction may include:
- fit to technical requirements
- print / material / format suitability
- practical specification fit

## 13.4 Regulatory
Regulatory has the highest baseline weight because non-compliance may affect both supplier and buyer.

Important direction includes:
- lower weight
- mono-material direction
- ease of separation
- recyclability or reusable direction
- traceability

Regulatory criteria should be able to both:
- increase score
- reduce score

Version 1 does not require final knockout rules.

---

## 14. Classification Direction

Supplier classification should remain explainable and may include states such as:
- Recommended
- Conditional
- Manual Review

The final thresholds and classification logic are still open for further refinement.

Classification should never hide the reasons behind the outcome.

---

## 15. Analytics Outputs

The system should support analytics such as:

- spend by supplier
- spend by country
- spend by site
- spend by material
- spend by size
- top spend items
- outlier candidates
- consolidation / standardization candidates
- import issue summary
- manual review / flags summary

These outputs are part of the product direction and should not be considered optional decoration.

---

## 16. Planned Data Surfaces / Future Screens

The following output surfaces should be supportable by reusable models:

- Import summary
- Supplier overview
- Country breakdown
- Site breakdown
- Material breakdown
- Item/detail table
- Flags/issues table

These are future-facing UI/data surfaces and should be supported by services and view models even if the current WinForms shell only exposes them partially.

---

## 17. Filtering Direction

A reusable filtering model should support future filtering by:
- supplier
- country
- site
- material
- size
- flagged only
- outliers only

Filtering should be implemented in a way that is reusable by a future Blazor/Radzen frontend.

---

## 18. Export Direction

The solution should support export-ready outputs for:
- cleaned data
- analytics summary
- flags/issues report

CSV is sufficient as an initial practical direction.

Export logic should be reusable and not tightly coupled to the current WinForms shell.

---

## 19. Demo / Synthetic Data

If synthetic suppliers are required for demonstration:
- they may be named `Fiktiv1`, `Fiktiv2`, `Fiktiv3`
- they must be clearly synthetic
- they should be based on realistic transformations of actual imported data patterns
- they should not be random filler disconnected from the real structure

---

## 20. Non-Functional Requirements

The solution should be:
- understandable
- testable
- explainable
- reusable
- extensible for future packaging profiles
- robust enough for inconsistent tender input
- suitable for further UI evolution

The architecture should prioritize:
- separation of concerns
- reusable services
- controlled data flow
- limited UI coupling

---

## 21. Testing Direction

Automated tests should continue to cover:
- domain model behavior
- import and validation
- cleaning and normalization
- evaluation logic
- analytics outputs
- frontend-ready view-model creation

Testing should remain focused on business logic and reusable outputs rather than fragile UI-specific behavior.

---

## 22. Open Decisions

The following remain open:
- detailed price scoring formula
- detailed technical scoring logic
- detailed material scoring logic
- classification thresholds
- knockout / exclusion rules
- plausibility checks for suspicious inputs
- exact supplier master-data identity strategy
- exact future Blazor navigation/layout composition

These should be documented clearly and refined incrementally.

---

## 23. Summary

PackagingTenderTool version 1 is a Labels-focused tender evaluation solution built around:

- one tender at a time
- one packaging profile at a time
- line-level evaluation
- spend-weighted supplier aggregation
- manual review instead of early exclusion
- 30/30/40 scoring direction
- import, validation, cleaning, and analytics
- reusable models for future Blazor + Radzen presentation

The specification should continue to support implementation decisions that strengthen business value, explainability, reuse, and frontend readiness rather than further investment in temporary GUI cosmetics.