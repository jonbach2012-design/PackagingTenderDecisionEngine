using PackagingTenderTool.Core.Analytics;
using PackagingTenderTool.Core.Models;
using PackagingTenderTool.Core.Services;

namespace PackagingTenderTool.Core.Tests;

public sealed class TenderAnalyticsServiceTests
{
    [Fact]
    public void AnalyzeCreatesSpendBreakdownsAndCandidates()
    {
        var cleaner = new LabelDataCleaningService();
        var rows = new[]
        {
            CreateLine("A", "Jæren", "90X219", "PP top white", 100m, 100m),
            CreateLine("B", "Jæren", "90x219", "PP top white", 200m, 105m),
            CreateLine("C", "Stokke", "90x219", "PP top white", 300m, 180m),
            CreateLine("D", "Stokke", "80X263", "PP top white", 400m, 120m)
        }.Select(cleaner.Clean).ToList();

        var summary = new TenderAnalyticsService().Analyze(rows);

        Assert.Equal(1000m, summary.TotalSpend);
        Assert.Equal(4, summary.ItemCount);
        Assert.Contains(summary.SpendByCountry, item => item.Name == "Norway" && item.ItemCount == 4);
        Assert.Equal("Stokke", summary.SpendBySite[0].Name);
        Assert.Contains(summary.SpendByLabelSize, item => item.Name == "90x219" && item.ItemCount == 3);
        Assert.Contains(summary.ConsolidationCandidates, candidate =>
            candidate.LabelSize == "90x219"
            && candidate.ItemCount == 3
            && candidate.SiteCount == 2);
        Assert.Contains(summary.PriceOutlierCandidates, candidate =>
            candidate.ItemNo == "C"
            && candidate.PercentAboveMedian > 25m);
    }

    private static LabelLineItem CreateLine(
        string itemNo,
        string site,
        string labelSize,
        string material,
        decimal spend,
        decimal pricePerThousand)
    {
        return new LabelLineItem
        {
            ItemNo = itemNo,
            ItemName = $"Item {itemNo}",
            SupplierName = "Flexoprint AS",
            Site = site,
            LabelSize = labelSize,
            Material = material,
            Spend = spend,
            PricePerThousand = pricePerThousand
        };
    }
}
