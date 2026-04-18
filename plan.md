# PackagingTenderTool Plan

## 1. Objective

The objective of PackagingTenderTool is to create a structured packaging tender evaluation tool that is easier to reuse, explain, and improve than a spreadsheet-only process.

Version 1 should establish:
- a stable core domain model
- a clear Labels packaging profile
- line-level evaluation
- supplier-level aggregation
- manual review handling
- a first usable scoring and ranking workflow

---

## 2. Current Status

The repository is now aligned to one local path and one Git repository.

Confirmed repository setup:
- local working path is the PackagingTenderTool repository
- Git is connected and working
- plan.md and spec.md are the current source documents

The project is still in early definition stage and does not yet contain full implementation code.

---

## 3. Confirmed Product Direction

The following decisions are now confirmed for version 1:

### 3.1 Scope
- version 1 handles one tender at a time
- version 1 uses one packaging profile per tender
- Labels is the first packaging profile

### 3.2 Evaluation structure
- evaluation starts at line level
- line results are aggregated to supplier level
- supplier aggregation is weighted by Spend

### 3.3 Supplier identity
- supplier grouping is based on Supplier name in version 1
- Supplier ID may be introduced later when M3 integration is more mature

### 3.4 Currency handling
- one currency applies per tender
- currency does not vary by line
- default currency is EUR
- tender setup may allow other currencies, such as NOK

### 3.5 Data quality handling
- missing or invalid data should trigger Manual Review
- this applies broadly in version 1 if possible
- missing or invalid data should not automatically exclude a supplier in version 1

### 3.6 Scoring structure
- Commercial: 30%
- Technical: 30%
- Regulatory: 40%

### 3.7 Commercial direction
- price must influence evaluation
- lowest price should generally result in the highest commercial score
- theoretical spend is important but not sufficient on its own

### 3.8 Regulatory direction
Regulatory has the highest weight because PPWR and EPR related issues may create significant risk not only for the supplier, but also for the buying company.

Important focus areas include:
- lower weight
- mono-material design
- easy separation
- reusable or recyclable material direction
- traceability

Regulatory criteria should be able to both:
- increase score
- reduce score

Some regulatory criteria may later become knockout rules, but not in version 1.

---

## 4. Remaining Open Decisions

The following items are not yet fully defined:

- detailed price scoring method
- detailed material scoring logic
- detailed technical scoring logic
- classification thresholds
- future exclusion / knockout rules
- future plausibility checks for suspicious supplier inputs
- possible use of Supplier ID through M3 integration

These are important, but they do not need to block the first implementation skeleton.

---

## 5. Implementation Strategy

The recommended build order is:

1. lock the current specification baseline
2. create the core project structure
3. create the domain model
4. implement line-level evaluation
5. implement supplier aggregation
6. implement manual review handling
7. test with hardcoded or sample data
8. implement Excel import
9. implement basic UI
10. add visualization such as supplier breakdown and radar chart
11. refine scoring logic
12. add later enhancements

This order reduces complexity and keeps architecture ahead of UI.

---

## 6. Phased Plan

## Phase 1 — Specification Baseline
Goal:
- establish a stable version 1 baseline for Labels

Tasks:
- confirm Labels as first packaging profile
- confirm input columns
- confirm tender-level currency handling
- confirm line-to-supplier evaluation flow
- confirm spend-weighted aggregation
- confirm manual review direction
- confirm 30/30/40 scoring model
- document open decisions clearly

Status:
- mostly completed

---

## Phase 2 — Project Skeleton
Goal:
- create the base project and technical structure

Tasks:
- create the C# solution/project
- establish folder structure
- create initial domain classes
- prepare separation between models, services, import, and UI

Expected outcome:
- a runnable empty skeleton with domain structure

---

## Phase 3 — Domain Model
Goal:
- represent the tender model cleanly in code

Candidate entities:
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

Tasks:
- define properties
- define relationships
- define basic enums/value objects where needed

Expected outcome:
- a stable domain foundation for scoring and import

---

## Phase 4 — Scoring Engine v1
Goal:
- implement the first scoring logic

Tasks:
- implement Commercial score structure
- implement placeholder Technical score structure
- implement placeholder Regulatory score structure
- implement weighted total score
- implement line-level scoring
- implement spend-weighted supplier aggregation

Expected outcome:
- first working evaluation engine without final advanced rules

---

## Phase 5 — Manual Review Engine v1
Goal:
- support safe handling of incomplete or invalid input

Tasks:
- define manual review triggers
- flag missing values
- flag invalid values
- collect review reasons
- expose review status in results

Expected outcome:
- non-blocking review behavior in line with version 1 direction

---

## Phase 6 — Sample Data Testing
Goal:
- validate architecture and scoring behavior early

Tasks:
- create sample Labels tender data
- test scoring flow
- test aggregation flow
- test manual review behavior
- test supplier grouping by Supplier name

Expected outcome:
- confidence that core model works before Excel import/UI work expands complexity

---

## Phase 7 — Excel Import
Goal:
- import Labels tender data from Excel

Tasks:
- map required columns
- validate presence of required columns
- parse rows into LabelLineItem objects
- trigger Manual Review on invalid or missing input

Expected outcome:
- first end-to-end input pipeline from Excel into domain model

---

## Phase 8 — Basic UI
Goal:
- provide a usable interface for version 1

Suggested areas:
- Tender setup
- Excel import and validation
- Supplier ranking
- Supplier detail view

Expected outcome:
- a minimal but understandable working prototype

---

## Phase 9 — Visualization
Goal:
- provide simple decision-support visuals

Tasks:
- supplier ranking table
- dimension score breakdown
- manual review visibility
- radar chart or similar overview

Expected outcome:
- clearer comparison between suppliers

---

## Phase 10 — Refinement
Goal:
- improve realism after first prototype is stable

Possible refinement topics:
- material scoring logic
- threshold logic
- classification outcomes
- plausibility checks
- future exclusion rules
- M3 supplier ID integration
- additional packaging profiles

---

## 7. Working Principles

The project should follow these principles:

- start small and stable
- architecture before interface
- line-level logic before supplier-level summary visuals
- manual review before automatic exclusion
- extensibility for future profiles
- clear business explanation for each scoring dimension

This is especially important because the tool must be understandable not only technically, but also commercially and operationally.

---

## 8. Immediate Next Step

The next practical step is:

- update spec.md and plan.md in the repository
- commit the updated baseline
- create the project skeleton
- begin the domain model

Suggested first implementation focus:
- Tender
- TenderSettings
- LabelLineItem
- LineEvaluation
- SupplierEvaluation
- ScoreBreakdown
- ManualReviewFlag

---

## 9. Suggested Commit Sequence

A sensible near-term Git sequence could be:

1. refine specification baseline
2. create project skeleton
3. add core domain models
4. add first scoring engine
5. add manual review handling
6. add sample data tests
7. add Excel import
8. add UI foundation

This gives a clean history and makes the process easier to explain later.

---

## 10. Summary

PackagingTenderTool is now ready to move from definition into structured implementation.

The most important baseline decisions for Labels version 1 are now set:
- one tender
- one packaging profile
- line-level scoring
- spend-weighted supplier aggregation
- manual review instead of early exclusion
- 30/30/40 dimension model
- regulatory weighted highest because compliance risk affects both supplier and buyer

The next step is to turn this into a project skeleton and start the domain model.