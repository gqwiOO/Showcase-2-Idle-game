using System;
using System.Collections.Generic;
using Services.Screen.Interfaces;
using UnityEngine;
using Zenject;

namespace Core.Scripts.Services.Tutorial
{
    public class TutorialHandler : MonoBehaviour, ITutorialHandler
    {
        private ITutorialService _tutorialService;
        private IMainScreenService _mainScreenService;
        private Dictionary<TutorialType,BaseTutorialScreen> _screens = new();

        [Inject]
        private void Construct(ITutorialService tutorialService, IMainScreenService mainScreenService)
        {
            _mainScreenService = mainScreenService;
            _tutorialService = tutorialService;
        }

        private void Awake() => Init();

        public void Init()
        {
            _tutorialService.OnTutorialInvoked += HandleTutorial;
            
            foreach (var screen in _mainScreenService.GetAllTutorialScreen())
                _screens.TryAdd(screen.TutorialType, screen);
        }

        public void HandleTutorial(object sender, TutorialType tutorialType)
        {
            _screens.TryGetValue(tutorialType, out BaseTutorialScreen screen);
            screen?.Open();
        }
    }
}