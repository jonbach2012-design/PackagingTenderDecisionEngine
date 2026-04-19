namespace PackagingTenderTool.Core.Import;

public sealed class LabelsImportSummary
{
    public string WorksheetName { get; set; } = string.Empty;

    public int HeaderRowNumber { get; set; }

    public int TotalRowsScanned { get; set; }

    public int ImportedRows { get; set; }

    public int ValidRows { get; set; }

    public int InvalidRows { get; set; }

    public int SkippedRows { get; set; }

    public int ManualReviewFlagCount { get; set; }

    public int SupplierCount { get; set; }

    public int SiteCount { get; set; }

    public int SizeCount { get; set; }

    public int MaterialCount { get; set; }

    public decimal TotalSpend { get; set; }
}
