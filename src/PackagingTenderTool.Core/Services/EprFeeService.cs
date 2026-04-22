using PackagingTenderTool.Core.Models;

namespace PackagingTenderTool.Core.Services;

public sealed class EprFeeService : IEprFeeService
{
    public static readonly IReadOnlyList<string> SupportedCountries = ["DK", "SE", "NO", "FI", "IE"];

    public static readonly IReadOnlyList<string> CoreCategories =
    [
        "Labels",
        "Cardboard",
        "Trays",
        "Packaging Mixed",
        "Flexibles"
    ];

    private readonly IReadOnlyList<EprRate> rates;

    public EprFeeService()
        : this(CreatePlaceholderRates())
    {
    }

    public EprFeeService(IEnumerable<EprRate> rates)
    {
        ArgumentNullException.ThrowIfNull(rates);
        this.rates = rates.ToList();
    }

    public IReadOnlyList<EprRate> GetRates() => rates;

    public decimal CalculateFee(string country, string category, decimal weightKg)
    {
        if (!TryCalculateFee(country, category, weightKg, out var fee, out var flag))
        {
            throw new ArgumentException(flag?.Reason ?? "EPR fee could not be calculated.");
        }

        return fee;
    }

    public bool TryCalculateFee(
        string country,
        string category,
        decimal weightKg,
        out decimal fee,
        out ManualReviewFlag? manualReviewFlag)
    {
        fee = 0m;
        manualReviewFlag = null;

        if (string.IsNullOrWhiteSpace(country))
        {
            manualReviewFlag = new ManualReviewFlag
            {
                FieldName = "EprCountry",
                Reason = "EPR country is missing.",
                Severity = ManualReviewSeverity.Warning
            };
            return false;
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            manualReviewFlag = new ManualReviewFlag
            {
                FieldName = "EprCategory",
                Reason = "EPR category is missing.",
                Severity = ManualReviewSeverity.Warning
            };
            return false;
        }

        if (weightKg < 0m)
        {
            manualReviewFlag = new ManualReviewFlag
            {
                FieldName = "WeightKg",
                SourceValue = weightKg.ToString("G"),
                Reason = "Weight cannot be negative for EPR fee calculation.",
                Severity = ManualReviewSeverity.Error
            };
            return false;
        }

        if (weightKg == 0m)
        {
            fee = 0m;
            return true;
        }

        var normalizedCountry = country.Trim().ToUpperInvariant();
        var normalizedCategory = NormalizeCategory(category);

        var rate = rates.FirstOrDefault(r =>
            string.Equals(r.CountryCode, normalizedCountry, StringComparison.OrdinalIgnoreCase)
            && string.Equals(r.Category, normalizedCategory, StringComparison.OrdinalIgnoreCase));

        if (rate is null)
        {
            manualReviewFlag = new ManualReviewFlag
            {
                FieldName = "EprRate",
                SourceValue = $"{normalizedCountry}|{normalizedCategory}",
                Reason = $"Missing EPR rate for country '{normalizedCountry}' and category '{normalizedCategory}'.",
                Severity = ManualReviewSeverity.Warning
            };
            return false;
        }

        fee = decimal.Round(weightKg * rate.RatePerKg, 4);
        return true;
    }

    private static string NormalizeCategory(string category)
    {
        var trimmed = category.Trim();

        return trimmed.Equals("PackagingMixed", StringComparison.OrdinalIgnoreCase)
            ? "Packaging Mixed"
            : trimmed;
    }

    private static IReadOnlyList<EprRate> CreatePlaceholderRates()
    {
        // Placeholder values only. Replace with Scandi Standard data later.
        const decimal low = 0.10m;
        const decimal mid = 0.50m;
        const decimal high = 1.20m;

        decimal RateForCategory(string category) => category switch
        {
            "Cardboard" => low,
            "Labels" => mid,
            "Trays" => mid,
            "Packaging Mixed" => mid,
            "Flexibles" => high,
            _ => mid
        };

        return SupportedCountries
            .SelectMany(country => CoreCategories.Select(category => new EprRate
            {
                CountryCode = country,
                Category = category,
                RatePerKg = RateForCategory(category)
            }))
            .ToList();
    }
}

