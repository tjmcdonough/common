using System.Collections.Generic;

namespace Airslip.Common.Types.Interfaces
{
    public interface ISettingWithDictionary<TType>
    {
        Dictionary<string, TType>? Settings { get; set; }
    }
}