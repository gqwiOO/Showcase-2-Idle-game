using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Services.Screen;
using UnityEngine;

namespace Core.Mechanics.Loading
{
    public class LoadingScreen: BaseScreen
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration;
        
        public override async UniTask Open()
        {
            await base.Open();
            _canvasGroup.alpha = 0;
            await _canvasGroup.DOFade(1f,_fadeDuration).AsyncWaitForKill();
        }
    }
}