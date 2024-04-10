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
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnGamePaused>(OnGamePaused);
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnGameResumed>(OnGameResumed);
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnExitToMainMenu>(OnExitToMainMenu);
        }
        
        public void StopListening()
        {
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnGamePaused>(OnGamePaused);
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnGameResumed>(OnGameResumed);
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnExitToMainMenu>(OnExitToMainMenu);
        }

        
        private void OnGamePaused(IGameStateEventsDispatcher.OnGamePaused eventData)
        {
            PauseTime();
        }
        private void OnGameResumed(IGameStateEventsDispatcher.OnGameResumed eventData)
        {
            ResumeTime();
        }
        private void OnExitToMainMenu(IGameStateEventsDispatcher.OnExitToMainMenu eventData)
        {
            ResumeTime();
        }

        private void PauseTime()
        {
            _timeScaleManager.SetPersistingTimeScale(0);
        }
        private void ResumeTime()
        {
            _timeScaleManager.SetPersistingTimeScale(1);
        }
        
        
        
    }
}