using Popeye.Core.Services.EventSystem;

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
            _eventSystemService.Subscribe<ISceneLoadManager.OnStartLoadingAdditiveSceneEvent>(OnStartLoadingScene);
        }
        public void StopListeningToSceneUpdates()
        {
            _eventSystemService.Unsubscribe<ISceneLoadManager.OnStartLoadingAdditiveSceneEvent>(OnStartLoadingScene);
        }

        private void OnStartLoadingScene(ISceneLoadManager.OnStartLoadingAdditiveSceneEvent eventData)
        {
            CurrentlyPlayedScene = eventData.SceneReference;
        }
        
    }
}