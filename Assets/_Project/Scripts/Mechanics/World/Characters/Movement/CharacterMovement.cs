using System;
using Constants;
using Core.Scripts.Extension.DOTWeen;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Mechanics.World.Characters.Movement
{
    public class CharacterMovement : MonoBehaviour, ICharacterMovement
    {
        [SerializeField] private Transform teleportPosition;
        [SerializeField] private Transform movePOs;
        [SerializeField] private Animator animator;
        [SerializeField] private float scaleSpeed;
        [SerializeField] private float walkSpeed;
        public Tween TeleportTo(Vector3 position)
        {
            return transform.ScaleToZeroMoveAndScaleBack(position, Vector3.one, scaleSpeed, scaleSpeed, SetStandPose);
        }

        public async UniTask GoTo(Vector3 position, float speed, Action OnEnded = null)
        {
            animator.SetTrigger(AnimationConstants.WalkTrigger);
            transform.LookAt(position,Vector3.up);
            
            transform.DOMove(position, walkSpeed)
                .SetSpeedBased()
                .OnComplete(() => OnEnded?.Invoke());
        }

        private void SetStandPose()
        {
            animator.SetTrigger(AnimationConstants.StandUpImmediatelyTrigger);
        }

        public async UniTask Start()
        {
            await UniTask.DelaySeconds(1f);
            TeleportTo(teleportPosition.position);
            await UniTask.DelaySeconds(1f);
            GoTo(movePOs.position, walkSpeed, () => animator.SetTrigger(AnimationConstants.IdleTrigger));

        }
    }
}