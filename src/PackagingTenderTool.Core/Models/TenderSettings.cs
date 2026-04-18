namespace PackagingTenderTool.Core.Models;

public sealed class TenderSettings
{
    public PackagingProfile PackagingProfile { get; set; } = PackagingProfile.Labels;

    public string CurrencyCode { get; set; } = "EUR";

    public string? ExpectedMaterial { get; set; }

    public string? ExpectedWindingDirection { get; set; }

    public string? ExpectedLabelSize { get; set; }

    public decimal? MaximumLabelWeightGrams { get; set; }

    public bool? ExpectedMonoMaterial { get; set; }

    public bool? ExpectedEasySeparation { get; set; }

    public bool? ExpectedReusableOrRecyclableMaterial { get; set; }

    public bool? ExpectedTraceability { get; set; }
}
