namespace PackagingTenderTool.Core.Models;

public sealed class LabelLineItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string? ItemNo { get; set; }

    public string? ItemName { get; set; }

    public string? SupplierName { get; set; }

    public string? Site { get; set; }

    public decimal? Quantity { get; set; }

    public decimal? Spend { get; set; }

    public decimal? PricePerThousand { get; set; }

    public decimal? Price { get; set; }

    public decimal? TheoreticalSpend { get; set; }

    public string? LabelSize { get; set; }

    public string? WindingDirection { get; set; }

    public string? Material { get; set; }

    public string? ReelDiameterOrPcsPerRoll { get; set; }

    public int? NumberOfColors { get; set; }

    public decimal? LabelWeightGrams { get; set; }

    public bool? IsMonoMaterial { get; set; }

    public bool? IsEasyToSeparate { get; set; }

    public bool? IsReusableOrRecyclableMaterial { get; set; }

    public bool? HasTraceability { get; set; }

    public List<ManualReviewFlag> SourceManualReviewFlags { get; set; } = [];

    public string? Comment { get; set; }
}
