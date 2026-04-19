namespace PackagingTenderTool.Core.Import;

public sealed class RawLabelTenderRow
{
    public int RowNumber { get; set; }

    public string? ItemNo { get; set; }

    public string? ItemName { get; set; }

    public string? SupplierName { get; set; }

    public string? Site { get; set; }

    public string? Quantity { get; set; }

    public string? Spend { get; set; }

    public string? PricePerThousand { get; set; }

    public string? Price { get; set; }

    public string? TheoreticalSpend { get; set; }

    public string? LabelSize { get; set; }

    public string? WindingDirection { get; set; }

    public string? Material { get; set; }

    public string? ReelDiameterOrPcsPerRoll { get; set; }

    public string? NumberOfColors { get; set; }

    public string? Comment { get; set; }
}
