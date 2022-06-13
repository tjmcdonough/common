using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Types.Enums;
using System.Collections.Generic;
using System.Security.Claims;

namespace Airslip.Common.Auth.Models
{
    public class GenerateQrCodeToken : IGenerateToken
    {
        public GenerateQrCodeToken(string entityId, string storeId, string checkoutId, string qrCodeKey, AirslipUserType airslipUserType)
        {
            EntityId = entityId;
            StoreId = storeId;
            CheckoutId = checkoutId;
            QrCodeKey = qrCodeKey;
            AirslipUserType = airslipUserType;
        }

        public string EntityId { get; init; }
        public string StoreId { get; init; }
        public string CheckoutId { get; init; }
        public string QrCodeKey { get; init; }

        public AirslipUserType AirslipUserType { get; init; }
        public List<Claim> GetCustomClaims(TokenEncryptionSettings settings)
        {
            List<Claim> claims = new()
            {
                new Claim(AirslipClaimTypes.STORE_ID, StoreId.Encrypt(settings)),
                new Claim(AirslipClaimTypes.CHECKOUT_ID, CheckoutId.Encrypt(settings)),
                new Claim(AirslipClaimTypes.QR_CODE_KEY, QrCodeKey.Encrypt(settings))
            };

            return claims;
        }
    }
}