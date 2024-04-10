using Popeye.Core.Services.EventSystem;
using Popeye.Modules.GameState;

namespace Popeye.Scripts.Core.Scenes.PlayedScene
{
    public class LastLoadedSceneProvider : ICurrentlyPlayedSceneProvider
    {
        private readonly IEventSystemService _eventSystemService;
        public ISceneReference CurrentlyPlayedScene { get; private set; }


        public LastLoadedSceneProvider(IEventSystemService eventSystemService)
        {
            _eventSystemService = eventSystemService;
        }
        
        
        public void StartListeningToSceneUpdates()
        {
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnStartLoadingAdditiveScene>(OnStartLoadingScene);
        }
        public void StopListeningToSceneUpdates()
        {
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnStartLoadingAdditiveScene>(OnStartLoadingScene);
        }

        private void OnStartLoadingScene(IGameStateEventsDispatcher.OnStartLoadingAdditiveScene eventData)
        {
            CurrentlyPlayedScene = eventData.SceneReference;
        }
        
    }
}