using Newtonsoft.Json;

namespace Airslip.Common.Notifications
{
    internal class AndroidNotificationPayload 
    {
        [JsonProperty(PropertyName = "data")]
        internal AndroidPayload Payload { get; }

        internal AndroidNotificationPayload(string message)
        {
            Payload = new AndroidPayload(message);
        }
        
        internal class AndroidPayload
        {
            internal string Message { get; }

            internal AndroidPayload(string message)
            {
                Message = message;
            }
        }
    }
}