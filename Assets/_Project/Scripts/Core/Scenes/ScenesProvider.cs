using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core.Scenes
{
    public static class ScenesProvider
    {
        public static async Task LoadScene(GameScene scene)
        {
            string sceneName = scene.ToString();
            var sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName);

            while (!sceneLoadOperation.isDone)
            {
                await Task.Yield();
            }
        }
    }

    public enum GameScene
    {
        None = 0,
        Main = 1,
        Loading = 2,
    }
}