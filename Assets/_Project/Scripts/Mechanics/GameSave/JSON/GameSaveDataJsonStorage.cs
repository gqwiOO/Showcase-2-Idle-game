using Core.Storage.JsonStorage;

namespace Mechanics.GameSave
{
    public class GameSaveDataJsonStorage: BaseJsonStorage<GameData>
    {
        private string _path;
        public override string Path => _path;
        
        public GameSaveDataJsonStorage(string path)
        {
            _path = path;
        }
    }
}