using System.Collections.Generic;
using System.Linq;
using Core.Scripts.Pools;
using Cysharp.Threading.Tasks;
using Mechanics.Characters;
using Mechanics.Hiring.Service;
using Mechanics.Hiring.Views;
using Services.Screen;
using Services.Screen.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Mechanics.Hiring.Screens
{
    public class HiringScreen : BaseScreen
    {
        [SerializeField] 
        private RectTransform _container;
    
        [SerializeField] 
        private RectTransform _noCompanyContainer;
    
        [SerializeField] 
        private RectTransform _withCompanyContainer;

        [ReadOnly]
        [SerializeField]
        private List<HireCharacterItem> _views = new List<HireCharacterItem>();

        [SerializeField]
        private PoolGameObjects _pool;
    
        private IHiringService _hiringService;
        private IMainScreenService _mainScreenService;
        private ICharactersProvider _charactersProvider;

        [Inject]
        private void Construct(IHiringService hiringService, IMainScreenService mainScreenService, ICharactersProvider charactersProvider)
        {
            _charactersProvider = charactersProvider;
            _mainScreenService = mainScreenService;
            _hiringService = hiringService;
        }

        private void Start()
        {
            _hiringService.OnHired += HiringService_OnHired;
        }

        private void HiringService_OnHired(ICharacterData obj)
        {
            _views.FirstOrDefault(item => item.CharacterKey == obj.Key).gameObject.SetActive(false);
        }

        public async UniTask Init()
        {
            _pool.Init();

            if (!_charactersProvider.GetMyCharacter().HasCompany())
            {
                _noCompanyContainer.gameObject.SetActive(true);
                _withCompanyContainer.gameObject.SetActive(false);
            }
            else
            {
                _noCompanyContainer.gameObject.SetActive(false);
                _withCompanyContainer.gameObject.SetActive(true);
            }
            InitViewItems();
        }

        private void InitViewItems()
        {
            var characters = _hiringService.GetAllAvailableCharactersToHire();
            EnsureViewCount(characters.Count);

            for (int i = 0; i < characters.Count; i++)
            {
                _views[i].gameObject.SetActive(true);
                _views[i].Init(characters[i]);
            }

            for (int i = characters.Count; i < _views.Count; i++)
                _views[i].gameObject.SetActive(false);
        }

        private void EnsureViewCount(int requiredCount)
        {
            while (_views.Count < requiredCount)
            {
                var view = GetCharacterItem();
                view.gameObject.SetActive(true);
                view.transform.SetParent(_container, false);
                _views.Add(view);
            }
        }

        private HireCharacterItem GetCharacterItem()
        {
            var result = _pool.Pull().GetOwner<HireCharacterItem>();
            result.OnHire += HireCharacterItem_OnHire;
            return result;
        }

        private void HireCharacterItem_OnHire(ICharacterData data) 
            => _mainScreenService.ShowConfirmHiringScreen(data);
    }
}