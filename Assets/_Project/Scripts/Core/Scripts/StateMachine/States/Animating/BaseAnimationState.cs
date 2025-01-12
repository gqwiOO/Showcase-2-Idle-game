using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Scripts.StateMachine.States.Animating
{
    public class BaseAnimationState : MonoBehaviour, IPayloadState<AnimationStateData>
    {
        [SerializeField]
        private Animator _animator;

        private AnimationStateData _data;

        public event Action OnAnimationEnded;

        public virtual async UniTask Enter(AnimationStateData data)
        {
            _data = data;
            if (_animator == null)
                throw new InvalidOperationException("Animator is not assigned.");
            
            await PlayAnimation(data.EnterKey);

            OnAnimationEnded?.Invoke();
        }

        private async UniTask PlayAnimation(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Animation trigger name is invalid.", nameof(key));
            _animator.SetTrigger(key);

            var animationStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            var animationDuration = animationStateInfo.length;

            await UniTask.Delay(TimeSpan.FromSeconds(animationDuration));
        }

        public virtual async UniTask ManualExit() => await Exit();

        public async UniTask Exit() => await PlayAnimation(_data.ExitKey);
    }

    public class AnimationStateData
    {
        public string EnterKey;
        public string ExitKey;

        public AnimationStateData(string enterKey, string exitKey)
        {
            EnterKey = enterKey;
            ExitKey = exitKey;
        }
        public AnimationStateData(string enterKey)
        {
            EnterKey = enterKey;
        }
    }
}