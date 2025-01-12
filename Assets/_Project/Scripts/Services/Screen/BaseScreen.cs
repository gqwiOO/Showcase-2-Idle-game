using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Services.Screen
{
    public abstract class BaseScreen : MonoBehaviour
    {

        private const string SHOW_ID = "Show";
        private const string HIDE_ID = "Hide";

        public virtual async UniTask Open()
            => gameObject.SetActive(true);

        public virtual async UniTask Hide()
            => gameObject.SetActive(false);
    }
}