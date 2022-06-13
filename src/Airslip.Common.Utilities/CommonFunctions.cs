using System;

namespace Airslip.Common.Utilities
{
    public static class CommonFunctions
    {
        public static string GetId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}