using System;
using System.Threading.Tasks;
using Mechanics;
using PimDeWitte.UnityMainThreadDispatcher;

namespace Core.Scripts.Services.Tutorial
{
    public interface ITutorialService: IService
    {
        void InvokeTutorial(TutorialType tutorialType);
        void InvokeTutorialsSequence(params TutorialType[] tutorialTypes);
        
        event System.EventHandler<TutorialType> OnTutorialInvoked;
        event System.EventHandler<TutorialType> OnTutorialCompleted;
        
        void InvokeStartTutorial();
        void CompleteTutorial(TutorialType tutorialType);
    }

    public class TutorialService : ITutorialService
    {
        private TutorialsStateJsonStorage _statesStorage;
        private TutorialsStates _statesData;

        public async void InvokeTutorialsSequence(params TutorialType[] tutorialTypes)
        {
            foreach (var tutorialType in tutorialTypes)
            {
                var tcs = new TaskCompletionSource<bool>();
                EventHandler<TutorialType> handler = null;
                handler = (sender, type) =>
                {
                    if (type == tutorialType)
                    {
                        OnTutorialCompleted -= handler;
                        tcs.SetResult(true);
                    }
                };

                OnTutorialCompleted += handler;
                InvokeTutorial(tutorialType);
                await tcs.Task;
            }
        }

        public event EventHandler<TutorialType> OnTutorialInvoked;
        public event EventHandler<TutorialType> OnTutorialCompleted;

        
        public async Task Init()
        {
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(() => _statesStorage.Load());
            _statesData = _statesStorage.Get();
        }

        public void InvokeTutorial(TutorialType tutorialType)
        {
            OnTutorialInvoked?.Invoke(this,tutorialType);
        }

        public void InvokeStartTutorial()
        {
            InvokeTutorial(TutorialType.Basic);
        }

        public void CompleteTutorial(TutorialType tutorialType)
        {
            OnTutorialCompleted?.Invoke(this, tutorialType);
        }
    }

    public enum TutorialType
    {
        None = 0,
        Basic = 1,
    }
}