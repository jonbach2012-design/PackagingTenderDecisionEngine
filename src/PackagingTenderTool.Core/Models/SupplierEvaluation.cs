namespace PackagingTenderTool.Core.Models;

public sealed class SupplierEvaluation
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string SupplierName { get; set; } = string.Empty;

    public List<LineEvaluation> LineEvaluations { get; set; } = [];

    public ScoreBreakdown ScoreBreakdown { get; set; } = new();

    public decimal TotalSpend { get; set; }

    public List<ManualReviewFlag> ManualReviewFlags { get; set; } = [];

    public SupplierClassification? Classification { get; set; }

    public string? ClassificationReason { get; set; }

    public bool RequiresManualReview =>
        ManualReviewFlags.Count > 0 || LineEvaluations.Any(line => line.RequiresManualReview);
}
