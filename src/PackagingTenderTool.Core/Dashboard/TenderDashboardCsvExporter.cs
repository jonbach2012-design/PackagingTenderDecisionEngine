using System.Globalization;
using System.Text;

namespace PackagingTenderTool.Core.Dashboard;

public sealed class TenderDashboardCsvExporter
{
    public string ExportItemRows(TenderDashboardViewModel dashboard)
    {
        ArgumentNullException.ThrowIfNull(dashboard);

        var builder = new StringBuilder();
        AppendRow(
            builder,
            "Item no",
            "Item name",
            "Supplier",
            "Country",
            "Site",
            "Label size",
            "Material",
            "Color group",
            "Winding direction",
            "Quantity",
            "Spend",
            "Price per 1,000",
            "Valid business data",
            "Has flags",
            "Outlier candidate");

        foreach (var row in dashboard.ItemRows)
        {
            AppendRow(
                builder,
                row.ItemNo,
                row.ItemName,
                row.SupplierName,
                row.Country,
                row.Site,
                row.LabelSize,
                row.Material,
                row.ColorGroup,
                row.WindingDirection,
                Format(row.Quantity),
                Format(row.Spend),
                Format(row.PricePerThousand),
                row.HasRequiredBusinessData ? "Yes" : "No",
                row.HasFlags ? "Yes" : "No",
                row.IsOutlierCandidate ? "Yes" : "No");
        }

        return builder.ToString();
    }

    public string ExportAnalyticsSummary(TenderDashboardViewModel dashboard)
    {
        ArgumentNullException.ThrowIfNull(dashboard);

        var builder = new StringBuilder();
        AppendRow(builder, "Section", "Name", "Value");
        foreach (var metric in dashboard.ImportMetrics)
        {
            AppendRow(builder, "Import", metric.Name, metric.Value);
        }

        foreach (var metric in dashboard.AnalyticsMetrics)
        {
            AppendRow(builder, "Analytics", metric.Name, metric.Value);
        }

        return builder.ToString();
    }

    public string ExportIssues(TenderDashboardViewModel dashboard)
    {
        ArgumentNullException.ThrowIfNull(dashboard);

        var builder = new StringBuilder();
        AppendRow(builder, "Row", "Field", "Severity", "Message", "Source value");
        foreach (var issue in dashboard.Issues)
        {
            AppendRow(
                builder,
                issue.RowNumber.ToString(CultureInfo.InvariantCulture),
                issue.FieldName,
                issue.Severity,
                issue.Message,
                issue.SourceValue);
        }

        return builder.ToString();
    }

    public string ExportSpendBreakdown(IEnumerable<DashboardSpendBreakdownRow> rows)
    {
        ArgumentNullException.ThrowIfNull(rows);

        var builder = new StringBuilder();
        AppendRow(builder, "Name", "Spend", "Share of total", "Items");
        foreach (var row in rows)
        {
            AppendRow(
                builder,
                row.Name,
                Format(row.Spend),
                Format(row.ShareOfTotal),
                row.ItemCount.ToString(CultureInfo.InvariantCulture));
        }

        return builder.ToString();
    }

    private static void AppendRow(StringBuilder builder, params string?[] cells)
    {
        builder.AppendLine(string.Join(",", cells.Select(Escape)));
    }

    private static string Escape(string? value)
    {
        value ??= string.Empty;
        return value.Contains('"') || value.Contains(',') || value.Contains('\n') || value.Contains('\r')
            ? $"\"{value.Replace("\"", "\"\"", StringComparison.Ordinal)}\""
            : value;
    }

    private static string Format(decimal? value)
    {
        return value?.ToString("0.##", CultureInfo.InvariantCulture) ?? string.Empty;
    }
}
