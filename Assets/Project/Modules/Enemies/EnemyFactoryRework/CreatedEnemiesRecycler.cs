using System.Collections.Generic;
using Popeye.Core.Services.EventSystem;
using Popeye.Scripts.Core.Scenes;
using Popeye.Scripts.Core.Scenes.PlayedScene;

namespace Popeye.Modules.Enemies.EnemyFactories
{
    public class CreatedEnemiesRecycler
    {
        private readonly IEventSystemService _eventSystemService;
        private readonly ICurrentlyPlayedSceneProvider _currentlyPlayedSceneProvider;

        private readonly List<AEnemy> _currentlyActiveEnemies;
        
        
        public CreatedEnemiesRecycler(
            IEventSystemService eventSystemService, 
            ICurrentlyPlayedSceneProvider currentlyPlayedSceneProvider)
        {
            _eventSystemService = eventSystemService;
            _currentlyPlayedSceneProvider = currentlyPlayedSceneProvider;
            _currentlyActiveEnemies = new List<AEnemy>(30);
        }


        public void StartTrackingEnemy(AEnemy enemy)
        {
            enemy.SetBelongingScene(_currentlyPlayedSceneProvider.CurrentlyPlayedScene);
            AddCurrentlyActiveEnemy(enemy);
        }
        
        private void AddCurrentlyActiveEnemy(AEnemy enemy)
        {
            _currentlyActiveEnemies.Add(enemy);
            enemy.OnRecycleComplete += RemoveCurrentlyActiveEnemy;
        }
        
        private void RemoveCurrentlyActiveEnemy(AEnemy enemy)
        {
            _currentlyActiveEnemies.Remove(enemy);
            enemy.OnRecycleComplete -= RemoveCurrentlyActiveEnemy;
        }


        
        public void StartListeningToSceneUpdates()
        {
            _eventSystemService.Subscribe<ISceneLoadManager.OnStartUnloadingSceneEvent>(OnStartUnloadingScene);
        }
        public void StopListeningToSceneUpdates()
        {
            _eventSystemService.Unsubscribe<ISceneLoadManager.OnStartUnloadingSceneEvent>(OnStartUnloadingScene);
        }
        
        private void OnStartUnloadingScene(ISceneLoadManager.OnStartUnloadingSceneEvent eventData)
        {
            KillCurrentlyActiveEnemiesIfBelongToScene(eventData.SceneReference);
        }
        private void KillCurrentlyActiveEnemiesIfBelongToScene(ISceneReference sceneReference)
        {
            for (int i = _currentlyActiveEnemies.Count - 1; i >= 0; --i)
            {
                AEnemy enemy = _currentlyActiveEnemies[i];
                if (enemy.BelongsToScene(sceneReference))
                {
                    enemy.DieFromOrder();
                }
            }            
        }
        
    }
}