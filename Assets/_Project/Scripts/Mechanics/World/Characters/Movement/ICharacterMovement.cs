using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Mechanics.World.Characters.Movement
{
    public interface ICharacterMovement
    {
        Tween TeleportTo(Vector3 position);
        UniTask GoTo(Vector3 position, float speed, Action OnEnded = null);
    }
}