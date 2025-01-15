using Cysharp.Threading.Tasks;

namespace Core.Scripts.Services.Tutorial
{
    public interface ITutorialHandler
    {
        void HandleTutorial(object sender,TutorialType tutorialType);

        void Init();
    }
}