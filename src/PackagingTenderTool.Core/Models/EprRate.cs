namespace PackagingTenderTool.Core.Models;

public sealed class EprRate
{
    public string CountryCode { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Fee rate in currency units per kilogram.
    /// </summary>
    public decimal RatePerKg { get; set; }
}

