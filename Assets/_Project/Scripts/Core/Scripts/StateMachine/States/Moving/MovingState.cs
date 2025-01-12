using Constants;
using Core.Scripts.StateMachine.States.Animating;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Core.Scripts.StateMachine.States.Moving
{
    public class MovingState: MonoBehaviour, IPayloadState<MovingStateData>
    {
        [SerializeField] private BaseAnimationState _animationState;
        [SerializeField] private Transform _moveTarget;
        public async UniTask Enter(MovingStateData data)
        {
            _animationState.Enter(new AnimationStateData(data.AnimationKey, AnimationConstants.IdleTrigger)).Forget();
            
            if (data.LookAt)
                await _moveTarget.DOLookAt(data.TargetPoint, 0.5f);
            
            await _moveTarget.DOMove(data.TargetPoint, data.Speed).SetSpeedBased().SetEase(Ease.Linear);
            await _animationState.ManualExit();
        }

        public async UniTask Exit()
        {
            
        }
    }

    public class MovingStateData
    {
        public readonly float Speed;
        public readonly string AnimationKey;
        public readonly bool LookAt;
        public Vector3 TargetPoint;

        public MovingStateData(float speed, string animationKey, Vector3 targetPoint,bool lookAt = true)
        {
            Speed = speed;
            AnimationKey = animationKey;
            TargetPoint = targetPoint;
            LookAt = lookAt;
        }
    }
}