using PackagingTenderTool.Core.Models;

namespace PackagingTenderTool.Core.Analytics;

public sealed class TenderAnalyticsService
{
    public TenderAnalyticsSummary Analyze(IEnumerable<CleanedLabelLineItem> cleanedRows)
    {
        ArgumentNullException.ThrowIfNull(cleanedRows);

        var rows = cleanedRows
            .Where(row => row.Source.Spend is > 0)
            .ToList();
        var totalSpend = rows.Sum(row => row.Source.Spend!.Value);

        return new TenderAnalyticsSummary
        {
            TotalSpend = totalSpend,
            ItemCount = rows.Count,
            SpendByCountry = BuildBreakdown(rows, row => row.Country),
            SpendBySite = BuildBreakdown(rows, row => row.Source.Site),
            SpendByLabelSize = BuildBreakdown(rows, row => row.NormalizedLabelSize),
            SpendByMaterial = BuildBreakdown(rows, row => row.NormalizedMaterial),
            TopSpendItems = BuildTopSpendItems(rows),
            PriceOutlierCandidates = BuildPriceOutlierCandidates(rows),
            ConsolidationCandidates = BuildConsolidationCandidates(rows)
        };
    }

    private static List<SpendBreakdownItem> BuildBreakdown(
        IReadOnlyCollection<CleanedLabelLineItem> rows,
        Func<CleanedLabelLineItem, string?> keySelector)
    {
        var totalSpend = rows.Sum(row => row.Source.Spend!.Value);
        return rows
            .GroupBy(row => NormalizeGroupName(keySelector(row)))
            .Select(group =>
            {
                var spend = group.Sum(row => row.Source.Spend!.Value);
                return new SpendBreakdownItem
                {
                    Name = group.Key,
                    Spend = spend,
                    ShareOfTotal = totalSpend == 0m ? 0m : Math.Round(spend / totalSpend * 100m, 2),
                    ItemCount = group.Count()
                };
            })
            .OrderByDescending(item => item.Spend)
            .ThenBy(item => item.Name)
            .ToList();
    }

    private static List<TopSpendItem> BuildTopSpendItems(IEnumerable<CleanedLabelLineItem> rows)
    {
        return rows
            .OrderByDescending(row => row.Source.Spend)
            .Take(10)
            .Select(row => new TopSpendItem
            {
                ItemNo = row.Source.ItemNo,
                ItemName = row.Source.ItemName,
                SupplierName = row.Source.SupplierName,
                Site = row.Source.Site,
                LabelSize = row.NormalizedLabelSize,
                Spend = row.Source.Spend!.Value
            })
            .ToList();
    }

    private static List<PriceOutlierCandidate> BuildPriceOutlierCandidates(IReadOnlyCollection<CleanedLabelLineItem> rows)
    {
        return rows
            .Where(row => row.Source.PricePerThousand is > 0
                && !string.IsNullOrWhiteSpace(row.NormalizedLabelSize))
            .GroupBy(row => $"{row.NormalizedLabelSize}|{row.NormalizedMaterial}")
            .Where(group => group.Count() >= 3)
            .SelectMany(group =>
            {
                var median = Median(group.Select(row => row.Source.PricePerThousand!.Value).Order().ToList());
                if (median <= 0m)
                {
                    return [];
                }

                return group
                    .Where(row => row.Source.PricePerThousand!.Value > median * 1.25m)
                    .Select(row => new PriceOutlierCandidate
                    {
                        ItemNo = row.Source.ItemNo,
                        ItemName = row.Source.ItemName,
                        LabelSize = row.NormalizedLabelSize,
                        Material = row.NormalizedMaterial,
                        PricePerThousand = row.Source.PricePerThousand!.Value,
                        GroupMedianPricePerThousand = median,
                        PercentAboveMedian = Math.Round((row.Source.PricePerThousand!.Value - median) / median * 100m, 2)
                    });
            })
            .OrderByDescending(candidate => candidate.PercentAboveMedian)
            .Take(10)
            .ToList();
    }

    private static List<ConsolidationCandidate> BuildConsolidationCandidates(IReadOnlyCollection<CleanedLabelLineItem> rows)
    {
        return rows
            .Where(row => !string.IsNullOrWhiteSpace(row.NormalizedLabelSize)
                && !string.IsNullOrWhiteSpace(row.NormalizedMaterial))
            .GroupBy(row => new
            {
                LabelSize = row.NormalizedLabelSize!,
                Material = row.NormalizedMaterial!
            })
            .Select(group => new ConsolidationCandidate
            {
                LabelSize = group.Key.LabelSize,
                Material = group.Key.Material,
                Spend = group.Sum(row => row.Source.Spend ?? 0m),
                ItemCount = group.Count(),
                SiteCount = group.Select(row => NormalizeGroupName(row.Source.Site)).Distinct().Count()
            })
            .Where(candidate => candidate.ItemCount >= 3)
            .OrderByDescending(candidate => candidate.Spend)
            .ThenByDescending(candidate => candidate.ItemCount)
            .Take(10)
            .ToList();
    }

    private static decimal Median(IReadOnlyList<decimal> values)
    {
        if (values.Count == 0)
        {
            return 0m;
        }

        var midpoint = values.Count / 2;
        return values.Count % 2 == 1
            ? values[midpoint]
            : (values[midpoint - 1] + values[midpoint]) / 2m;
    }

    private static string NormalizeGroupName(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? "(missing)" : value.Trim();
    }
}
