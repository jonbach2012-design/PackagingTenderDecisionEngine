# Technical Specification: Initial C# Project Skeleton

## Difficulty

Medium.

The requested change is not algorithmically complex, but it establishes the first code structure and domain contracts for the repository. The work should be careful because later scoring, import, aggregation, and UI work will depend on these names, folders, and model relationships.

## Technical Context

- Repository currently contains planning/specification documents only: `spec.md`, `plan.md`, and task artifacts.
- No existing C# solution, project files, source folders, tests, or build configuration were found.
- The root `.gitignore` has been updated before file generation to cover common generated artifacts: `bin/`, `obj/`, `.vs/`, `node_modules/`, `dist/`, `build/`, `.cache/`, `*.log`, `TestResults/`, and `coverage/`.
- The implementation should use the installed .NET SDK through the `dotnet` CLI. Target framework should be a current stable .NET target available in the local SDK; prefer `net8.0` unless the SDK only supports a newer target.
- No third-party package is required for the skeleton/domain-model step.

## Implementation Approach

Create a small solution that is runnable immediately while keeping business logic and domain models separated from the host application:

1. Create `PackagingTenderTool.sln`.
2. Create a runnable console project for the application entry point.
3. Create a class library for the core domain and future service/import logic.
4. Create a test project for domain-model smoke tests or simple construction tests.
5. Wire project references so the app and tests reference the core library.
6. Add folders for the required architectural areas:
   - `Models`
   - `Services`
   - `Import`
   - `UI`
7. Implement initial domain models only. Do not implement final scoring rules, thresholds, exclusion logic, advanced plausibility checks, Excel import, or full UI.

Recommended layout:

```text
PackagingTenderTool.sln
src/
  PackagingTenderTool.App/
    Program.cs
    UI/
  PackagingTenderTool.Core/
    Models/
    Services/
    Import/
tests/
  PackagingTenderTool.Core.Tests/
```

The console application can print a short startup message or create a minimal sample object to prove the project runs. Keep this host intentionally thin.

## Source Code Structure Changes

Files to create:

- `PackagingTenderTool.sln`
- `src/PackagingTenderTool.App/PackagingTenderTool.App.csproj`
- `src/PackagingTenderTool.App/Program.cs`
- `src/PackagingTenderTool.App/UI/.gitkeep`
- `src/PackagingTenderTool.Core/PackagingTenderTool.Core.csproj`
- `src/PackagingTenderTool.Core/Models/Tender.cs`
- `src/PackagingTenderTool.Core/Models/TenderSettings.cs`
- `src/PackagingTenderTool.Core/Models/PackagingProfile.cs`
- `src/PackagingTenderTool.Core/Models/LabelLineItem.cs`
- `src/PackagingTenderTool.Core/Models/Supplier.cs`
- `src/PackagingTenderTool.Core/Models/LineEvaluation.cs`
- `src/PackagingTenderTool.Core/Models/SupplierEvaluation.cs`
- `src/PackagingTenderTool.Core/Models/ScoreBreakdown.cs`
- `src/PackagingTenderTool.Core/Models/ManualReviewFlag.cs`
- `src/PackagingTenderTool.Core/Services/.gitkeep`
- `src/PackagingTenderTool.Core/Import/.gitkeep`
- `tests/PackagingTenderTool.Core.Tests/PackagingTenderTool.Core.Tests.csproj`
- `tests/PackagingTenderTool.Core.Tests/DomainModelTests.cs`

Existing files to modify:

- `.gitignore` if additional generated artifacts are discovered during implementation.

## Data Model and Interface Changes

Use plain C# domain classes with sensible defaults and collection properties. Suggested namespaces:

- `PackagingTenderTool.Core.Models`
- `PackagingTenderTool.Core.Services`
- `PackagingTenderTool.Core.Import`

### `PackagingProfile`

Use an enum to lock version 1 to Labels while leaving space for future profiles:

- `Labels`

Avoid implementing Trays/Cardboard behavior now.

### `TenderSettings`

Recommended properties:

