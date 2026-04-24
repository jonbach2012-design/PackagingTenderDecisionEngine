namespace PackagingTenderTool.Core.Models;

public sealed class LabelTenderWeights
{
    public decimal PriceWeight { get; init; } = 0.5m;

    public decimal Co2Weight { get; init; } = 0.5m;

    public (decimal Price, decimal Co2) GetNormalized()
    {
        var sum = PriceWeight + Co2Weight;
        if (sum <= 0m)
        {
            return (0.5m, 0.5m);
        }

        return (PriceWeight / sum, Co2Weight / sum);
    }
}

