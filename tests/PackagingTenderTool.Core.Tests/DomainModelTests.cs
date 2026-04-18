using PackagingTenderTool.Core.Models;

namespace PackagingTenderTool.Core.Tests;

public sealed class DomainModelTests
{
    [Fact]
    public void TenderSettingsDefaultToLabelsProfileAndEurCurrency()
    {
        var tender = new Tender();

        Assert.Equal(PackagingProfile.Labels, tender.Settings.PackagingProfile);
        Assert.Equal("EUR", tender.Settings.CurrencyCode);
        Assert.Empty(tender.LabelLineItems);
        Assert.NotEqual(Guid.Empty, tender.Id);
    }

    [Fact]
    public void TenderOwnsLabelLineItemsForLabelsProfileV1()
    {
        var lineItem = new LabelLineItem
        {
            ItemNo = "LBL-001",
            SupplierName = "Acme Labels",
            Spend = 1250.50m
        };

        var tender = new Tender { Name = "Labels tender" };
        tender.LabelLineItems.Add(lineItem);

        Assert.Same(lineItem, tender.LabelLineItems.Single());
        Assert.Equal("Acme Labels", tender.LabelLineItems.Single().SupplierName);
        Assert.Equal(1250.50m, tender.LabelLineItems.Single().Spend);
    }

    [Fact]
    public void SupplierGroupingUsesSupplierNameInVersionOne()
    {
        var supplier = new Supplier { Name = "Acme Labels" };
        supplier.LineItems.Add(new LabelLineItem { SupplierName = "Acme Labels", Spend = 10m });
        supplier.LineItems.Add(new LabelLineItem { SupplierName = "Acme Labels", Spend = 20m });

        Assert.Equal("Acme Labels", supplier.GroupingKey);
        Assert.All(supplier.LineItems, line => Assert.Equal(supplier.Name, line.SupplierName));
    }

    [Fact]
    public void LabelLineItemSupportsNullableImportedValues()
    {
        var lineItem = new LabelLineItem
        {
            ItemNo = "LBL-002",
            SupplierName = null,
            Quantity = null,
            Spend = null,
            PricePerThousand = null,
            Price = null,
            TheoreticalSpend = null,
            NumberOfColors = null
        };

        Assert.Null(lineItem.SupplierName);
        Assert.Null(lineItem.Quantity);
        Assert.Null(lineItem.Spend);
        Assert.Null(lineItem.PricePerThousand);
        Assert.Null(lineItem.Price);
        Assert.Null(lineItem.TheoreticalSpend);
        Assert.Null(lineItem.NumberOfColors);
    }

    [Fact]
    public void LineEvaluationCanRepresentManualReviewWithoutScoringRules()
    {
        var lineItem = new LabelLineItem { ItemNo = "LBL-003" };
        var evaluation = new LineEvaluation
        {
            LineItemId = lineItem.Id,
            LineItem = lineItem
        };

        evaluation.ManualReviewFlags.Add(new ManualReviewFlag
        {
            FieldName = nameof(LabelLineItem.Spend),
            SourceValue = "not-a-number",
            Reason = "Imported spend value could not be parsed.",
            Severity = ManualReviewSeverity.Error
        });

        Assert.True(evaluation.RequiresManualReview);
        Assert.Same(lineItem, evaluation.LineItem);
        Assert.Equal(lineItem.Id, evaluation.LineItemId);
        Assert.Null(evaluation.ScoreBreakdown.Total);
    }

    [Fact]
    public void SupplierEvaluationAggregatesLineReviewStateAndLeavesSpendWeightingDeferred()
    {
        var lineEvaluation = new LineEvaluation
        {
            LineItem = new LabelLineItem
            {
                SupplierName = "Acme Labels",
                Spend = 100m
            }
        };

        lineEvaluation.LineItemId = lineEvaluation.LineItem.Id;
        lineEvaluation.ManualReviewFlags.Add(new ManualReviewFlag
        {
            FieldName = nameof(LabelLineItem.Material),
            Reason = "Material is missing.",
            Severity = ManualReviewSeverity.Warning
        });

        var supplierEvaluation = new SupplierEvaluation
        {
            SupplierName = "Acme Labels",
            TotalSpend = 100m
        };

        supplierEvaluation.LineEvaluations.Add(lineEvaluation);

        Assert.True(supplierEvaluation.RequiresManualReview);
        Assert.Equal("Acme Labels", supplierEvaluation.SupplierName);
        Assert.Equal(100m, supplierEvaluation.TotalSpend);
        Assert.Null(supplierEvaluation.ScoreBreakdown.Total);
    }
}
