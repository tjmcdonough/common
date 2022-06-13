using JetBrains.Annotations;

namespace Airslip.Common.Types.Enums;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public enum DataSources
{
    Yapily,
    Api2Cart,
    SwanRetail,
    CustomerPortal,
    Analytics,
    QrMatching,
    SmartReceipts,
    MockData,
    Unknown
}