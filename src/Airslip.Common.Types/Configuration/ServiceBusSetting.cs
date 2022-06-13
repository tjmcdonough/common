namespace Airslip.Common.Types.Configuration
{
    public class ServiceBusSetting
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string QueueName { get; set; } = string.Empty;
        
        public int ProcessingConcurrency { get; set; } = 1;
    }
}