using Airslip.Common.Types.Enums;

namespace Airslip.Integrations.Banking.Types.Requests
{
    public class ConsentRequest
    {
        public AirslipUserType AirslipUserType { get; set; } = AirslipUserType.Standard;
        public string? EntityId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string BankingBankId { get; set; } = string.Empty;
        public string? CallbackUrl { get; set; }
    }
}