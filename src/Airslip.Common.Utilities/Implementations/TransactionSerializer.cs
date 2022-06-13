using Airslip.Common.Utilities.Interfaces;
using System.Text.RegularExpressions;

namespace Airslip.Common.Utilities.Implementations
{
    public class TransactionSerializer : ITransactionSerializer
    {
        public string? GetLastCardDigits(string? maskedPanNumber)
        {
            if (maskedPanNumber is null)
                return null;

            int maskedPanNumberLength = maskedPanNumber.Length;

            if (maskedPanNumberLength < 4)
                return null;

            string regexPattern = maskedPanNumber.Contains("x-") ? "[^x-]*$" : "[^*]*$";

            Match regexSearchForLastNumbers = Regex.Match(maskedPanNumber, regexPattern);
            
            return regexSearchForLastNumbers.Value;
        }
    }
}