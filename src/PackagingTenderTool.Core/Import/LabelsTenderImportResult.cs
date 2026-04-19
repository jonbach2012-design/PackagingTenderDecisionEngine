using PackagingTenderTool.Core.Models;

namespace PackagingTenderTool.Core.Import;

public sealed class LabelsTenderImportResult
{
    public Tender Tender { get; set; } = new();

    public LabelsImportSummary Summary { get; set; } = new();

    public List<RawLabelTenderRow> RawRows { get; set; } = [];

    public List<CleanedLabelLineItem> CleanedRows { get; set; } = [];

    public List<LabelsImportIssue> Issues { get; set; } = [];

    public bool HasErrors => Issues.Any(issue => issue.Severity == LabelsImportIssueSeverity.Error);
}
