using Core.Scripts.StateMachine;
using UnityEngine;

namespace Mechanics.World.Characters.AI
{
    public class CharacterBehaviour: MonoBehaviour
    {
        [SerializeField] private BaseStateMachine _stateMachine;
        
        private void Start()
        {
            // var stateData = new MovingStateData(1f, AnimationConstants.WalkTrigger, Vector3.zero);
            // _stateMachine.Enter<MovingState,MovingStateData>(stateData);
        }
    }
}