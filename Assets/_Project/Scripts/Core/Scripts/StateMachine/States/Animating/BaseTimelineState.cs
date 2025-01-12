using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace Core.Scripts.StateMachine.States.Animating
{
    public class BaseTimelineState : MonoBehaviour, IState
    {
        [SerializeField]
        private PlayableDirector _timeLinePlayableDirector;

        public event Action OnTimelineEnded;

        public virtual async UniTask Enter()
        {
            if (_timeLinePlayableDirector == null)
                throw new InvalidOperationException("PlayableDirector is not assigned.");

            _timeLinePlayableDirector.Play();

            await UniTask.WaitUntil(() => 
                _timeLinePlayableDirector.state != PlayState.Playing);

            OnTimelineEnded?.Invoke();
        }

        public async UniTask Exit()
        {
            
        }
    }
}