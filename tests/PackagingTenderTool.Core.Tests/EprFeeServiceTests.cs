using PackagingTenderTool.Core.Services;

namespace PackagingTenderTool.Core.Tests;

public sealed class EprFeeServiceTests
{
    [Fact]
    public void CalculateFeeReturnsWeightTimesRate()
    {
        var service = new EprFeeService();

        var fee = service.CalculateFee("DK", "Cardboard", 10m);

        Assert.Equal(1.0m, fee);
    }

    [Fact]
    public void TryCalculateFeeReturnsFlagWhenRateMissing()
    {
        var service = new EprFeeService([]);

        var ok = service.TryCalculateFee("DK", "Labels", 1m, out var fee, out var flag);

        Assert.False(ok);
        Assert.Equal(0m, fee);
        Assert.NotNull(flag);
        Assert.Equal("EprRate", flag!.FieldName);
    }
}

