namespace PackagingTenderTool.Core.Analytics;

public sealed class TenderAnalyticsSummary
{
    public decimal TotalSpend { get; set; }

    public int ItemCount { get; set; }

    public List<SpendBreakdownItem> SpendBySite { get; set; } = [];

    public List<SpendBreakdownItem> SpendByCountry { get; set; } = [];

    public List<SpendBreakdownItem> SpendByLabelSize { get; set; } = [];

    public List<SpendBreakdownItem> SpendByMaterial { get; set; } = [];

    public List<TopSpendItem> TopSpendItems { get; set; } = [];

    public List<PriceOutlierCandidate> PriceOutlierCandidates { get; set; } = [];

    public List<ConsolidationCandidate> ConsolidationCandidates { get; set; } = [];
}

public sealed class SpendBreakdownItem
{
    public string Name { get; set; } = string.Empty;

    public decimal Spend { get; set; }

    public decimal ShareOfTotal { get; set; }

    public int ItemCount { get; set; }
}

public sealed class TopSpendItem
{
    public string? ItemNo { get; set; }

    public string? ItemName { get; set; }

    public string? SupplierName { get; set; }

    public string? Site { get; set; }

    public string? LabelSize { get; set; }

    public decimal Spend { get; set; }
}

public sealed class PriceOutlierCandidate
{
    public string? ItemNo { get; set; }

    public string? ItemName { get; set; }

    public string? LabelSize { get; set; }

    public string? Material { get; set; }

    public decimal PricePerThousand { get; set; }

    public decimal GroupMedianPricePerThousand { get; set; }

    public decimal PercentAboveMedian { get; set; }
}

public sealed class ConsolidationCandidate
{
    public string LabelSize { get; set; } = string.Empty;

    public string Material { get; set; } = string.Empty;

    public decimal Spend { get; set; }

    public int ItemCount { get; set; }

    public int SiteCount { get; set; }
}
