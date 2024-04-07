using Popeye.Core.Services.EventSystem;
using Popeye.Scripts.Core.Scenes;

namespace Popeye.Modules.GameDataEvents
{
    public class LastLoadedSceneDataEventsProvider : IActiveSceneDataEventsProvider
    {
        private readonly IEventSystemService _eventSystemService;
        private SceneReferenceAsset _currentActiveScene;

        
        public LastLoadedSceneDataEventsProvider(IEventSystemService eventSystemService)
        {
            _eventSystemService = eventSystemService;
        }
        
        public string GetActiveSceneName()
        {
            return _currentActiveScene.SceneName;
        }

        public void StartListening()
        {
            _eventSystemService.Subscribe<ISceneLoadManager.OnStartLoadingAdditiveSceneEvent>(UpdateCurrentActiveScene);
        }
        public void StopListening()
        {
            _eventSystemService.Subscribe<ISceneLoadManager.OnStartLoadingAdditiveSceneEvent>(UpdateCurrentActiveScene);
        }


        private void UpdateCurrentActiveScene(ISceneLoadManager.OnStartLoadingAdditiveSceneEvent eventData)
        {
            _currentActiveScene = eventData.SceneReference;
        }
    }
}