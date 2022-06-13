using System;

namespace Airslip.Common.Auth.Exceptions
{
    public class EnvironmentUnsupportedException : Exception
    {
        public string SuppliedEnvironment { get; }
        public string SupportedEnvironment { get; }

        public EnvironmentUnsupportedException(string suppliedEnvironment, string supportedEnvironment)
        {
            SuppliedEnvironment = suppliedEnvironment;
            SupportedEnvironment = supportedEnvironment;
        }
    }
}