using System.Collections.Generic;

namespace Airslip.Common.Notifications
{
    public record NotificationRegistration(
        string registrationId,
        string deviceId,
        string deviceToken,
        ISet<string> descriptionTags);
}