namespace PackagingTenderTool.Core.Models;

public sealed class ScoreBreakdown
{
    public decimal? Commercial { get; set; }

    public decimal? Technical { get; set; }

    public decimal? Regulatory { get; set; }

    public decimal? Total { get; set; }
}
