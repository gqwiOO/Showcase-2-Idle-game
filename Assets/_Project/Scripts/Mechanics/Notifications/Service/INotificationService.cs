using System;

namespace Mechanics.Notifications
{
    public interface INotificationService
    {
        event Action<NotificationData> OnNotificationRequested;

        void Show(NotificationType type, string message = null);
    }
}
