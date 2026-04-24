namespace PackagingTenderTool.Core.Models;

/// <summary>User-tunable limits for LabelTender constraint checks (CO₂ and lead time).</summary>
public sealed class LabelTenderAdvancedConstraints
{
    public const decimal Co2SliderMin = 1.0m;

    public const decimal Co2SliderMax = 5.0m;

    public const decimal LeadSliderMin = 5m;

    public const decimal LeadSliderMax = 60m;

    /// <summary>Suppliers with CO₂-impact strictly above this fail the constraint.</summary>
    public decimal MaxCo2Impact { get; init; } = 2.25m;

    /// <summary>Suppliers with delivery / lead time strictly above this (calendar days) fail the constraint.</summary>
    public decimal MaxLeadTimeDays { get; init; } = 20m;
}
