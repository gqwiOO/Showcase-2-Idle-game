using Cysharp.Threading.Tasks;

namespace Core.Scripts.StateMachine.States
{
    public interface IPayloadState<T>: IExitableState
    {
        UniTask Enter(T data);
    }
}