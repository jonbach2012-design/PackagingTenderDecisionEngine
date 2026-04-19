using PackagingTenderTool.Core.Dashboard;
using PackagingTenderTool.Core.Models;
using PackagingTenderTool.Core.Services;

namespace PackagingTenderTool.Core.Tests;

public sealed class TenderDashboardViewModelFactoryTests
{
    [Fact]
    public void CreateExposesImportAndAnalyticsMetrics()
    {
        var tender = new Tender
        {
            Name = "Dashboard tender"
        };
        tender.LabelLineItems.Add(CreateLine("A", "Jæren", "90X219", "PP top white", 100m, 100m));
        tender.LabelLineItems.Add(CreateLine("B", "Stokke", "80X263", "Thermo top", 200m, 200m));

        var result = new LabelsTenderEvaluationService().Evaluate(tender);
        var viewModel = new TenderDashboardViewModelFactory().Create(result);

        Assert.Contains(viewModel.AnalyticsMetrics, metric => metric.Name == "Sites" && metric.Value == "2");
        Assert.Contains(viewModel.AnalyticsMetrics, metric => metric.Name == "Countries" && metric.Value == "1");
        Assert.Contains(viewModel.AnalyticsMetrics, metric => metric.Name == "Label sizes" && metric.Value == "2");
        Assert.Contains(viewModel.AnalyticsMetrics, metric => metric.Name == "Materials" && metric.Value == "2");
        Assert.Contains(viewModel.AnalyticsMetrics, metric => metric.Name == "Total spend" && metric.Value == "300");
        Assert.Equal(2, viewModel.ImportSummary.RowsImported);
        Assert.Equal(1, viewModel.ImportSummary.CountryCount);
        Assert.Contains(viewModel.SupplierOverview, row => row.SupplierName == "Flexoprint AS" && row.TotalSpend == 300m);
        Assert.Contains(viewModel.SpendByCountry, row => row.Name == "Norway" && row.Spend == 300m);
        Assert.Contains(viewModel.SpendBySite, row => row.Name == "Stokke" && row.Spend == 200m);
        Assert.Contains(viewModel.SpendByMaterial, row => row.Name == "Thermo Top" && row.ItemCount == 1);
        Assert.Contains(viewModel.SpendByLabelSize, row => row.Name == "90x219" && row.ItemCount == 1);
        Assert.Contains(viewModel.ItemRows, row => row.ItemNo == "B" && row.Material == "Thermo Top");
        Assert.Contains(viewModel.Countries, option => option.Value == "Norway" && option.ItemCount == 2);
        Assert.Contains(viewModel.Sites, option => option.Value == "Jæren" && option.ItemCount == 1);
    }

    [Fact]
    public void CreateAppliesReusableDashboardQuery()
    {
        var tender = new Tender { Name = "Filtered dashboard tender" };
        tender.LabelLineItems.Add(CreateLine("A", "Jæren", "90X219", "PP top white", 100m, 100m));
        tender.LabelLineItems.Add(CreateLine("B", "Stokke", "80X263", "Thermo top", 200m, 200m));
        tender.LabelLineItems.Add(CreateLine("C", "Jæren", "90X219", "PP top white", 300m, 110m));

        var result = new LabelsTenderEvaluationService().Evaluate(tender);
        var viewModel = new TenderDashboardViewModelFactory().Create(
            result,
            new TenderDashboardQuery
            {
                Site = "Jæren",
                Country = "Norway",
                Supplier = "Flexoprint AS",
                Material = "PP Top White",
                MaxRows = 10
            });

        Assert.Equal(2, viewModel.ItemRows.Count);
        Assert.All(viewModel.ItemRows, row => Assert.Equal("Jæren", row.Site));
        Assert.Contains(viewModel.AnalyticsMetrics, metric => metric.Name == "Visible spend" && metric.Value == "400");
        Assert.Single(viewModel.SpendBySite);
        Assert.Single(viewModel.SpendByMaterial);
    }

    [Fact]
    public void CreateAppliesFlaggedOnlyAndOutlierOnlyFilters()
    {
        var tender = new Tender { Name = "Flagged and outlier dashboard tender" };
        tender.LabelLineItems.Add(CreateLine("A", "Jæren", "90X219", "PP top white", 100m, 100m));
        tender.LabelLineItems.Add(CreateLine("B", "Jæren", "90X219", "PP top white", 100m, 100m));
        tender.LabelLineItems.Add(CreateLine("C", "Jæren", "90X219", "PP top white", 100m, 200m));
        tender.LabelLineItems.Add(CreateLine("D", "Stokke", "80X263", "Thermo top", 100m, 120m, addFlag: true));

        var result = new LabelsTenderEvaluationService().Evaluate(tender);
        var factory = new TenderDashboardViewModelFactory();

        var flagged = factory.Create(result, new TenderDashboardQuery { FlaggedOnly = true });
        var outliers = factory.Create(result, new TenderDashboardQuery { OutliersOnly = true });

        Assert.Single(flagged.ItemRows);
        Assert.Equal("D", flagged.ItemRows[0].ItemNo);
        Assert.Single(outliers.ItemRows);
        Assert.Equal("C", outliers.ItemRows[0].ItemNo);
        Assert.True(outliers.ItemRows[0].IsOutlierCandidate);
    }

    [Fact]
    public void CsvExporterProducesExportReadyItemRows()
    {
        var tender = new Tender { Name = "Export dashboard tender" };
        tender.LabelLineItems.Add(CreateLine("A", "Jæren", "90X219", "PP top white", 100m, 100m));

        var result = new LabelsTenderEvaluationService().Evaluate(tender);
        var viewModel = new TenderDashboardViewModelFactory().Create(result);
        var csv = new TenderDashboardCsvExporter().ExportItemRows(viewModel);

        Assert.Contains("Item no,Item name,Supplier,Country,Site,Label size,Material", csv);
        Assert.Contains("A,Item A,Flexoprint AS,Norway,Jæren,90x219,PP top WHITE", csv);
        Assert.Contains("Valid business data,Has flags,Outlier candidate", csv);
        Assert.Contains("Import,Visible rows,1", new TenderDashboardCsvExporter().ExportAnalyticsSummary(viewModel));
    }

    private static LabelLineItem CreateLine(
        string itemNo,
        string site,
        string labelSize,
        string material,
        decimal spend,
        decimal pricePerThousand,
        bool addFlag = false)
    {
        var line = new LabelLineItem
        {
            ItemNo = itemNo,
            ItemName = $"Item {itemNo}",
            SupplierName = "Flexoprint AS",
            Site = site,
            LabelSize = labelSize,
            Material = material,
            Spend = spend,
            PricePerThousand = pricePerThousand
        };

        if (addFlag)
        {
            line.SourceManualReviewFlags.Add(new ManualReviewFlag
            {
                FieldName = "Test",
                Reason = "Test flag"
            });
        }

        return line;
    }
}
