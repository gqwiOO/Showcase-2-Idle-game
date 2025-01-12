using System.Threading.Tasks;
using Core.Scenes;
using Services.ServiceInitializer;
using UnityEngine;
using Zenject;

namespace Mechanics
{
    public class Bootstrap: MonoBehaviour
    {
        private IServiceInitializer _serviceInitializer;

        [Inject]
        private void Construct(IServiceInitializer serviceInitializer)
        {
            _serviceInitializer = serviceInitializer;
        }

        private async void Awake()
        {
            Application.targetFrameRate = 60;
            await Task.Run(() => _serviceInitializer.Init());
            await Task.Delay(500);
            await ScenesProvider.LoadScene(GameScene.Main);
        }
    }
}