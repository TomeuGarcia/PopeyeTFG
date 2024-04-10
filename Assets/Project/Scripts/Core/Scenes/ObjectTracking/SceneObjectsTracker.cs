using System.Collections.Generic;
using Popeye.Core.Services.EventSystem;
using Popeye.Modules.GameState;
using Popeye.Scripts.Core.Scenes.PlayedScene;

namespace Popeye.Scripts.Core.Scenes.ObjectTracking
{
    public class SceneObjectsTracker : ISceneObjectsTracker, ISceneObjectTrackerListener
    {
        private readonly IEventSystemService _eventSystemService;
        private readonly ICurrentlyPlayedSceneProvider _currentlyPlayedSceneProvider;

        private readonly List<ISceneObject> _currentlyActiveObjects;
        
        
        public SceneObjectsTracker(
            IEventSystemService eventSystemService, 
            ICurrentlyPlayedSceneProvider currentlyPlayedSceneProvider)
        {
            _eventSystemService = eventSystemService;
            _currentlyPlayedSceneProvider = currentlyPlayedSceneProvider;
            _currentlyActiveObjects = new List<ISceneObject>(30);
        }

        
        
        public void StartListeningToSceneUpdates()
        {
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnStartUnloadingScene>(OnStartUnloadingScene);
        }
        public void StopListeningToSceneUpdates()
        {
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnStartUnloadingScene>(OnStartUnloadingScene);
        }

        private void OnStartUnloadingScene(IGameStateEventsDispatcher.OnStartUnloadingScene eventData)
        {
            NotifyCurrentlyActiveObjectsIfBelongToScene(eventData.SceneReference);
        }
        

        public void StartTrackingObject(ISceneObject sceneObject)
        {
            sceneObject.SetBelongingScene(_currentlyPlayedSceneProvider.CurrentlyPlayedScene);
            AddCurrentlyActiveObject(sceneObject);
        }
        
        
        private void AddCurrentlyActiveObject(ISceneObject sceneObject)
        {
            _currentlyActiveObjects.Add(sceneObject);
            sceneObject.SetTrackerListener(this);
        }
        
        private void RemoveCurrentlyActiveObject(ISceneObject sceneObject)
        {
            _currentlyActiveObjects.Remove(sceneObject);
            sceneObject.SetTrackerListener(null);
        }

        
        private void NotifyCurrentlyActiveObjectsIfBelongToScene(ISceneReference sceneReference)
        {
            for (int i = _currentlyActiveObjects.Count - 1; i >= 0; --i)
            {
                ISceneObject enemy = _currentlyActiveObjects[i];
                if (enemy.BelongsToScene(sceneReference))
                {
                    enemy.OnBelongSceneWasUnloaded();
                }
            }            
        }

        public void OnSceneObjectDisabled(ISceneObject sceneObject)
        {
            RemoveCurrentlyActiveObject(sceneObject);
        }
        
    }
}