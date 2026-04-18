namespace PackagingTenderTool.Core.Models;

public sealed class TenderSettings
{
    public PackagingProfile PackagingProfile { get; set; } = PackagingProfile.Labels;

    public string CurrencyCode { get; set; } = "EUR";
}
