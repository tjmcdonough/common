using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
using Microsoft.ApplicationInsights.Extensibility;
using System;

namespace Airslip.Common.Monitoring;

public static class Helpers
{
    public static TelemetryClient? GetTelemetryClient()
    {
        string? instrumentationKey = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY") ?? null;
        if (string.IsNullOrEmpty(instrumentationKey)) return null;
        
        TelemetryConfiguration telemetryConfiguration = new();
        telemetryConfiguration.InstrumentationKey = instrumentationKey;
        telemetryConfiguration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
        telemetryConfiguration.TelemetryInitializers.Add(new AzureAppServiceRoleNameFromHostNameHeaderInitializer());
        
        return new TelemetryClient(telemetryConfiguration);
    }
}