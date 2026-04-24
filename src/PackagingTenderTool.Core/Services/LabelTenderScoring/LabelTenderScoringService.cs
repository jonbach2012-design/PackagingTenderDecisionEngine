using PackagingTenderTool.Core.Models;

namespace PackagingTenderTool.Core.Services.LabelTenderScoring;

public sealed class LabelTenderScoringService
{
    private readonly ILabelTenderScoringStrategy _strategy;

    public LabelTenderScoringService(ILabelTenderScoringStrategy strategy)
    {
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    public IReadOnlyList<LabelTenderSupplierScore> ScoreSuppliers(
        IReadOnlyList<SupplierModel> suppliers,
        LabelTenderWeights weights,
        LabelTenderAdvancedConstraints constraints)
    {
        return _strategy.Score(suppliers, weights, constraints);
    }
}

