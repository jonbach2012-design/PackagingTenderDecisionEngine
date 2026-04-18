# PackagingTenderTool Specification

## 1. Purpose

PackagingTenderTool is a decision-support application for packaging tenders.

The purpose of the tool is to help evaluate and compare supplier bids in a more structured, transparent, and reusable way than a traditional spreadsheet-only process.

Version 1 is focused on:
- one tender at a time
- one packaging profile per tender
- Excel-based input
- structured evaluation at line level
- aggregated supplier-level scoring
- ranking and recommendation support

The tool is not intended to fully replace procurement judgment. It is intended to support it with a more systematic evaluation model.

---

## 2. Version 1 Scope

Version 1 includes:
- one tender at a time
- one selected packaging profile per tender
- Excel import
- line-level evaluation
- supplier-level aggregation
- weighted scoring model
- manual review handling for missing or invalid data
- supplier ranking
- score breakdown
- basic recommendation support
- simple visual output such as radar chart

Version 1 does not yet include:
- final exclusion logic
- advanced plausibility checks
- full M3 supplier ID integration
- multi-profile tenders in the same run
- advanced regulatory rules engine
- final classification threshold logic

---

## 3. Packaging Profile Strategy

Version 1 starts with one packaging profile:

- Labels

The intention is to establish the core model using Labels first, and later extend the same architecture to additional packaging profiles such as:
- Trays
- Cardboard

Each tender in version 1 uses one packaging profile only.

---

## 4. Labels Profile v1

### 4.1 Input columns

The Labels profile version 1 uses the following Excel input columns:

- Item no
- Item name
- Supplier name
- Site
- Quantity
- Spend
- Price per 1,000
- Price
- Theoretical spend
- Label size
- Winding direction
- Material
- Reel diameter / pcs per roll
- No. of colors
- Comment

### 4.2 Tender settings

Each tender has its own settings.

For Labels version 1, the following settings apply:

- Packaging type: Labels
- One currency per tender
- Default currency: EUR
- Currency can be changed during tender setup, for example to NOK
- Currency applies at tender level, not line level

### 4.3 Supplier identification

In version 1, supplier grouping is based on:

- Supplier name

Later versions may introduce Supplier ID when M3 integration is more mature.

---

## 5. Evaluation Structure

### 5.1 Evaluation flow

Evaluation starts at line level.

Each imported line is evaluated individually first.

After line-level evaluation, results are aggregated to supplier level.

Supplier-level aggregation is weighted by:

- Spend

This means suppliers with higher spend impact more heavily on the aggregated result than low-spend lines.

### 5.2 Evaluation levels

The model therefore has two main evaluation levels:

#### Line level
Used to evaluate:
- commercial attractiveness per line
- technical fit per line
- regulatory/sustainability fit per line
- missing data or invalid data
- manual review triggers

#### Supplier level
Used to evaluate:
- aggregated supplier score
- aggregated score breakdown
- ranking across suppliers
- overall recommendation support

---

## 6. Data Quality Handling

### 6.1 Version 1 direction

Missing or invalid data should trigger:

- Manual Review

This applies broadly in version 1 to all relevant fields where possible.

Examples may include:
- missing values
- invalid number formats
- unexpected text in numeric fields
- incomplete technical values
- unclear or unusable input data

### 6.2 Non-blocking approach

In version 1:
- missing or invalid data does not automatically exclude a supplier
- missing or invalid data should not automatically stop the full evaluation
- instead, the line or supplier should be flagged for manual review

This is done to allow model learning and testing before stricter rules are introduced.

---

## 7. Scoring Model

## 7.1 Main dimensions

The Labels evaluation model version 1 is divided into three main dimensions:

- Commercial: 30%
- Technical: 30%
- Regulatory: 40%

## 7.2 Rationale for weighting

Regulatory has the highest weight because PPWR and EPR related conditions may create major financial, operational, and compliance risk.

This risk may affect:
- the supplier
- the buying company
- future packaging viability
- market access

This means the evaluation must not focus on direct price alone.

A low-cost offer may still be weak overall if it creates:
- additional compliance costs
- packaging redesign risk
- poor recyclability
- poor future fit with packaging requirements
- higher total business risk

