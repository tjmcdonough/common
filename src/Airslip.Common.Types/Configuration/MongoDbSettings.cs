using JetBrains.Annotations;

namespace Airslip.Common.Types.Configuration
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public record MongoDbSettings
    {
        public string ConnectionString { get; init; } = string.Empty;
        public string DatabaseName { get; init; } = string.Empty;
    }
}