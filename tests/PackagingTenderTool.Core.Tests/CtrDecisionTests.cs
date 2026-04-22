using PackagingTenderTool.Core.Analytics;
using PackagingTenderTool.Core.Models;

namespace PackagingTenderTool.Core.Tests;

public sealed class CtrDecisionTests
{
    [Fact]
    public void SupplierWithPerfectTechnicalAndRegulatoryCanLoseWhenCommercialIsWeightedHigh()
    {
        var expensivePerfect = new LabelLineItem
        {
            SupplierName = "SupplierPerfect",
            Spend = 100m,
            Quantity = 100_000m,
            PricePerThousand = 10m,
            TechnicalRating = 100m,
            IsMonoMaterial = true,
            IsEasyToSeparate = true,
            IsReusableOrRecyclableMaterial = true,
            HasTraceability = true,
            LabelWeightGrams = 1000m,
            EprSchemes = [new EprSchemeInfo { CountryCode = "DK", Category = "Flexibles" }]
        };

        var cheapMediocre = new LabelLineItem
        {
            SupplierName = "SupplierCheap",
            Spend = 10m,
            Quantity = 100_000m,
            PricePerThousand = 1m,
            TechnicalRating = 40m,
            IsMonoMaterial = false,
            IsEasyToSeparate = false,
            IsReusableOrRecyclableMaterial = false,
            HasTraceability = false,
            LabelWeightGrams = 1000m,
            EprSchemes = [new EprSchemeInfo { CountryCode = "DK", Category = "Flexibles" }]
        };

        // Build LineEvaluations with explicit regulatory/technical score breakdowns,
        // because the CTR model consumes LineEvaluation inputs.
        var lines = new List<LineEvaluation>
        {
            new()
            {
                LineItem = expensivePerfect,
                ScoreBreakdown = new ScoreBreakdown { Technical = 100m, Regulatory = 100m, Commercial = 0m, Total = 0m }
            },
            new()
            {
                LineItem = cheapMediocre,
                ScoreBreakdown = new ScoreBreakdown { Technical = 40m, Regulatory = 40m, Commercial = 0m, Total = 0m }
            }
        };

        var analytics = new TenderAnalyticsService();
        var weights = new CtrWeights { CommercialWeight = 0.85m, TechnicalWeight = 0.10m, RegulatoryWeight = 0.05m };

        var summaries = analytics.CalculateCtrDecisionScores(lines, weights);

        var winner = summaries.First();
        Assert.Equal("SupplierCheap", winner.SupplierName);
    }

    [Fact]
    public void CommercialNormalizationGives100ToLowestTcoAndRelativeScoresToOthers()
    {
        var cheap = new LineEvaluation
        {
            LineItem = new LabelLineItem { SupplierName = "A", Spend = 10m, Quantity = 100_000m, PricePerThousand = 1m, TechnicalRating = 50m },
            ScoreBreakdown = new ScoreBreakdown { Technical = 50m, Regulatory = 50m, Commercial = 0m, Total = 0m }
        };
        var expensive = new LineEvaluation
        {
            LineItem = new LabelLineItem { SupplierName = "B", Spend = 20m, Quantity = 100_000m, PricePerThousand = 2m, TechnicalRating = 50m },
            ScoreBreakdown = new ScoreBreakdown { Technical = 50m, Regulatory = 50m, Commercial = 0m, Total = 0m }
        };

        var analytics = new TenderAnalyticsService();
        var summaries = analytics.CalculateCtrDecisionScores([cheap, expensive], new CtrWeights { CommercialWeight = 1m, TechnicalWeight = 0m, RegulatoryWeight = 0m });

        var a = summaries.Single(s => s.SupplierName == "A");
        var b = summaries.Single(s => s.SupplierName == "B");

        Assert.Equal(100m, a.CommercialScore);
        Assert.Equal(50m, b.CommercialScore);
    }

    [Fact]
    public void DecisionScoreUsesNormalizedWeightsWhenTheyDoNotSumToOne()
    {
        var lineA = new LineEvaluation
        {
            LineItem = new LabelLineItem { SupplierName = "A", Spend = 10m, Quantity = 100_000m, PricePerThousand = 1m, TechnicalRating = 80m },
            ScoreBreakdown = new ScoreBreakdown { Technical = 80m, Regulatory = 20m, Commercial = 0m, Total = 0m }
        };
        var analytics = new TenderAnalyticsService();
        var weights = new CtrWeights { CommercialWeight = 600m, TechnicalWeight = 300m, RegulatoryWeight = 100m }; // sums to 1000 (should normalize)

        var summary = analytics.CalculateCtrDecisionScores([lineA], weights).Single();

        // Single supplier => cheapest is itself => CommercialScore = 100.
        Assert.Equal(decimal.Round(100m * 0.60m + 80m * 0.30m + 20m * 0.10m, 2), summary.DecisionScore);
    }

    [Fact]
    public void TechnicalUsesTechnicalRatingWhenPresentOtherwiseFallsBackToLineTechnicalScore()
    {
        var withRating = new LineEvaluation
        {
            LineItem = new LabelLineItem { SupplierName = "A", Spend = 10m, Quantity = 100_000m, PricePerThousand = 1m, TechnicalRating = 90m },
            ScoreBreakdown = new ScoreBreakdown { Technical = 10m, Regulatory = 50m, Commercial = 0m, Total = 0m }
        };
        var noRating = new LineEvaluation
        {
            LineItem = new LabelLineItem { SupplierName = "B", Spend = 10m, Quantity = 100_000m, PricePerThousand = 1m, TechnicalRating = null },
            ScoreBreakdown = new ScoreBreakdown { Technical = 70m, Regulatory = 50m, Commercial = 0m, Total = 0m }
        };

        var analytics = new TenderAnalyticsService();
        var summaries = analytics.CalculateCtrDecisionScores([withRating, noRating], new CtrWeights { CommercialWeight = 0m, TechnicalWeight = 1m, RegulatoryWeight = 0m });

        Assert.Equal(90m, summaries.Single(s => s.SupplierName == "A").TechnicalScore);
        Assert.Equal(70m, summaries.Single(s => s.SupplierName == "B").TechnicalScore);
    }

    [Fact]
    public void RegulatoryIsSpendWeightedAcrossLines()
    {
        var line1 = new LineEvaluation
        {
            LineItem = new LabelLineItem { SupplierName = "A", Spend = 25m, Quantity = 100_000m, PricePerThousand = 1m, TechnicalRating = 50m },
            ScoreBreakdown = new ScoreBreakdown { Technical = 50m, Regulatory = 80m, Commercial = 0m, Total = 0m }
        };
        var line2 = new LineEvaluation
        {
            LineItem = new LabelLineItem { SupplierName = "A", Spend = 75m, Quantity = 100_000m, PricePerThousand = 1m, TechnicalRating = 50m },
            ScoreBreakdown = new ScoreBreakdown { Technical = 50m, Regulatory = 40m, Commercial = 0m, Total = 0m }
        };

        var analytics = new TenderAnalyticsService();
        var summary = analytics.CalculateCtrDecisionScores([line1, line2], new CtrWeights { CommercialWeight = 0m, TechnicalWeight = 0m, RegulatoryWeight = 1m })
            .Single();

        Assert.Equal(50m, summary.RegulatoryScore);
    }
}

