using System;
using System.Collections.Generic;
using System.Linq;
using Core.Scripts.Extension.System;
using Cysharp.Threading.Tasks;
using Mechanics.Characters;
using Mechanics.Companies;
using Mechanics.Product.JSON;
using UnityEngine;
using Zenject;

namespace Mechanics.Product
{
    public class ProductService : IProductService, IInitializable
    {
        private ProductNamesJsonStorage _jsonNamesStorage;
        private ICharactersProvider _charactersProvider;

        public event Action<IGameProductData> OnNewGameProductAdded;
        public event Action<IGameProductData> OnGameProductDeveloped;

        private List<IProductData> _productsInDevelopingStatus = new ();
        
        
        //TODO: Remote
        private const int _baseDevelopTime = 60;
        

        [Inject]
        private void Construct(ICharactersProvider charactersProvider)
        {
            _charactersProvider = charactersProvider;
        }

        public void Initialize()
        {
            _jsonNamesStorage = new ProductNamesJsonStorage();
            _jsonNamesStorage.Load();
        }

        public void CreateGameProduct(CreateGameProductData createGameProductData, string Owner)
        {
            IGameProductData productData = new GameProductData(createGameProductData);

            var character = _charactersProvider.GetCharacterByKey(Owner);
            // if (!character.HasCompany())
                character.AddProduct(productData.Key);
            // else 
                // TODO: Add product to player's company

            OnNewGameProductAdded?.Invoke(productData);
            
            AddDevProductsToDevelopers(productData);
            StartDevelopingGameProduct(productData);
        }

        private void AddDevProductsToDevelopers(IGameProductData productData)
        {
            foreach (var developer in productData.DevelopingData.Developers)
                developer.AddDevelopmentProduct(productData.Key);
        }
        private void RemoveDevProductsToDevelopers(IGameProductData productData)
        {
            foreach (var developer in productData.DevelopingData.Developers)
                developer.RemoveDevelopmentProduct(productData.Key);
        }

        private void StartDevelopingGameProduct(IGameProductData productData)
        {
            RunDevelopingTask(productData).Forget();
        }

        private async UniTask RunDevelopingTask(IGameProductData productData)
        {
            _productsInDevelopingStatus.Add(productData);

            var developingSpeed = GetDevelopingSpeedInSeconds(productData);
            while (productData.DevelopingData.CurrentProgress < 1f)
            {
                productData.DevelopingData.AddProgress(Time.deltaTime / developingSpeed);
                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            RemoveDevProductsToDevelopers(productData);
            productData.ReleaseProduct();
        }

        private float GetDevelopingSpeedInSeconds(IGameProductData productData)
        {
            float result = _baseDevelopTime;
            foreach (var developer in productData.DevelopingData.Developers)
                result -= result / 100 * GetDeveloperTimeBoost(developer);
            return result;
        }

        public void CreateGameProduct(CreateGameProductData createGameProductData, ICompanyData Owner)
        {
            
        }

        public string GetRandomGameNameWithGenre(GameGenre gameGenre, string currentName = "")
        {
            return _jsonNamesStorage
                .Get()
                .GenreNamesList
                .Where(list => list.Genre == gameGenre)
                .ToList().PickRandom()
                .Name;
        }

        private float GetDeveloperTimeBoost(ICharacterData characterData)
        {
            // TODO: Remote
            switch (characterData.CharacterSkill)
            {
                case CharacterSkill.Beginner:
                    return 5f;
                case CharacterSkill.Intermediate:
                    return 8f;
                case CharacterSkill.Advanced:
                    return 15f;
                case CharacterSkill.Expert:
                    return 20f;
                default:
                    return 0f;
            }
            
        }
    }
}