- `PackagingProfile PackagingProfile`
- `string CurrencyCode`

Defaults:

- `PackagingProfile = PackagingProfile.Labels`
- `CurrencyCode = "EUR"`

Currency belongs to the tender, not line items.

### `Tender`

Recommended properties:

- `Guid Id`
- `string Name`
- `TenderSettings Settings`
- `List<LabelLineItem> LabelLineItems`

Version 1 supports one tender and one packaging profile per tender. Since Labels is the only active profile, a typed `LabelLineItems` collection is acceptable for the skeleton.

### `LabelLineItem`

Represent imported Labels profile source data. Recommended properties align to the current specification:

- `Guid Id`
- `string? ItemNo`
- `string? ItemName`
- `string? SupplierName`
- `string? Site`
- `decimal? Quantity`
- `decimal? Spend`
- `decimal? PricePerThousand`
- `decimal? Price`
- `decimal? TheoreticalSpend`
- `string? LabelSize`
- `string? WindingDirection`
- `string? Material`
- `string? ReelDiameterOrPcsPerRoll`
- `int? NumberOfColors`
- `string? Comment`

Numeric values should be nullable so missing or invalid import data can later be represented without forcing fake values.

### `Supplier`

Version 1 supplier grouping is by supplier name. Recommended properties:

- `string Name`
- `List<LabelLineItem> LineItems`

Do not introduce Supplier ID yet.

### `ScoreBreakdown`

Represent score dimensions without final threshold behavior:

- `decimal? Commercial`
- `decimal? Technical`
- `decimal? Regulatory`
- `decimal? Total`

The 30/30/40 weighting is part of the project direction, but final scoring rules are out of scope for this skeleton.

### `ManualReviewFlag`

Recommended properties:

- `Guid Id`
- `string Reason`
- `string? FieldName`
- `string? SourceValue`
- `ManualReviewSeverity Severity`

Add a small enum if useful:

- `Info`
- `Warning`
- `Error`

Manual review is non-blocking in version 1 and must not imply exclusion.

### `LineEvaluation`

Recommended properties:

- `Guid Id`
- `Guid LineItemId`
- `LabelLineItem LineItem`
- `ScoreBreakdown ScoreBreakdown`
- `List<ManualReviewFlag> ManualReviewFlags`
- `bool RequiresManualReview`

Line evaluation is the starting point for evaluation.

### `SupplierEvaluation`

Recommended properties:

- `Guid Id`
- `string SupplierName`
- `List<LineEvaluation> LineEvaluations`
- `ScoreBreakdown ScoreBreakdown`
- `decimal TotalSpend`
- `List<ManualReviewFlag> ManualReviewFlags`
- `bool RequiresManualReview`

Supplier aggregation is weighted by `Spend`, but the actual aggregation algorithm can be deferred to the later scoring/aggregation step.

## Out of Scope

Do not implement:

- final commercial, technical, or regulatory scoring rules
- classification thresholds
- recommendation outcomes
- exclusion or knockout behavior
- advanced plausibility checks
- Excel import parsing
- M3 supplier ID handling
- full UI
- additional packaging profiles beyond `Labels`

## Verification Approach

Run these commands after implementation:

```powershell
dotnet restore PackagingTenderTool.sln
dotnet build PackagingTenderTool.sln
dotnet test PackagingTenderTool.sln
dotnet run --project src/PackagingTenderTool.App/PackagingTenderTool.App.csproj
```

Expected result:

- restore succeeds
- build succeeds with no errors
- tests pass
- console app runs and exits successfully

If the local SDK does not support the initially selected target framework, adjust the project target to a locally supported stable target and document that in the implementation report.

## Follow-On Implementation Plan

Replace the generic implementation step with concrete steps:

1. Create the solution, app, core library, test project, project references, and required folders.
2. Add initial domain model classes and focused tests that verify defaults, relationships, supplier-name grouping assumptions, tender-level currency, and manual-review-capable nullable source data.
3. Run restore/build/test/run verification and write the implementation report.
