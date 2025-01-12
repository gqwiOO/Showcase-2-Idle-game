using System;
using DG.Tweening;
using UnityEngine;

namespace Core.Scripts.Extension.DOTWeen
{
    public static class TweenAnimationsExtensions
    {
        public static Tween ScaleToZeroMoveAndScaleBack(this Transform transform, Vector3 worldPosition, Vector3 resultScale,
            float scaleToZeroSpeed = 1f, float scaleBackSpeed = 1f, Action OnZeroScale = null)
        {
            var sequence = DOTween.Sequence();

            sequence.Append(transform.DOScale(Vector3.zero, scaleToZeroSpeed).OnComplete( () =>OnZeroScale?.Invoke()));
            sequence.Append(transform.DOMove(worldPosition, 0f));
            sequence.Append(transform.DOScale(resultScale, scaleBackSpeed));

            return sequence;
        }
    }
}