using Cysharp.Threading.Tasks;

namespace Core.Scripts.StateMachine.States
{
    public interface IState: IExitableState
    {
        UniTask Enter();
    }
}