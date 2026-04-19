namespace PackagingTenderTool.Core.Import;

public sealed class LabelsImportIssue
{
    public int RowNumber { get; set; }

    public string FieldName { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string? SourceValue { get; set; }

    public LabelsImportIssueSeverity Severity { get; set; } = LabelsImportIssueSeverity.Warning;
}

public enum LabelsImportIssueSeverity
{
    Info = 1,
    Warning = 2,
    Error = 3
}
