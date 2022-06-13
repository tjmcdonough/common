namespace Airslip.Common.Services.Consent.Data
{
    public static class EndPoints
    {
        public static string GetAccounts(string baseUri) 
            => $"{baseUri}/accounts";
        public static string GetAccount(string baseUri, string accountId) 
            => $"{baseUri}/accounts/{accountId}";
        public static string GetTransactions(string baseUri, string? accountId = null) 
            => accountId == null ? $"{baseUri}/transactions" : $"{baseUri}/transactions/{accountId}";
    }
}