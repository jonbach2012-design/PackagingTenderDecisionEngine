namespace PackagingTenderTool.Core.Analytics;

public sealed class CtrSupplierSummary
{
    public string SupplierName { get; set; } = string.Empty;

    public decimal TotalTco { get; set; }

    public decimal CommercialScore { get; set; }

    public decimal TechnicalScore { get; set; }

    public decimal RegulatoryScore { get; set; }

    public decimal DecisionScore { get; set; }
}

