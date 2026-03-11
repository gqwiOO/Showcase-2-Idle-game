using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
namespace Mechanics.Notifications
{
    public class NotificationItemView : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private CanvasGroup canvasGroup;

        public void Show(string message, float displayDuration, float fadeDuration)
        {
            text.text = message;
            canvasGroup.alpha = 1f;
            RunHideAfterDelay(displayDuration, fadeDuration).Forget();
        }

        private async UniTaskVoid RunHideAfterDelay(float displayDuration, float fadeDuration)
        {
            await UniTask.DelaySeconds(displayDuration);
            await canvasGroup.DOFade(0f, fadeDuration).AsyncWaitForKill();
            Destroy(gameObject);
        }
    }
}
