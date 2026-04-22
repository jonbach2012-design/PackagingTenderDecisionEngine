using PackagingTenderTool.Core.Analytics;

namespace PackagingTenderTool.Core.Models;

public sealed class TenderProject
{
    public string BaseCurrencyCode { get; set; } = "EUR";

    public TenderEvaluationResult Result { get; set; } = new();

    public CtrWeights CtrWeights { get; set; } = new();

    /// <summary>
    /// Optional manual supplier-level technical scores (0-100) used by CTR while awaiting a fixed rubric.
    /// Key: SupplierName.
    /// </summary>
    public Dictionary<string, decimal> ManualTechnicalScores { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}

