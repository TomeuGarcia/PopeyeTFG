using Popeye.Core.Services.EventSystem;
using Project.Scripts.Time.TimeScale;

namespace Popeye.Modules.GameState
{
    public class TimeManagerGameEventsListener
    {
        private readonly IEventSystemService _eventSystemService;
        private readonly ITimeScaleManager _timeScaleManager;

        public TimeManagerGameEventsListener(IEventSystemService eventSystemService, ITimeScaleManager timeScaleManager)
        {
            _eventSystemService = eventSystemService;
            _timeScaleManager = timeScaleManager;
        }
        
        public void StartListening()
        {
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnGamePausedEvent>(PauseTime);
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnGameResumedEvent>(ResumeTime);
        }
        
        public void StopListening()
        {
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnGamePausedEvent>(PauseTime);
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnGameResumedEvent>(ResumeTime);
        }


        private void PauseTime(IGameStateEventsDispatcher.OnGamePausedEvent eventData)
        {
            _timeScaleManager.SetPersistingTimeScale(0);
        }
        private void ResumeTime(IGameStateEventsDispatcher.OnGameResumedEvent eventData)
        {
            _timeScaleManager.SetPersistingTimeScale(1);
        }
        
        
    }
}