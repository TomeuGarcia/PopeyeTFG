using Popeye.Core.Services.EventSystem;

namespace Popeye.Modules.GameState
{
    public class GameStateEventsDispatcher : IGameStateEventsDispatcher
    {
        private readonly IEventSystemService _eventSystemService;

        public GameStateEventsDispatcher(IEventSystemService eventSystemService)
        {
            _eventSystemService = eventSystemService;
        }
        
        
        public void InvokeOnGamePaused()
        {
            _eventSystemService.Dispatch(new IGameStateEventsDispatcher.OnGamePausedEvent());
        }

        public void InvokeOnGameResumed()
        {
            _eventSystemService.Dispatch(new IGameStateEventsDispatcher.OnGameResumedEvent());
        }
    }
}