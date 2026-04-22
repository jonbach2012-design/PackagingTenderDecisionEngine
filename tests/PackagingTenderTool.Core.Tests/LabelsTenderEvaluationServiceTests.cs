using PackagingTenderTool.Core.Models;
using PackagingTenderTool.Core.Services;

namespace PackagingTenderTool.Core.Tests;

public sealed class LabelsTenderEvaluationServiceTests
{
    [Fact]
    public void EvaluateRunsLineSupplierAndClassificationFlow()
    {
        var tender = new Tender
        {
            Name = "Labels tender",
            Settings = new TenderSettings
            {
                ExpectedMaterial = "PP white",
                ExpectedWindingDirection = "Left",
                ExpectedLabelSize = "80x120",
                MaximumLabelWeightGrams = 2m,
                ExpectedMonoMaterial = true,
                ExpectedEasySeparation = true,
                ExpectedReusableOrRecyclableMaterial = true,
                ExpectedTraceability = true
            }
        };
        var lineItem = TestDataFactory.CreateValidLabelLineItem(
            supplierName: "Acme Labels",
            spend: 100m,
            pricePerThousand: 10m,
            countryCode: "DK",
            category: "Labels",
            labelWeightGrams: 1.5m);
        lineItem.Material = "PP white";
        lineItem.WindingDirection = "Left";
        lineItem.LabelSize = "80x120";
        lineItem.IsMonoMaterial = true;
        lineItem.IsEasyToSeparate = true;
        lineItem.IsReusableOrRecyclableMaterial = true;
        lineItem.HasTraceability = true;
        tender.LabelLineItems.Add(lineItem);

        var result = new LabelsTenderEvaluationService().Evaluate(tender);
        var supplierEvaluation = result.SupplierEvaluations.Single();

        Assert.Same(tender, result.Tender);
        Assert.Single(result.LineEvaluations);
        Assert.Equal("Acme Labels", supplierEvaluation.SupplierName);
        Assert.Equal(100m, supplierEvaluation.ScoreBreakdown.Regulatory);
        Assert.Equal(100m, supplierEvaluation.ScoreBreakdown.Total);
        Assert.Equal(SupplierClassification.Recommended, supplierEvaluation.Classification);
        Assert.False(supplierEvaluation.RequiresManualReview);
    }
}
