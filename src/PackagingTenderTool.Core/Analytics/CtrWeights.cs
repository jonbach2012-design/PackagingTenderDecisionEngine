namespace PackagingTenderTool.Core.Analytics;

public sealed class CtrWeights
{
    public decimal CommercialWeight { get; set; } = 0.60m;
    public decimal TechnicalWeight { get; set; } = 0.30m;
    public decimal RegulatoryWeight { get; set; } = 0.10m;
}

