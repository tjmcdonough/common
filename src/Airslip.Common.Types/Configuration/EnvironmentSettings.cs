using System;

namespace Airslip.Common.Types.Configuration
{
    public record EnvironmentSettings
    {
        public string EnvironmentName { get; set; } = String.Empty;
    }
}