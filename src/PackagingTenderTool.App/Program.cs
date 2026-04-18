using System.Globalization;
using ClosedXML.Excel;
using PackagingTenderTool.Core.Models;
using PackagingTenderTool.Core.Services;

var tenderSettings = CreateSampleTenderSettings();
using var importStream = CreateSampleLabelsWorkbook();

var result = new LabelsTenderEvaluationService().ImportAndEvaluate(
    importStream,
    "Labels Tender v1 Regulatory Scoring Sample",
    tenderSettings);

PrintSummary(result);

static TenderSettings CreateSampleTenderSettings()
{
    return new TenderSettings
    {
        PackagingProfile = PackagingProfile.Labels,
        CurrencyCode = "EUR",
        ExpectedMaterial = "PP white",
        ExpectedWindingDirection = "Left",
        ExpectedLabelSize = "80x120",
        MaximumLabelWeightGrams = 2m,
        ExpectedMonoMaterial = true,
        ExpectedEasySeparation = true,
        ExpectedReusableOrRecyclableMaterial = true,
        ExpectedTraceability = true
    };
}

static MemoryStream CreateSampleLabelsWorkbook()
{
    using var workbook = new XLWorkbook();
    var worksheet = workbook.Worksheets.Add("Labels");
    var headers = new[]
    {
        "Item no",
        "Item name",
        "Supplier name",
        "Site",
        "Quantity",
        "Spend",
        "Price per 1,000",
        "Price",
        "Theoretical spend",
        "Label size",
        "Winding direction",
        "Material",
        "Reel diameter / pcs per roll",
        "No. of colors",
        "Label weight (g)",
        "Mono-material design",
        "Easy separation",
        "Reusable or recyclable material direction",
        "Traceability",
        "Comment"
    };
    var rows = new object?[][]
    {
        [
            "LBL-001",
            "Front label 80x120",
            "Acme Labels",
            "DK01",
            "100000",
            "1.250,00",
            "12,50",
            null,
            "1.250,00",
            "80x120",
            "Left",
            "PP white",
            "300mm",
            4,
            "1,8",
            "yes",
            "yes",
            "yes",
            "yes",
            "Matches all regulatory reference values."
        ],
        [
            "LBL-002",
            "Back label 60x90",
            "Acme Labels",
            "DK01",
            "80000",
            "740.00",
            "9.25",
            null,
            "740.00",
            "80x120",
            "Left",
            "Paper",
            "300mm",
            2,
            "2.2",
            "no",
            "yes",
            "yes",
            "yes",
            "Higher weight and mono-material mismatch reduce regulatory score."
        ],
        [
            "LBL-003",
            "Neck label 35x45",
            "Beta Packaging",
            "SE01",
            "60000",
            "690,00",
            "11,50",
            null,
            "690,00",
            "80x120",
            "Right",
            "PP clear",
            "250mm",
            3,
            "1,6",
            "yes",
            "no",
            "yes",
            "yes",
            "Easy separation mismatch reduces regulatory score."
        ],
        [
            "LBL-004",
            "Promo label 50x50",
            "Beta Packaging",
            "SE01",
            "25000",
            null,
            "8.75",
            null,
            null,
            null,
            "Left",
            null,
            "200mm",
            1,
            null,
            null,
            "yes",
            null,
            "yes",
            "Missing values demonstrate non-blocking manual review."
        ]
    };

    for (var columnIndex = 0; columnIndex < headers.Length; columnIndex++)
    {
        worksheet.Cell(1, columnIndex + 1).Value = headers[columnIndex];
    }

    for (var rowIndex = 0; rowIndex < rows.Length; rowIndex++)
    {
        for (var columnIndex = 0; columnIndex < rows[rowIndex].Length; columnIndex++)
        {
            worksheet.Cell(rowIndex + 2, columnIndex + 1).Value = XLCellValue.FromObject(rows[rowIndex][columnIndex]);
        }
    }

    var stream = new MemoryStream();
    workbook.SaveAs(stream);
    stream.Position = 0;

    return stream;
}

