namespace PackagingTenderTool.Core.Models;

/// <summary>
/// Supplier row for label-tender analysis: pillar scores plus drill-down inputs.
/// </summary>
public sealed class SupplierModel
{
    public string SupplierName { get; init; } = string.Empty;

    /// <summary>Commercial sub-parameter (e.g. unit price).</summary>
    public decimal Price { get; init; }

    /// <summary>Technical / sustainability sub-parameter.</summary>
    public decimal Co2Impact { get; init; }

    /// <summary>Regulatory / logistics sub-parameter (calendar days).</summary>
    public decimal DeliveryTimeDays { get; init; }

    /// <summary>ISO country code or name for geographic cockpit KPIs.</summary>
    public string Country { get; init; } = string.Empty;

    /// <summary>Number of production / supply sites represented by this bid.</summary>
    public int SiteCount { get; init; }

    /// <summary>Pillar score 0–100 (commercial dimension).</summary>
    public decimal CommercialScore { get; init; }

    /// <summary>Pillar score 0–100 (technical dimension).</summary>
    public decimal TechnicalScore { get; init; }

    /// <summary>Pillar score 0–100 (regulatory dimension).</summary>
    public decimal RegulatoryScore { get; init; }

    /// <summary>
    /// Weighted final score when pillar scores are 0–100 and weights are percentage points summing to 100.
    /// Maximum is exactly 100.0 when all pillar scores are 100.
    /// </summary>
    public decimal ComputeFinalScore(decimal commercialWeightPct, decimal technicalWeightPct, decimal regulatoryWeightPct)
    {
        return (CommercialScore * (commercialWeightPct / 100m))
               + (TechnicalScore * (technicalWeightPct / 100m))
               + (RegulatoryScore * (regulatoryWeightPct / 100m));
    }
}

/// <summary>One grid row: supplier plus weighted total for approved pillar weights.</summary>
public sealed record SupplierPillarAnalysisRow(SupplierModel Supplier, decimal TotalScore);

/// <summary>
/// Weighted pillar aggregation. Weights are whole-number percentages that sum to 100.
/// </summary>
public static class SupplierPillarAnalysis
{
    /// <summary>Delegates to <see cref="SupplierModel.ComputeFinalScore"/>.</summary>
    public static decimal ComputeWeightedTotal(
        SupplierModel supplier,
        decimal commercialWeight,
        decimal technicalWeight,
        decimal regulatoryWeight)
    {
        ArgumentNullException.ThrowIfNull(supplier);
        return supplier.ComputeFinalScore(commercialWeight, technicalWeight, regulatoryWeight);
    }

    public static IReadOnlyList<SupplierPillarAnalysisRow> BuildOrdered(
        IReadOnlyList<SupplierModel> suppliers,
        decimal commercialWeight,
        decimal technicalWeight,
        decimal regulatoryWeight)
    {
        ArgumentNullException.ThrowIfNull(suppliers);
        return suppliers
            .Select(s => new SupplierPillarAnalysisRow(
                s,
                decimal.Round(ComputeWeightedTotal(s, commercialWeight, technicalWeight, regulatoryWeight), 1, MidpointRounding.AwayFromZero)))
            .OrderByDescending(r => r.TotalScore)
            .ThenBy(r => r.Supplier.SupplierName, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}