---

## 8. Commercial Direction

Commercial scoring must have significant impact.

Price should matter clearly in the model.

The general direction for version 1 is:

- lowest price should result in the highest commercial score

This should be considered:
- at line level
- at aggregated supplier level

The following commercial inputs are relevant:
- Spend
- Price per 1,000
- Price
- Theoretical spend

Theoretical spend is important, but it is not sufficient on its own for final evaluation.

Detailed price scoring method beyond this overall principle is still to be refined later.

---

## 9. Technical Direction

Technical scoring should reflect whether a supplier’s label solution fits the actual packaging and operational requirements.

Relevant technical fields may include:
- Label size
- Winding direction
- Material
- Reel diameter / pcs per roll
- No. of colors

The detailed technical scoring logic is still to be clarified later.

Version 1 should be designed so technical scoring criteria can be extended without changing the overall architecture.

---

## 10. Regulatory Direction

Regulatory and sustainability-related conditions must influence the evaluation strongly.

Material differences are important because the best business choice is not always the lowest direct price option.

EPR fees, PPWR-related requirements, recyclability implications, and traceability may materially change the final value of a supplier offer.

### 10.1 Important regulatory focus areas for Labels v1

Important regulatory and sustainability-related focus areas include:

- lower weight
- mono-material design
- easy separation
- reusable or recyclable material direction
- traceability

### 10.2 Score behavior

Regulatory criteria should be able to:
- increase score
- reduce score

This means regulatory assessment is not only about penalties. Strong future-fit solutions should also receive positive recognition.

### 10.3 Future direction

Some regulatory criteria may later become:
- hard exclusions
- knockout rules

But not in version 1.

---

## 11. Manual Review and Future Validation

### 11.1 Manual review in version 1

Manual Review is the main safeguard in version 1 for:
- missing data
- invalid data
- unclear values

### 11.2 Future plausibility checks

A later version should support plausibility checks for possible misunderstandings or unrealistic supplier input.

Examples may include:
- unusually low or high prices
- inconsistent relationships between quantity, spend, and price
- suspicious material values
- major deviations compared with other supplier bids
- possible misunderstanding of tender requirements

This is a future enhancement and not a blocking mechanism in version 1.

---

## 12. Expected Output

Version 1 should provide output at both line level and supplier level.

### 12.1 Line-level output
Examples:
- line score
- dimension breakdown
- manual review flag
- review reason(s)

### 12.2 Supplier-level output
Examples:
- aggregated supplier score
- score breakdown by dimension
- supplier ranking
- recommendation support
- manual review summary
- radar chart or similar simple visual

---

## 13. Recommendation and Classification

The tool should support recommendation logic, but final threshold definitions are still open.

Classification logic is expected later to include outcomes such as:
- Recommended
- Conditional
- Not Recommended
- Excluded

However, detailed thresholds and conditions are not yet defined in version 1.

---

## 14. Domain Direction

The expected domain model for version 1 may include concepts such as:

- Tender
- TenderSettings
- PackagingProfile
- LabelLineItem
- Supplier
- LineEvaluation
- SupplierEvaluation
- ScoreBreakdown
- ManualReviewFlag
- ClassificationResult

Names may change during implementation, but the architecture should support clear separation between:
- tender setup
- imported source data
- scoring logic
- aggregation logic
- review handling
- output generation

---

## 15. Open Decisions

The following areas remain open and will be clarified later:

- exact price scoring method beyond the general lowest-price direction
- exact material scoring logic
- detailed technical scoring definition
- classification thresholds
- future exclusion rules
- future plausibility checks
- possible supplier ID support through M3 integration

---

## 16. Summary

PackagingTenderTool version 1 starts with a Labels profile and a structured evaluation model.

The model evaluates suppliers:
- first at line level
- then at supplier level
- using spend-weighted aggregation

The model is built around three dimensions:
- Commercial
- Technical
- Regulatory

Regulatory is weighted highest because compliance and packaging sustainability requirements may create major downstream risk for both supplier and buyer.

Version 1 is intentionally conservative:
- missing or invalid data leads to Manual Review
- not automatic exclusion
- exclusion and advanced anomaly logic can be introduced later