namespace PackagingTenderTool.Core.Models;

public sealed class CleanedLabelLineItem
{
    public LabelLineItem Source { get; set; } = new();

    public string? NormalizedLabelSize { get; set; }

    public string? NormalizedMaterial { get; set; }

    public string? Country { get; set; }

    public string? NormalizedColorGroup { get; set; }

    public string? NormalizedWindingDirection { get; set; }

    public bool HasRequiredBusinessData =>
        !string.IsNullOrWhiteSpace(Source.ItemNo)
        && !string.IsNullOrWhiteSpace(Source.SupplierName)
        && Source.Spend.HasValue;
}
