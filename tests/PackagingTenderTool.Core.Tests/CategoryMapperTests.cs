using PackagingTenderTool.Core.Services;

namespace PackagingTenderTool.Core.Tests;

public sealed class CategoryMapperTests
{
    [Fact]
    public void MapToSystemCategoryMapsLdpeToFlexibles()
    {
        var mapper = new CategoryMapper(
        [
            new CategoryMapping { SupplierTerm = "LDPE", SystemCategory = "Flexibles" }
        ]);

        Assert.Equal("Flexibles", mapper.MapToSystemCategory("LDPE"));
        Assert.Equal("Flexibles", mapper.MapToSystemCategory("ldpe"));
        Assert.Equal("Flexibles", mapper.MapToSystemCategory("  LDPE "));
    }

    [Fact]
    public void MapToSystemCategoryMapsCommonSynonymsToFlexibles()
    {
        var mapper = new CategoryMapper(
        [
            new CategoryMapping { SupplierTerm = "Soft Plast", SystemCategory = "Flexibles" },
            new CategoryMapping { SupplierTerm = "Plastic", SystemCategory = "Flexibles" }
        ]);

        Assert.Equal("Flexibles", mapper.MapToSystemCategory("Soft Plast"));
        Assert.Equal("Flexibles", mapper.MapToSystemCategory("soft  plast"));
        Assert.Equal("Flexibles", mapper.MapToSystemCategory("Plastic"));
    }

    [Fact]
    public void MapToSystemCategoryUsesFuzzyMatchingWhenNoExactMatchExists()
    {
        var mapper = new CategoryMapper(
        [
            new CategoryMapping { SupplierTerm = "Soft Plast", SystemCategory = "Flexibles" }
        ]);

        Assert.Equal("Flexibles", mapper.MapToSystemCategory("Soft Plastic"));
    }

    [Fact]
    public void MapToSystemCategoryReturnsNullWhenNoMappingExists()
    {
        var mapper = new CategoryMapper(new List<CategoryMapping>());

        Assert.Null(mapper.MapToSystemCategory("UNKNOWN"));
        Assert.Null(mapper.MapToSystemCategory(null));
        Assert.Null(mapper.MapToSystemCategory(" "));
    }
}

