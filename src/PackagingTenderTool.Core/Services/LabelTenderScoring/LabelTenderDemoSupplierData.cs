using PackagingTenderTool.Core.Models;

namespace PackagingTenderTool.Core.Services.LabelTenderScoring;

public static class LabelTenderDemoSupplierData
{
    public static IReadOnlyList<SupplierModel> Create()
    {
        return
        [
            new SupplierModel
            {
                SupplierName = "NordPack Solutions",
                Price = 12.40m,
                Co2Impact = 2.30m,
                DeliveryTimeDays = 14m,
                Country = "DK",
                SiteCount = 3,
                CommercialScore = 83m,
                TechnicalScore = 78m,
                RegulatoryScore = 72m
            },
            new SupplierModel
            {
                SupplierName = "GreenWrap Nordic",
                Price = 11.90m,
                Co2Impact = 1.85m,
                DeliveryTimeDays = 12m,
                Country = "SE",
                SiteCount = 2,
                CommercialScore = 69m,
                TechnicalScore = 70m,
                RegulatoryScore = 64m
            },
            new SupplierModel
            {
                SupplierName = "FlexiForm Europe",
                Price = 10.75m,
                Co2Impact = 2.90m,
                DeliveryTimeDays = 25m,
                Country = "DE",
                SiteCount = 4,
                CommercialScore = 76m,
                TechnicalScore = 61m,
                RegulatoryScore = 82m
            },
            new SupplierModel
            {
                SupplierName = "ScanLabel Systems",
                Price = 13.10m,
                Co2Impact = 2.05m,
                DeliveryTimeDays = 22m,
                Country = "NO",
                SiteCount = 2,
                CommercialScore = 48m,
                TechnicalScore = 52m,
                RegulatoryScore = 44m
            }
        ];
    }
}
