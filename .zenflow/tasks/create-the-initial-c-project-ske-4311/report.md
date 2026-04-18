# Regulatory Scoring Update

## Implementation Summary

PackagingTenderTool now includes initial regulatory scoring for Labels profile v1.

The service layer supports:

- line-level regulatory scoring in `LineEvaluationService`
- supplier-level regulatory aggregation in `SupplierAggregationService`
- total weighted score calculation through `ScoreBreakdownCalculator`
- supplier classification through `SupplierClassificationService`
- demo output from the console host with non-zero regulatory scores

The first regulatory criteria are represented as tender-level expected values and nullable supplier line values:

- lower label weight through `MaximumLabelWeightGrams` and `LabelWeightGrams`
- mono-material design
- easy separation
- reusable or recyclable material direction
- traceability

Regulatory scoring stays simple and explicit. Each configured criterion contributes equally to the line-level regulatory score. Matching values increase the score, mismatches reduce it, and missing or invalid values create manual review flags without blocking evaluation or applying hard exclusion behavior.

Supplier regulatory scores are aggregated using the existing spend-weighted supplier aggregation path. The total score continues to use the current weighting:

- Commercial: 30%
- Technical: 30%
- Regulatory: 40%

Classification uses the updated total score while preserving manual review behavior. A supplier with manual review flags is classified as `ManualReview`, even when a numeric total score can still be calculated.

## Verification

Commands run from the repository root:

```powershell
dotnet build PackagingTenderTool.sln
```

Result: passed. Build succeeded with 0 warnings and 0 errors.

```powershell
dotnet test PackagingTenderTool.sln
```

Result: passed. 41 tests passed, 0 failed, 0 skipped.

```powershell
dotnet run --project src/PackagingTenderTool.App/PackagingTenderTool.App.csproj
```

Result: passed. The console demo printed supplier scores including non-zero regulatory values:

```text
Supplier: Acme Labels
  Scores: Commercial=79.14, Technical=87.61, Regulatory=85.13, Total=84.08
  Classification: Recommended

Supplier: Beta Packaging
  Scores: Commercial=76.09, Technical=33.33, Regulatory=80, Total=64.83
  Classification: ManualReview
```

## Scope Notes

No hard exclusion or knockout behavior was added.

No advanced PPWR/EPR rules, legal/regulatory engine behavior, or UI were implemented.

Manual review remains non-blocking: flagged rows and suppliers can still receive numeric scores, but classification reflects the current manual review status.
