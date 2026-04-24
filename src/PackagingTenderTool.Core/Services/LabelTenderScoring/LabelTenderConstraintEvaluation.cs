using PackagingTenderTool.Core.Models;

namespace PackagingTenderTool.Core.Services.LabelTenderScoring;

public static class LabelTenderConstraintEvaluation
{
    public const string Co2ExceedsLimitMessage = "CO2 exceeds limit";

    public const string LeadTimeTooLongMessage = "Lead time too long";

    /// <summary>
    /// Builds a new warning list from the current supplier values and slider limits (no carry-over from prior runs).
    /// </summary>
    public static List<string> CollectWarningReasons(
        SupplierModel supplier,
        LabelTenderAdvancedConstraints constraints)
    {
        ArgumentNullException.ThrowIfNull(supplier);
        ArgumentNullException.ThrowIfNull(constraints);

        var reasons = new List<string>();
        if (supplier.Co2Impact > constraints.MaxCo2Impact)
        {
            reasons.Add(Co2ExceedsLimitMessage);
        }

        if (supplier.DeliveryTimeDays > constraints.MaxLeadTimeDays)
        {
            reasons.Add(LeadTimeTooLongMessage);
        }

        return reasons;
    }
}
