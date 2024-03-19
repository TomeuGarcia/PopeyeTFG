namespace Popeye.Modules.GameState
{
    public interface IGameStateEventsDispatcher
    {
        public struct OnGamePausedEvent{}
        public struct OnGameResumedEvent{}

        void InvokeOnGamePaused();
        void InvokeOnGameResumed();
    }
}