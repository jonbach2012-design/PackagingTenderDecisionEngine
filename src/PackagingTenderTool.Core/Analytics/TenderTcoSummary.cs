namespace PackagingTenderTool.Core.Analytics;

public sealed class TenderTcoSummary
{
    public decimal TotalNetSpend { get; set; }

    public decimal TotalEprImpact { get; set; }

    public decimal AggregatedTco => TotalNetSpend + TotalEprImpact;

    public decimal? WeightedRegulatoryScore { get; set; }
}

