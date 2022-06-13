using Newtonsoft.Json;

namespace Airslip.Common.Notifications
{
    internal class AppleNotificationPayload
    {
        [JsonProperty(PropertyName = "aps")]
        internal ApplePayload Payload { get; }

        internal AppleNotificationPayload(string message)
        {
            Payload = new ApplePayload(message);
        }
        
        internal class ApplePayload
        {
            [JsonProperty(PropertyName = "alert")]
            internal string Message { get; }

            internal ApplePayload(string message)
            {
                Message = message;
            }
        }
    }
}