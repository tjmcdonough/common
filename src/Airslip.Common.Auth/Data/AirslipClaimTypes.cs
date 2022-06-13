using System;

namespace Airslip.Common.Auth.Data;

public static class AirslipClaimTypes
{
    public const string USER_ROLE = "userrole";
    public const string APPLCATION_ROLES = "approles";
    public const string USER_ID = "userid";
    public const string YAPILY_USER_ID = "yapilyuserid";
    public const string USER_AGENT = "ua";
    public const string STORE_ID = "storeid";
    public const string CHECKOUT_ID = "checkoutid";
    public const string QR_CODE_KEY = "qrcode";
    public const string IP_ADDRESS = "ip";
    public const string ENTITY_ID = "entityid";
    public const string ENVIRONMENT = "environment";
    public const string CORRELATION = "correlation";
    public const string AIRSLIP_USER_TYPE = "airslipusertype";
    public const string API_KEY = "apikey";
        
    public const string USER_ROLE_SHORT = "r";
    public const string APPLCATION_ROLES_SHORT = "o";
    public const string ENTITY_ID_SHORT = "i";
    public const string AIRSLIP_USER_TYPE_SHORT = "a";
    public const string API_KEY_SHORT = "k";
    public const string ENVIRONMENT_SHORT = "e";
    public const string USER_ID_SHORT = "u";

    public static string MapToLong(this string claimType)
    {
        return claimType switch
            {
                ENTITY_ID_SHORT => ENTITY_ID,
                AIRSLIP_USER_TYPE_SHORT => AIRSLIP_USER_TYPE,
                ENVIRONMENT_SHORT => ENVIRONMENT,
                USER_ID_SHORT => USER_ID,
                API_KEY_SHORT => API_KEY,
                USER_ROLE_SHORT => USER_ROLE,
                APPLCATION_ROLES_SHORT => APPLCATION_ROLES,
                _ => claimType
            };
    }
}