namespace Airslip.Common.Utilities.Interfaces
{
    public interface ITransactionSerializer
    {
        public string? GetLastCardDigits(string? maskedPanNumber);
    }
}