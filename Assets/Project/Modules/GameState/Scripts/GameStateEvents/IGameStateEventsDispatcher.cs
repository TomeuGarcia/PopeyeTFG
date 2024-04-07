namespace Popeye.Modules.GameState
{
    public interface IGameStateEventsDispatcher
    {
        public struct OnGamePaused{}
        public struct OnGameResumed{}
        public struct OnExitToMainMenu{}

        void InvokeOnGamePaused();
        void InvokeOnGameResumed();
        void InvokeOnExitToMainMenu();
    }
}