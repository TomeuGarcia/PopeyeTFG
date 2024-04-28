using Popeye.Core.Services.EventSystem;
using Popeye.Scripts.Core.Scenes;

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
            _eventSystemService.Dispatch(new IGameStateEventsDispatcher.OnGamePaused());
        }

        public void InvokeOnGameResumed()
        {
            _eventSystemService.Dispatch(new IGameStateEventsDispatcher.OnGameResumed());
        }

        public void InvokeOnExitToMainMenu()
        {
            _eventSystemService.Dispatch(new IGameStateEventsDispatcher.OnExitToMainMenu());
        }

        
        public void InvokeOnStartLoadingAdditiveScene(ISceneReference sceneReference)
        {
            _eventSystemService.Dispatch(new IGameStateEventsDispatcher.OnStartLoadingAdditiveScene(sceneReference));
        }

        public void InvokeOnStartLoadingAnyScene(ISceneReference sceneReference)
        {
            _eventSystemService.Dispatch(new IGameStateEventsDispatcher.OnStartLoadingAnyScene(sceneReference));
        }

        public void InvokeOnFinishLoadingScenes()
        {
            _eventSystemService.Dispatch(new IGameStateEventsDispatcher.OnFinishLoadingScenes());
        }

        public void InvokeOnStartUnloadingScene(ISceneReference sceneReference)
        {
            _eventSystemService.Dispatch(new IGameStateEventsDispatcher.OnStartUnloadingScene(sceneReference));
        }

        public void InvokeOnStartCameraAnimation()
        {
            _eventSystemService.Dispatch(new IGameStateEventsDispatcher.OnStartCameraAnimation());
        }

        public void InvokeOnFinishCameraAnimation()
        {
            _eventSystemService.Dispatch(new IGameStateEventsDispatcher.OnFinishCameraAnimation());
        }
    }
}