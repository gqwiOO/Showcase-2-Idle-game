namespace Mechanics.Notifications
{
    public struct NotificationData
    {
        public NotificationType Type { get; }
        public string Message { get; }

        public NotificationData(NotificationType type, string message = null)
        {
            Type = type;
            Message = message;
        }
    }
}
