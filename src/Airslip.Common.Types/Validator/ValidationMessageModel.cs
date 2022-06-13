namespace Airslip.Common.Types.Validator
{
    public record ValidationMessageModel(string FieldName, string ErrorCode, string Message);
}