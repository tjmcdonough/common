using Airslip.Common.Types;

namespace Airslip.Common.Utilities.Interfaces
{
    public interface IParsingService<TType>
    {
        ParsingResult<TType> TryParse(string payload);
    }
}