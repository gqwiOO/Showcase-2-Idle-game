
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Services.Screen;
using UnityEngine;

namespace Core.Scripts.Services.Tutorial
{
    public class BaseTutorialScreen : BaseScreen
    {
        [field: SerializeField]
        public TutorialType TutorialType { get; private set; }

        [SerializeField]
        private List<GameObject> stages;

        private int currentStageIndex = -1;
        
        public override UniTask Open()
        {
            currentStageIndex = -1;
            NextStage();
            return base.Open();
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                NextStage();
        }

        private void NextStage()
        {
            if (currentStageIndex >= 0 && currentStageIndex < stages.Count)
                stages[currentStageIndex].SetActive(false);

            currentStageIndex++;

            if (currentStageIndex < stages.Count)
                stages[currentStageIndex].SetActive(true);
            else
                Hide().Forget();
        }
    }
}
