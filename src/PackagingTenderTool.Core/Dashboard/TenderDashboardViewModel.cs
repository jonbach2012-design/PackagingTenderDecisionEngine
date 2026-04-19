namespace PackagingTenderTool.Core.Dashboard;

public sealed class TenderDashboardViewModel
{
    public TenderDashboardQuery Query { get; set; } = new();

    public DashboardImportSummary ImportSummary { get; set; } = new();

    public List<DashboardMetric> ImportMetrics { get; set; } = [];

    public List<DashboardMetric> AnalyticsMetrics { get; set; } = [];

    public List<DashboardSupplierOverviewRow> SupplierOverview { get; set; } = [];

    public List<DashboardFilterOption> Suppliers { get; set; } = [];

    public List<DashboardFilterOption> Countries { get; set; } = [];

    public List<DashboardFilterOption> Sites { get; set; } = [];

    public List<DashboardFilterOption> Materials { get; set; } = [];

    public List<DashboardFilterOption> LabelSizes { get; set; } = [];

    public List<DashboardSpendBreakdownRow> SpendByCountry { get; set; } = [];

    public List<DashboardSpendBreakdownRow> SpendBySite { get; set; } = [];

    public List<DashboardSpendBreakdownRow> SpendByMaterial { get; set; } = [];

    public List<DashboardSpendBreakdownRow> SpendByLabelSize { get; set; } = [];

    public List<DashboardTenderItemRow> ItemRows { get; set; } = [];

    public List<DashboardTopSpendRow> TopSpendItems { get; set; } = [];

    public List<DashboardOutlierRow> Outliers { get; set; } = [];

    public List<DashboardConsolidationRow> ConsolidationCandidates { get; set; } = [];

    public List<DashboardIssueRow> Issues { get; set; } = [];
}

public sealed class TenderDashboardQuery
{
    public string? Supplier { get; set; }

    public string? Country { get; set; }

    public string? Site { get; set; }

    public string? Material { get; set; }

    public string? LabelSize { get; set; }

    public decimal? MinimumSpend { get; set; }

    public bool FlaggedOnly { get; set; }

    public bool OutliersOnly { get; set; }

    public int MaxRows { get; set; } = 250;
}

public sealed class DashboardImportSummary
{
    public string WorksheetName { get; set; } = string.Empty;

    public int HeaderRowNumber { get; set; }

    public int RowsImported { get; set; }

    public int ValidRows { get; set; }

    public int InvalidRows { get; set; }

    public int SkippedRows { get; set; }

    public int SupplierCount { get; set; }

    public int CountryCount { get; set; }

    public int SiteCount { get; set; }

    public int SizeCount { get; set; }

    public int MaterialCount { get; set; }

    public int ManualReviewFlagCount { get; set; }

    public decimal TotalSpend { get; set; }
}

public sealed class DashboardMetric
{
    public string Name { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;
}

public sealed class DashboardFilterOption
{
    public string Value { get; set; } = string.Empty;

    public int ItemCount { get; set; }

    public decimal Spend { get; set; }
}

public sealed class DashboardSupplierOverviewRow
{
    public string SupplierName { get; set; } = string.Empty;

    public decimal TotalSpend { get; set; }

    public decimal? CommercialScore { get; set; }

    public decimal? TechnicalScore { get; set; }

    public decimal? RegulatoryScore { get; set; }

    public decimal? TotalScore { get; set; }

    public string Classification { get; set; } = string.Empty;

    public bool RequiresManualReview { get; set; }

    public int ManualReviewFlagCount { get; set; }
}

public sealed class DashboardSpendBreakdownRow
{
    public string Name { get; set; } = string.Empty;

    public decimal Spend { get; set; }

    public decimal ShareOfTotal { get; set; }

    public int ItemCount { get; set; }
}

public sealed class DashboardTenderItemRow
{
    public string? ItemNo { get; set; }

    public string? ItemName { get; set; }

    public string? SupplierName { get; set; }

    public string? Country { get; set; }

    public string? Site { get; set; }

    public string? LabelSize { get; set; }

    public string? Material { get; set; }

    public string? ColorGroup { get; set; }

    public string? WindingDirection { get; set; }

    public decimal? Quantity { get; set; }

    public decimal? Spend { get; set; }

    public decimal? PricePerThousand { get; set; }

    public bool HasRequiredBusinessData { get; set; }

    public bool HasFlags { get; set; }

    public bool IsOutlierCandidate { get; set; }
}

public sealed class DashboardTopSpendRow
{
    public string? ItemNo { get; set; }

    public string? ItemName { get; set; }

    public string? SupplierName { get; set; }

    public string? Site { get; set; }

    public string? LabelSize { get; set; }

    public decimal Spend { get; set; }
}

public sealed class DashboardOutlierRow
{
    public string? ItemNo { get; set; }

    public string? ItemName { get; set; }

    public string? LabelSize { get; set; }

    public string? Material { get; set; }

    public decimal PricePerThousand { get; set; }

    public decimal MedianPricePerThousand { get; set; }

    public decimal PercentAboveMedian { get; set; }
}

public sealed class DashboardConsolidationRow
{
    public string LabelSize { get; set; } = string.Empty;

    public string Material { get; set; } = string.Empty;

    public decimal Spend { get; set; }

    public int ItemCount { get; set; }

    public int SiteCount { get; set; }
}

public sealed class DashboardIssueRow
{
    public int RowNumber { get; set; }

    public string FieldName { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string? SourceValue { get; set; }

    public string Severity { get; set; } = string.Empty;
}
