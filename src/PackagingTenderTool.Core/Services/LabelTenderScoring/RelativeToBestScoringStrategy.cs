using PackagingTenderTool.Core.Models;

namespace PackagingTenderTool.Core.Services.LabelTenderScoring;

/// <summary>
/// Scores suppliers relative to the best (lowest) observed Price / CO2 in the compared set.
/// Intended as an explainable first-pass scoring for the LabelTender screen.
/// </summary>
public sealed class RelativeToBestScoringStrategy : ILabelTenderScoringStrategy
{
    public IReadOnlyList<LabelTenderSupplierScore> Score(
        IReadOnlyList<SupplierModel> suppliers,
        LabelTenderWeights weights,
        LabelTenderAdvancedConstraints constraints)
    {
        ArgumentNullException.ThrowIfNull(suppliers);
        ArgumentNullException.ThrowIfNull(weights);
        ArgumentNullException.ThrowIfNull(constraints);

        if (suppliers.Count == 0)
        {
            return [];
        }

        var minPrice = suppliers.Where(s => s.Price > 0m).Select(s => s.Price).DefaultIfEmpty(0m).Min();
        var minCo2 = suppliers.Where(s => s.Co2Impact > 0m).Select(s => s.Co2Impact).DefaultIfEmpty(0m).Min();

        var (wPrice, wCo2) = weights.GetNormalized();

        return suppliers
            .Select(supplier =>
            {
                _ = constraints;
                var enrichedSupplier = new SupplierModel
                {
                    SupplierName = supplier.SupplierName,
                    Price = supplier.Price,
                    Co2Impact = supplier.Co2Impact,
                    DeliveryTimeDays = supplier.DeliveryTimeDays,
                    Country = supplier.Country,
                    SiteCount = supplier.SiteCount,
                    CommercialScore = supplier.CommercialScore,
                    TechnicalScore = supplier.TechnicalScore,
                    RegulatoryScore = supplier.RegulatoryScore
                };

                var priceScore = ScoreLowerIsBetter(minPrice, enrichedSupplier.Price);
                var co2Score = ScoreLowerIsBetter(minCo2, enrichedSupplier.Co2Impact);
                var total = (priceScore * wPrice) + (co2Score * wCo2);

                return new LabelTenderSupplierScore
                {
                    Supplier = enrichedSupplier,
                    PriceScore = decimal.Round(priceScore, 2),
                    Co2Score = decimal.Round(co2Score, 2),
                    TotalScore = decimal.Round(total, 2)
                };
            })
            .OrderByDescending(result => result.TotalScore)
            .ThenBy(result => result.Supplier.SupplierName, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static decimal ScoreLowerIsBetter(decimal best, decimal current)
    {
        if (best <= 0m || current <= 0m)
        {
            return 0m;
        }

        // Aligns with spec-style relative scoring direction: (best / current) * 100.
        return (best / current) * 100m;
    }
}

