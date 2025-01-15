using System.Collections.Generic;
using Core.Storage.JsonStorage;

namespace Core.Scripts.Services.Tutorial
{
    public class TutorialsStateJsonStorage: BaseJsonStorage<TutorialsStates>
    {
        public override string Path => "TutorialStates";
    }
    
    [System.Serializable]
    public class TutorialsStates
    {
        public List<TutorialState> TutorialStates;
    }
    
    [System.Serializable]
    public class TutorialState
    {
        public TutorialType TutorialType;
        public bool IsCompleted;
    }
}