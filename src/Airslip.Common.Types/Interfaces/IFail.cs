namespace Airslip.Common.Types.Interfaces
{
    public interface IFail : IResponse
    {
        public string ErrorCode { get; }
    }
}
