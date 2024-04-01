using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    public class GameObjectSceneDataEventsProvider : IActiveSceneDataEventsProvider
    {
        private readonly GameObject _gameObject;

        public GameObjectSceneDataEventsProvider(GameObject gameObject)
        {
            _gameObject = gameObject;
        }
        
        public string GetActiveSceneName()
        {
            return _gameObject.scene.name;
        }
    }
}