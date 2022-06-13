using Airslip.Common.Services.Consent.Data;
using Airslip.Common.Types.Enums;
using JetBrains.Annotations;

namespace Airslip.Common.Services.Consent.Models
{
    public class MerchantSummaryModel
    {
        public string? IconUrl { [UsedImplicitly] get; set; }
        public string? LogoUrl { [UsedImplicitly] get; set; }
        public string? Id { get; init; }
        public string? Name { get; init; }
        public string CategoryCode { get; set; } = Constants.DEFAULT_CATEGORY_CODE;
        public MerchantTypes Type { get; set; } = MerchantTypes.Unsupported;
    }
}