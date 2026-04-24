using PackagingTenderTool.Core.Models;

namespace PackagingTenderTool.Core.Services.LabelTenderScoring;

public sealed class LabelTenderSupplierScore
{
    public required SupplierModel Supplier { get; init; }

    public decimal PriceScore { get; init; }

    public decimal Co2Score { get; init; }

    public decimal TotalScore { get; init; }
}

