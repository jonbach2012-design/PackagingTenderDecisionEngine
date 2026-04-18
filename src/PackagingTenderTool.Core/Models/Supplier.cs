namespace PackagingTenderTool.Core.Models;

public sealed class Supplier
{
    public string Name { get; set; } = string.Empty;

    public string GroupingKey => Name;

    public List<LabelLineItem> LineItems { get; set; } = [];
}
