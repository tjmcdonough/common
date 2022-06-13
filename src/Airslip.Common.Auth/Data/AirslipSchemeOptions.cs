namespace Airslip.Common.Auth.Data
{
    public static class AirslipSchemeOptions
    {
        public const string ApiKeyScheme = "ApiKeyAuthScheme";
        public const string ApiKeyHeaderField = "x-api-key";
        public const string QrCodeAuthScheme = "QrCodeAuthScheme";
        public const string CookieAuthScheme = "CookieAuthScheme";
        public const string JwtBearerScheme = "Bearer";
        public const string JwtBearerHeaderField = "Authorization";
        
        public const string CookieTokenField = "tf";
        public const string CookieEncryptField = "tfe";
        public const string CookieCyclesField = "ec";
        public const string CookieTimestampField = "ts";
        
        public static string ThisEnvironment { get; set; } = "Development";
    }
}