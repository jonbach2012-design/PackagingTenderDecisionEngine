using PackagingTenderTool.Core.Models;

namespace PackagingTenderTool.Core.Tests;

internal static class TestDataFactory
{
    public static LabelLineItem CreateStandardLabelItem(
        string supplierName = "Acme Labels",
        decimal? spend = 100m,
        decimal? pricePerThousand = 10m)
    {
        return CreateValidLabelLineItem(
            supplierName: supplierName,
            spend: spend,
            pricePerThousand: pricePerThousand,
            countryCode: "DK",
            category: "Labels",
            labelWeightGrams: 100m);
    }

    public static LabelLineItem CreateValidLabelLineItem(
        string supplierName = "Acme Labels",
        decimal? spend = 100m,
        decimal? pricePerThousand = 10m,
        string countryCode = "DK",
        string category = "Labels",
        decimal labelWeightGrams = 100m)
    {
        var item = new LabelLineItem
        {
            SupplierName = supplierName,
            Spend = spend,
            PricePerThousand = pricePerThousand,
            LabelWeightGrams = labelWeightGrams
        };

        item.EprSchemes.Add(new EprSchemeInfo
        {
            CountryCode = countryCode,
            Category = category
        });

        return item;
    }
}

