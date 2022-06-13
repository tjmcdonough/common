using Microsoft.Extensions.Configuration;
using System;

namespace Airslip.Common.Functions.Extensions
{
    public static class Extensions
    {
        static readonly string EnvironmentName = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") ?? "Production";

        public static IConfigurationBuilder AddDefaultConfig(this IConfigurationBuilder builder, string[] args)
        {
            return builder
                .AddJsonFile("appSettings.json", false)
                .AddJsonFile($"appSettings.{EnvironmentName}.json", true)
                .AddJsonFile("appSettings.local.json", true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);
        }
    }   
}