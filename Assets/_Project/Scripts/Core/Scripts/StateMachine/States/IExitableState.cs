using Cysharp.Threading.Tasks;

namespace Core.Scripts.StateMachine.States
{
    public interface IExitableState
    {
        UniTask Exit();
    }
}