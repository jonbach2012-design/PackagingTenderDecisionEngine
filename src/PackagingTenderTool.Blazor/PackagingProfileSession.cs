using PackagingTenderTool.Core.Models;

namespace PackagingTenderTool.Blazor;

/// <summary>
/// Packaging profile, linked pillar weights (always sum 100), and advanced constraint limits.
/// </summary>
public sealed class PackagingProfileSession
{
    public const string ProfileLabels = "Labels";

    public string? SelectedProfile { get; private set; }

    public int Commercial { get; private set; } = 30;

    public int Technical { get; private set; } = 30;

    public int Regulatory { get; private set; } = 40;

    public decimal MaxCo2Impact { get; private set; } = 2.25m;

    public decimal MaxLeadTimeDays { get; private set; } = 30m;

    /// <summary>Always 100 when weights are maintained via linked sliders.</summary>
    public int PillarSum => Commercial + Technical + Regulatory;

    public event Action? Changed;

    public void SelectLabels()
    {
        if (SelectedProfile is not null)
        {
            return;
        }

        SelectedProfile = ProfileLabels;
        Notify();
    }

    public void SetCommercial(int value) => ApplyPrimaryWeight(value, which: 0);

    public void SetTechnical(int value) => ApplyPrimaryWeight(value, which: 1);

    public void SetRegulatory(int value) => ApplyPrimaryWeight(value, which: 2);

    /// <summary>Sets one pillar to <paramref name="value"/> and splits the remainder across the other two by prior ratio.</summary>
    private void ApplyPrimaryWeight(int value, int which)
    {
        value = Math.Clamp(value, 0, 100);
        var remainder = 100 - value;

        switch (which)
        {
            case 0:
                Commercial = value;
                (Technical, Regulatory) = DistributeRemaining(remainder, Technical, Regulatory);
                break;
            case 1:
                Technical = value;
                (Commercial, Regulatory) = DistributeRemaining(remainder, Commercial, Regulatory);
                break;
            default:
                Regulatory = value;
                (Commercial, Technical) = DistributeRemaining(remainder, Commercial, Technical);
                break;
        }

        Notify();
    }

    /// <summary>Split <paramref name="remainder"/> across two pillars, proportional to their previous values.</summary>
    private static (int first, int second) DistributeRemaining(int remainder, int prevFirst, int prevSecond)
    {
        remainder = Math.Clamp(remainder, 0, 100);
        var denom = prevFirst + prevSecond;
        if (denom <= 0)
        {
            var half = remainder / 2;
            return (half, remainder - half);
        }

        var first = (int)Math.Round(remainder * (prevFirst / (decimal)denom));
        first = Math.Clamp(first, 0, remainder);
        var second = remainder - first;
        return (first, second);
    }

    public void SetMaxCo2Impact(decimal value)
    {
        MaxCo2Impact = Math.Clamp(
            value,
            LabelTenderAdvancedConstraints.Co2SliderMin,
            LabelTenderAdvancedConstraints.Co2SliderMax);
        Notify();
    }

    public void SetMaxLeadTimeDays(decimal value)
    {
        MaxLeadTimeDays = Math.Clamp(
            value,
            LabelTenderAdvancedConstraints.LeadSliderMin,
            LabelTenderAdvancedConstraints.LeadSliderMax);
        Notify();
    }

    private void Notify() => Changed?.Invoke();
}
