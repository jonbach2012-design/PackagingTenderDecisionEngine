using PackagingTenderTool.Core.Models;

namespace PackagingTenderTool.Core.Services.LabelTenderScoring;

public interface ILabelTenderScoringStrategy
{
    IReadOnlyList<LabelTenderSupplierScore> Score(
        IReadOnlyList<SupplierModel> suppliers,
        LabelTenderWeights weights,
        LabelTenderAdvancedConstraints constraints);
}

