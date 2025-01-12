using Core.Storage.JsonStorage;

namespace Mechanics.GameSession
{
    public class GameSessionJsonStorage: BaseJsonStorage<GameSession>
    {
        public GameSessionJsonStorage(string path)
        {
            Path = path;
        }

        public override string Path { get;}
    }
}