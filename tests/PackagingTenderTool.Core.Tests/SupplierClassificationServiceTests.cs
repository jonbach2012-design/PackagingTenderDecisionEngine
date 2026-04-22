using PackagingTenderTool.Core.Models;
using PackagingTenderTool.Core.Services;

namespace PackagingTenderTool.Core.Tests;

public sealed class SupplierClassificationServiceTests
{
    [Fact]
    public void ClassifyReturnsRecommendedWhenScoreMeetsRecommendedThreshold()
    {
        var supplierEvaluation = CreateSupplierEvaluation(totalScore: 70m);

        var classification = new SupplierClassificationService().Classify(supplierEvaluation);

        Assert.Equal(SupplierClassification.Recommended, classification);
    }

    [Fact]
    public void ClassifyReturnsConditionalWhenScoreMeetsConditionalThreshold()
    {
        var supplierEvaluation = CreateSupplierEvaluation(totalScore: 50m);

        var classification = new SupplierClassificationService().Classify(supplierEvaluation);

        Assert.Equal(SupplierClassification.Conditional, classification);
    }

    [Fact]
    public void ClassifyReturnsNotRecommendedWhenScoreIsBelowConditionalThreshold()
    {
        var supplierEvaluation = CreateSupplierEvaluation(totalScore: 49.99m);

        var classification = new SupplierClassificationService().Classify(supplierEvaluation);

        Assert.Equal(SupplierClassification.NotRecommended, classification);
    }

    [Fact]
    public void ClassifyReturnsConditionalWhenFlagsArePresent()
    {
        var supplierEvaluation = CreateSupplierEvaluation(totalScore: 95m);
        supplierEvaluation.ManualReviewFlags.Add(new ManualReviewFlag
        {
            FieldName = nameof(LabelLineItem.Spend),
            Reason = "Spend is missing."
        });

        var classification = new SupplierClassificationService().Classify(supplierEvaluation);

        Assert.Equal(SupplierClassification.Conditional, classification);
    }

    [Fact]
    public void ClassifyReturnsConditionalWhenTotalScoreIsMissing()
    {
        var supplierEvaluation = CreateSupplierEvaluation(totalScore: null);

        var classification = new SupplierClassificationService().Classify(supplierEvaluation);

        Assert.Equal(SupplierClassification.Conditional, classification);
    }

    [Fact]
    public void ApplyClassificationStoresClassificationAndReasonOnSupplierEvaluation()
    {
        var supplierEvaluation = CreateSupplierEvaluation(totalScore: 75m);
        var service = new SupplierClassificationService();

        service.ApplyClassification(supplierEvaluation);

        Assert.Equal(SupplierClassification.Recommended, supplierEvaluation.Classification);
        Assert.Contains("provisional", supplierEvaluation.ClassificationReason);
    }

    [Fact]
    public void ApplyClassificationsUpdatesAllSupplierEvaluations()
    {
        var supplierEvaluations = new[]
        {
            CreateSupplierEvaluation(totalScore: 80m),
            CreateSupplierEvaluation(totalScore: 60m),
            CreateSupplierEvaluation(totalScore: 40m)
        };

        var classifiedEvaluations = new SupplierClassificationService()
            .ApplyClassifications(supplierEvaluations);

        Assert.Equal(SupplierClassification.Recommended, classifiedEvaluations[0].Classification);
        Assert.Equal(SupplierClassification.Conditional, classifiedEvaluations[1].Classification);
        Assert.Equal(SupplierClassification.NotRecommended, classifiedEvaluations[2].Classification);
    }

    private static SupplierEvaluation CreateSupplierEvaluation(decimal? totalScore)
    {
        return new SupplierEvaluation
        {
            SupplierName = "Acme Labels",
            ScoreBreakdown = new ScoreBreakdown
            {
                Commercial = totalScore,
                Technical = 0m,
                Regulatory = 0m,
                Total = totalScore
            },
            TotalSpend = 100m
        };
    }
}
