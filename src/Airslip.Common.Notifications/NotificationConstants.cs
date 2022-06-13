namespace Airslip.Common.Notifications
{
    public static class NotificationConstants
    {
        public static string UnsupportedMessage(params string[] devices) => $"The supported devices are {devices}";
    }
}