static void PrintSummary(TenderEvaluationResult result)
{
    var tender = result.Tender;

    Console.WriteLine("PackagingTenderTool regulatory scoring sample");
    Console.WriteLine("---------------------------------------------");
    Console.WriteLine("Import source: generated Labels v1 Excel workbook");
    Console.WriteLine($"Tender: {tender.Name}");
    Console.WriteLine($"Profile: {tender.Settings.PackagingProfile}");
    Console.WriteLine($"Currency: {tender.Settings.CurrencyCode}");
    Console.WriteLine($"Imported sample lines: {tender.LabelLineItems.Count}");
    Console.WriteLine("Regulatory references:");
    Console.WriteLine($"  Maximum label weight: {FormatDecimal(tender.Settings.MaximumLabelWeightGrams)} g");
    Console.WriteLine($"  Mono-material design: {FormatExpectedBool(tender.Settings.ExpectedMonoMaterial)}");
    Console.WriteLine($"  Easy separation: {FormatExpectedBool(tender.Settings.ExpectedEasySeparation)}");
    Console.WriteLine($"  Reusable or recyclable material direction: {FormatExpectedBool(tender.Settings.ExpectedReusableOrRecyclableMaterial)}");
    Console.WriteLine($"  Traceability: {FormatExpectedBool(tender.Settings.ExpectedTraceability)}");
    Console.WriteLine();

    foreach (var supplierEvaluation in result.SupplierEvaluations)
    {
        Console.WriteLine($"Supplier: {DisplaySupplierName(supplierEvaluation.SupplierName)}");
        Console.WriteLine($"  Total spend: {FormatMoney(supplierEvaluation.TotalSpend, tender.Settings.CurrencyCode)}");
        Console.WriteLine($"  Manual review required: {FormatYesNo(supplierEvaluation.RequiresManualReview)}");
        Console.WriteLine($"  Line evaluations: {supplierEvaluation.LineEvaluations.Count}");
        Console.WriteLine($"  Manual review flags: {supplierEvaluation.ManualReviewFlags.Count}");
        Console.WriteLine($"  Scores: {FormatScoreBreakdown(supplierEvaluation.ScoreBreakdown)}");
        Console.WriteLine($"  Classification: {FormatClassification(supplierEvaluation.Classification)}");
        Console.WriteLine();
    }
}

static string DisplaySupplierName(string supplierName)
{
    return string.IsNullOrWhiteSpace(supplierName)
        ? "(missing supplier)"
        : supplierName;
}

static string FormatMoney(decimal amount, string currencyCode)
{
    return $"{amount.ToString("0.00", CultureInfo.InvariantCulture)} {currencyCode}";
}

static string FormatYesNo(bool value)
{
    return value ? "Yes" : "No";
}

static string FormatScoreBreakdown(ScoreBreakdown scoreBreakdown)
{
    return $"Commercial={FormatScore(scoreBreakdown.Commercial)}, "
        + $"Technical={FormatScore(scoreBreakdown.Technical)}, "
        + $"Regulatory={FormatScore(scoreBreakdown.Regulatory)}, "
        + $"Total={FormatScore(scoreBreakdown.Total)}";
}

static string FormatScore(decimal? score)
{
    return score.HasValue
        ? score.Value.ToString("0.##", CultureInfo.InvariantCulture)
        : "n/a";
}

static string FormatDecimal(decimal? value)
{
    return value.HasValue
        ? value.Value.ToString("0.##", CultureInfo.InvariantCulture)
        : "n/a";
}

static string FormatExpectedBool(bool? value)
{
    return value.HasValue ? FormatYesNo(value.Value) : "n/a";
}

static string FormatClassification(SupplierClassification? classification)
{
    return classification?.ToString() ?? "Unclassified";
}
