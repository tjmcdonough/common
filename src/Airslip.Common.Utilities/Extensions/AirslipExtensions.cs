using Airslip.Common.Types.Enums;
using System;

namespace Airslip.Common.Utilities.Extensions
{
    public class AirslipExtensions
    {
        public static PosProviders? GetPosProvider(string name)
        {
            bool canParse = Enum.TryParse(name, out PosProviders posProvider);

            return canParse ? posProvider : null;
        }
    }
}