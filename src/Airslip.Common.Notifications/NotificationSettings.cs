using System.Collections.Generic;

namespace Airslip.Common.Notifications
{
    public class NotificationSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string HubPath { get; set; } = string.Empty;
        public bool EnableTestSend { get; set; } = false;
        public Dictionary<string, string> Headers { get; set; } = new();
    }
}