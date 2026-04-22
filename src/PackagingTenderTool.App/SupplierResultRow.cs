using System.Globalization;
using PackagingTenderTool.Core.Models;

namespace PackagingTenderTool.App;

internal sealed class SupplierResultRow
{
    private SupplierResultRow()
    {
    }

    public bool Compare { get; set; }

    public string SupplierName { get; private init; } = string.Empty;

    public decimal TotalSpend { get; private init; }

    public decimal? EprFee { get; set; }

    public decimal? RiskScore { get; set; }

    public decimal? CommercialScore { get; private init; }

    public decimal? TechnicalScore { get; private init; }

    public decimal? RegulatoryScore { get; private init; }

    public decimal? TotalScore { get; private init; }

    public decimal? CtrCommercialScore { get; set; }

    public decimal? CtrTechnicalScore { get; set; }

    public decimal? CtrRegulatoryScore { get; set; }

    public decimal? DecisionScore { get; set; }

    public decimal? ManualTechnicalScore { get; set; }

    public SupplierClassification? Classification { get; private init; }

    public int ManualReviewFlagCount { get; private init; }

    public string CurrencyCode { get; private init; } = "EUR";

    public string Notes { get; private init; } = string.Empty;

    public string TotalSpendDisplay => $"{TotalSpend:N2} {CurrencyCode}";

    public string EprFeeDisplay => EprFee.HasValue ? $"{EprFee.Value:N2} {CurrencyCode}" : "-";

    public string RiskScoreDisplay => RiskScore.HasValue ? $"{RiskScore.Value:N1}" : "-";

    public string CommercialScoreDisplay => FormatScore(CommercialScore);

    public string TechnicalScoreDisplay => FormatScore(TechnicalScore);

    public string RegulatoryScoreDisplay => FormatScore(RegulatoryScore);

    public string TotalScoreDisplay => FormatScore(TotalScore);

    public string DecisionScoreDisplay => FormatScore(DecisionScore);

    public string CtrCommercialScoreDisplay => FormatScore(CtrCommercialScore);

    public string CtrTechnicalScoreDisplay => FormatScore(CtrTechnicalScore);

    public string CtrRegulatoryScoreDisplay => FormatScore(CtrRegulatoryScore);

    public string CtrProfileDisplay =>
        $"C:{FormatShort(CtrCommercialScore)} T:{FormatShort(CtrTechnicalScore)} R:{FormatShort(CtrRegulatoryScore)}";

    public string ManualTechnicalScoreDisplay => ManualTechnicalScore.HasValue ? FormatShort(ManualTechnicalScore) : "-";

    public static SupplierResultRow FromSupplier(SupplierEvaluation supplier, string currencyCode)
    {
        var scoreBreakdown = supplier.ScoreBreakdown ?? new ScoreBreakdown();
        return new SupplierResultRow
        {
            SupplierName = string.IsNullOrWhiteSpace(supplier.SupplierName) ? "(missing supplier)" : supplier.SupplierName,
            TotalSpend = supplier.TotalSpend,
            CommercialScore = scoreBreakdown.Commercial,
            TechnicalScore = scoreBreakdown.Technical,
            RegulatoryScore = scoreBreakdown.Regulatory,
            TotalScore = scoreBreakdown.Total,
            Classification = supplier.Classification,
            ManualReviewFlagCount = supplier.ManualReviewFlags?.Count ?? 0,
            CurrencyCode = string.IsNullOrWhiteSpace(currencyCode) ? "EUR" : currencyCode,
            Notes = BuildNotes(supplier)
        };
    }

    private static string BuildNotes(SupplierEvaluation supplier)
    {
        if (supplier.ManualReviewFlags?.Count > 0)
        {
            var firstFlag = supplier.ManualReviewFlags[0];
            return $"Manual review is required. First flag: {firstFlag.FieldName ?? "Source data"} - {firstFlag.Reason}";
        }

        return supplier.ClassificationReason ??
            "No manual review flags. Classification is based on the provisional total score thresholds.";
    }

    private static string FormatScore(decimal? value)
    {
        return value.HasValue ? value.Value.ToString("N2", CultureInfo.CurrentCulture) : "-";
    }

    private static string FormatShort(decimal? value)
    {
        return value.HasValue ? decimal.Round(value.Value, 0).ToString("N0", CultureInfo.CurrentCulture) : "-";
    }
}
