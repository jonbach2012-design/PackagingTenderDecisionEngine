namespace PackagingTenderTool.Core.Models;

public sealed class Tender
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public TenderSettings Settings { get; set; } = new();

    public List<LabelLineItem> LabelLineItems { get; set; } = [];
}
