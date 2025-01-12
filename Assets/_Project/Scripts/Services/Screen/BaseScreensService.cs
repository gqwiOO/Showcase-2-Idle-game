using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Services.Screen
{
    public class BaseScreensService : SerializedMonoBehaviour
    {
        [SerializeField] private Dictionary<Type, BaseScreen> _screens;

        [Button]
        private void GetScreens()
        {
            _screens = GetComponentsInChildren<BaseScreen>(true)
                .ToDictionary(screen => screen.GetType());
        }

        protected List<BaseScreen> GetAllScreensExceptOf(BaseScreen screen)
        {
            return _screens.Values.Where(item => item != screen).ToList();
        }

        protected T GetScreen<T>() where T : BaseScreen
        {
            _screens.TryGetValue(typeof(T), out var result);
            return result as T;
        }

        protected virtual async UniTask Open(BaseScreen screen) => await screen.Open();
        protected virtual async UniTask Hide(BaseScreen screen) => await screen.Hide();
    }
}