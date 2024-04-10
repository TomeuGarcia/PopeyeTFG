using Popeye.Scripts.Core.Scenes.PlayedScene;


namespace Popeye.Modules.GameDataEvents
{
    public class LastLoadedSceneDataEventsProvider : IActiveSceneDataEventsProvider
    {
        private readonly ICurrentlyPlayedSceneProvider _currentlyPlayedSceneProvider;

        
        public LastLoadedSceneDataEventsProvider(ICurrentlyPlayedSceneProvider currentlyPlayedSceneProvider)
        {
            _currentlyPlayedSceneProvider = currentlyPlayedSceneProvider;
        }
        
        public string GetActiveSceneName()
        {
            return _currentlyPlayedSceneProvider.CurrentlyPlayedScene.SceneName;
        }
        
        
    }
}