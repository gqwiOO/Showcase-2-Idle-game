namespace Mechanics.GameSession.Service
{
    public interface IGameSessionService
    {
        GameSession LoadSession();
        void SaveSession();
    }

    public class GameSessionService : IGameSessionService
    {
        private readonly GameSessionJsonStorage _gameSessionJsonStorage = new ("Data/Sessions/Session_1");
        public GameSession LoadSession()
        {
            return new GameSession();
        }

        public void SaveSession()
        {
            
        }
    }
}