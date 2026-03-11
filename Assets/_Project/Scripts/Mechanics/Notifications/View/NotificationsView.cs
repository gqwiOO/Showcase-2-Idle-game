using UnityEngine;
using Zenject;

namespace Mechanics.Notifications
{
    public class NotificationsView : MonoBehaviour
    {
        [SerializeField] private NotificationItemView itemPrefab;
        [SerializeField] private RectTransform content;
        [SerializeField] private float displayDuration = 5f;
        [SerializeField] private float fadeDuration = 0.3f;

        private INotificationService _notificationService;

        [Inject]
        private void Construct(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        private void OnEnable()
        {
            _notificationService.OnNotificationRequested += OnNotificationRequested;
        }

        private void OnDisable()
        {
            _notificationService.OnNotificationRequested -= OnNotificationRequested;
        }

        private void OnNotificationRequested(NotificationData data)
        {
            var item = Instantiate(itemPrefab, content);
            item.gameObject.SetActive(true);
            item.Show(data.Message, displayDuration, fadeDuration);
        }
    }
}
