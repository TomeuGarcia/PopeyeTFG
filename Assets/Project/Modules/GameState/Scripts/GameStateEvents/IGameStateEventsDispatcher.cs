namespace Popeye.Modules.GameState
{
    public interface IGameStateEventsDispatcher
    {
        public struct OnGamePaused{}
        public struct OnGameResumed{}

        void InvokeOnGamePaused();
        void InvokeOnGameResumed();
    }
}