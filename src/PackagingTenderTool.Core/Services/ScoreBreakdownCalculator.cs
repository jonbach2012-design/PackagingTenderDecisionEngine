using PackagingTenderTool.Core.Models;

namespace PackagingTenderTool.Core.Services;

public static class ScoreBreakdownCalculator
{
    public const decimal CommercialWeight = 0.30m;
    public const decimal TechnicalWeight = 0.30m;
    public const decimal RegulatoryWeight = 0.40m;

    public static decimal? CalculateTotal(ScoreBreakdown scoreBreakdown)
    {
        if (scoreBreakdown.Commercial is null
            || scoreBreakdown.Technical is null
            || scoreBreakdown.Regulatory is null)
        {
            return null;
        }

        return decimal.Round(
            scoreBreakdown.Commercial.Value * CommercialWeight
            + scoreBreakdown.Technical.Value * TechnicalWeight
            + scoreBreakdown.Regulatory.Value * RegulatoryWeight,
            2);
    }
}
