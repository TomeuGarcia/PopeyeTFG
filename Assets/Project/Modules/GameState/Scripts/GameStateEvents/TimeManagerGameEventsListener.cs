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
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnGamePaused>(PauseTime);
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnGameResumed>(ResumeTime);
        }
        
        public void StopListening()
        {
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnGamePaused>(PauseTime);
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnGameResumed>(ResumeTime);
        }


        private void PauseTime(IGameStateEventsDispatcher.OnGamePaused eventData)
        {
            _timeScaleManager.SetPersistingTimeScale(0);
        }
        private void ResumeTime(IGameStateEventsDispatcher.OnGameResumed eventData)
        {
            _timeScaleManager.SetPersistingTimeScale(1);
        }
        
        
    }
}