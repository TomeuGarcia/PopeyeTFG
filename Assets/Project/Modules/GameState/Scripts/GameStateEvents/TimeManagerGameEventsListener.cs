using Cysharp.Threading.Tasks;
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
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnGamePaused>(OnGamePausedEvent);
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnGameResumed>(OnGameResumedEvent);
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnExitToMainMenu>(OnExitToMainMenuEvent);
            
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnStartCameraAnimation>(OnCameraAnimationStartEvent);
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnFinishCameraAnimation>(OnCameraAnimationStartEvent);
        }
        
        public void StopListening()
        {
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnGamePaused>(OnGamePausedEvent);
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnGameResumed>(OnGameResumedEvent);
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnExitToMainMenu>(OnExitToMainMenuEvent);
            
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnStartCameraAnimation>(OnCameraAnimationStartEvent);
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnFinishCameraAnimation>(OnCameraAnimationStartEvent);
        }

        
        private void OnGamePausedEvent(IGameStateEventsDispatcher.OnGamePaused eventData)
        {
            PauseTime();
        }
        private void OnGameResumedEvent(IGameStateEventsDispatcher.OnGameResumed eventData)
        {
            ResumeTime();
        }
        private void OnExitToMainMenuEvent(IGameStateEventsDispatcher.OnExitToMainMenu eventData)
        {
            ResumeTimeAlways();
        }
        
        
        private void OnCameraAnimationStartEvent(IGameStateEventsDispatcher.OnStartCameraAnimation eventData)
        {
            PauseTime();
        }
        private void OnCameraAnimationStartEvent(IGameStateEventsDispatcher.OnFinishCameraAnimation eventData)
        {
            ResumeTime();
        }

        
        
        
        private void PauseTime()
        {
            _timeScaleManager.PauseTimeScalePersisting();
        }
        private void ResumeTime()
        {
            _timeScaleManager.ResumeTimeScalePersisting();
        }
        private void ResumeTimeAlways()
        {
            _timeScaleManager.ResumeTimeScalePersisting(true);            
        }
        
        
        
    }
}