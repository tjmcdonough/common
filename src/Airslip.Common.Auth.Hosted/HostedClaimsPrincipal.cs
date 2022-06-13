namespace Airslip.Common.Auth.Hosted
{
    public record HostedClaimsPrincipal
    {
        public string CorrelationId { get; init; } = string.Empty;
        public string IpAddress { get;init; }= string.Empty;
        public string EntityId { get; init; }= string.Empty;
        public string UserAgent { get; init; }= string.Empty;
    }
}