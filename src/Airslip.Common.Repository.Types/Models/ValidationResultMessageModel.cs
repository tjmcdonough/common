namespace Airslip.Common.Repository.Types.Models;

public record ValidationResultMessageModel(string FieldName, string Message)
{
    public string? ErrorCode { get; init; }
};