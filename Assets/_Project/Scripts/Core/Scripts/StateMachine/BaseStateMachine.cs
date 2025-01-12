using System;
using System.Collections.Generic;
using Core.Scripts.StateMachine.States;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Scripts.StateMachine
{
    public class BaseStateMachine: SerializedMonoBehaviour
    {
        [SerializeField]
        private List<IExitableState> _statesList;
        
        [FormerlySerializedAs("_hasBaseState")] [SerializeField]
        private bool _hasStartState;
        
        [ShowIf(nameof(_hasStartState))]
        [SerializeField] 
        private IState _startState;
        
        private readonly Dictionary<Type, IExitableState> _states = new();
        private IExitableState _currentState;

        private void Awake() => Init();

        private void Start()
        {
            if (_hasStartState)
                Enter(_startState).Forget();
        }

        private void Init()
        {
            _states.Clear();
            foreach (var state in _statesList)
                _states.Add(state.GetType(), state);
        }

        public async UniTask Enter<T>() where T : class, IState => await ChangeState<T>();

        public async UniTask Enter<T, TPayload>(TPayload payload) where T : class, IPayloadState<TPayload> => await ChangeState<T, TPayload>(payload);
        
        public async UniTask Enter(IState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            _currentState?.Exit();

            _currentState = state;
            await state.Enter();
        }

        public void Exit()
        {
            (_currentState as IExitableState)?.Exit();
            _currentState = null;
        }

        private async UniTask ChangeState<T>() where T : class, IState
        {
            _currentState?.Exit();

            if (_states.TryGetValue(typeof(T), out var newState) && newState is T state)
            {
                _currentState = state;
                await state.Enter();
            }
            else
                throw new InvalidOperationException($"State of type {typeof(T)} is not registered.");
        }

        private async UniTask ChangeState<T, TPayload>(TPayload payload) where T : class, IPayloadState<TPayload>
        {
            _currentState?.Exit();

            if (_states.TryGetValue(typeof(T), out var newState) && newState is T state)
            {
                _currentState = state;
                await state.Enter(payload);
            }
            else
            {
                throw new InvalidOperationException($"State of type {typeof(T)} is not registered.");
            }
        }
    }
}