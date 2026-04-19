using PackagingTenderTool.Core.Models;
using PackagingTenderTool.Core.Services;

namespace PackagingTenderTool.Core.Tests;

public sealed class LabelDataCleaningServiceTests
{
    [Theory]
    [InlineData("100X169", "100x169")]
    [InlineData("74,2X219", "74.2x219")]
    [InlineData("150X212,5", "150x212.5")]
    public void CleanNormalizesLabelSizes(string sourceValue, string expectedValue)
    {
        var cleaned = new LabelDataCleaningService().Clean(new LabelLineItem
        {
            LabelSize = sourceValue
        });

        Assert.Equal(expectedValue, cleaned.NormalizedLabelSize);
    }

    [Fact]
    public void CleanGroupsColorCountsConservatively()
    {
        var cleaned = new LabelDataCleaningService().Clean(new LabelLineItem
        {
            NumberOfColors = 6
        });

        Assert.Equal("5-6 colors", cleaned.NormalizedColorGroup);
    }

    [Theory]
    [InlineData("Jæren", "Norway")]
    [InlineData("Stokke", "Norway")]
    [InlineData("", "(missing)")]
    public void CleanAddsConservativeCountry(string site, string expectedCountry)
    {
        var cleaned = new LabelDataCleaningService().Clean(new LabelLineItem
        {
            Site = site
        });

        Assert.Equal(expectedCountry, cleaned.Country);
    }
}
