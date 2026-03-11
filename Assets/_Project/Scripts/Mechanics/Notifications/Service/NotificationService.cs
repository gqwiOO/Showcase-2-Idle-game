using System;

namespace Mechanics.Notifications
{
    public class NotificationService : INotificationService
    {
        private static readonly string[] DefaultMessages =
        {
            "Contract completed successfully",
            "Contract failed",
            "Employee improved his skill."
        };

        public event Action<NotificationData> OnNotificationRequested;

        public void Show(NotificationType type, string message = null)
        {
            var text = !string.IsNullOrEmpty(message) ? message : DefaultMessages[(int)type];
            OnNotificationRequested?.Invoke(new NotificationData(type, text));
        }
    }
}
