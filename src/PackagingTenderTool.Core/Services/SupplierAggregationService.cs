using PackagingTenderTool.Core.Models;

namespace PackagingTenderTool.Core.Services;

public sealed class SupplierAggregationService
{
    public IReadOnlyList<SupplierEvaluation> AggregateBySupplierName(IEnumerable<LineEvaluation> lineEvaluations)
    {
        ArgumentNullException.ThrowIfNull(lineEvaluations);

        return lineEvaluations
            .GroupBy(evaluation => evaluation.LineItem.SupplierName ?? string.Empty)
            .Select(CreateSupplierEvaluation)
            .OrderBy(evaluation => evaluation.SupplierName, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static SupplierEvaluation CreateSupplierEvaluation(IGrouping<string, LineEvaluation> supplierGroup)
    {
        var lineEvaluations = supplierGroup.ToList();
        var totalSpend = lineEvaluations.Sum(GetValidSpend);

        var supplierEvaluation = new SupplierEvaluation
        {
            SupplierName = supplierGroup.Key,
            LineEvaluations = lineEvaluations,
            TotalSpend = totalSpend,
            ScoreBreakdown = CreateWeightedScoreBreakdown(lineEvaluations, totalSpend, out var aggregationFlags)
        };

        foreach (var manualReviewFlag in lineEvaluations.SelectMany(line => line.ManualReviewFlags))
        {
            supplierEvaluation.ManualReviewFlags.Add(manualReviewFlag);
        }

        foreach (var flag in aggregationFlags)
        {
            supplierEvaluation.ManualReviewFlags.Add(flag);
        }

        return supplierEvaluation;
    }

    private static ScoreBreakdown CreateWeightedScoreBreakdown(
        IReadOnlyCollection<LineEvaluation> lineEvaluations,
        decimal totalSpend,
        out List<ManualReviewFlag> aggregationFlags)
    {
        aggregationFlags = [];
        if (totalSpend <= 0m)
        {
            return new ScoreBreakdown();
        }

        var scoreBreakdown = new ScoreBreakdown
        {
            Commercial = WeightedAverage(lineEvaluations, totalSpend, score => score.Commercial),
            Technical = WeightedAverage(lineEvaluations, totalSpend, score => score.Technical),
            Regulatory = WeightedAverage(lineEvaluations, totalSpend, score => score.Regulatory)
        };

        var weightedTotal = WeightedAverage(lineEvaluations, totalSpend, score => score.Total);
        scoreBreakdown.Total = weightedTotal;

        if (weightedTotal is null)
        {
            aggregationFlags.Add(new ManualReviewFlag
            {
                FieldName = nameof(ScoreBreakdown.Total),
                Reason = "Supplier total score could not be calculated for one or more lines (missing spend or line score).",
                Severity = ManualReviewSeverity.Warning
            });
        }

        return scoreBreakdown;
    }

    private static decimal? WeightedAverage(
        IEnumerable<LineEvaluation> lineEvaluations,
        decimal totalSpend,
        Func<ScoreBreakdown, decimal?> scoreSelector)
    {
        decimal weightedScore = 0m;
        decimal includedSpend = 0m;

        foreach (var lineEvaluation in lineEvaluations)
        {
            var spend = GetValidSpend(lineEvaluation);
            if (spend <= 0m)
            {
                continue;
            }

            var score = scoreSelector(lineEvaluation.ScoreBreakdown);
            if (score is null)
            {
                continue;
            }

            weightedScore += score.Value * spend;
            includedSpend += spend;
        }

        if (includedSpend <= 0m)
        {
            return null;
        }

        return decimal.Round(weightedScore / includedSpend, 2);
    }

    private static decimal GetValidSpend(LineEvaluation lineEvaluation)
    {
        return lineEvaluation.LineItem.Spend is >= 0m
            ? lineEvaluation.LineItem.Spend.Value
            : 0m;
    }
}
