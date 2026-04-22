using PackagingTenderTool.Core.Models;

namespace PackagingTenderTool.Core.Services;

public interface IEprFeeService
{
    /// <summary>
    /// Calculates EPR fee using: WeightKg * RatePerKg(country, category).
    /// Throws if the rate is missing or inputs are invalid.
    /// </summary>
    decimal CalculateFee(string country, string category, decimal weightKg);

    /// <summary>
    /// Same calculation as CalculateFee, but returns a ManualReviewFlag when
    /// a lookup cannot be performed (missing rate / invalid inputs).
    /// </summary>
    bool TryCalculateFee(
        string country,
        string category,
        decimal weightKg,
        out decimal fee,
        out ManualReviewFlag? manualReviewFlag);

    IReadOnlyList<EprRate> GetRates();
}

