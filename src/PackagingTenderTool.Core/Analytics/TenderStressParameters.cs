namespace PackagingTenderTool.Core.Analytics;

public sealed class TenderStressParameters
{
    /// <summary>
    /// Multiplier applied to EPR impact. Example: +10% => 1.10.
    /// </summary>
    public decimal EprInflationMultiplier { get; set; } = 1.0m;

    /// <summary>
    /// Multiplier applied to material/price based spend. Example: -5% => 0.95.
    /// </summary>
    public decimal MaterialPriceMultiplier { get; set; } = 1.0m;
}

