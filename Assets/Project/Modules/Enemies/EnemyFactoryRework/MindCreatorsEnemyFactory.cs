using System.Collections.Generic;
using Popeye.Core.Services.EventSystem;
using Popeye.Modules.Enemies.Components;
using Popeye.Modules.Enemies.General;
using Popeye.Scripts.Core.Scenes;
using Popeye.Scripts.Core.Scenes.PlayedScene;
using UnityEngine;

namespace Popeye.Modules.Enemies.EnemyFactories
{
    public class MindCreatorsEnemyFactory: IEnemyFactory
    {
        private readonly Dictionary<EnemyID,IEnemyMindFactoryCreator> _enemyTypeToFactoryCreator;
        private readonly IEventSystemService _eventSystemService;
        private readonly ICurrentlyPlayedSceneProvider _currentlyPlayedSceneProvider;

        private readonly List<AEnemy> _currentlyActiveEnemies;
        
        public MindCreatorsEnemyFactory(Dictionary<EnemyID, IEnemyMindFactoryCreator> enemyTypeToFactoryCreator,
            IEventSystemService eventSystemService, ICurrentlyPlayedSceneProvider currentlyPlayedSceneProvider)
        {
            _enemyTypeToFactoryCreator = enemyTypeToFactoryCreator;
            _eventSystemService = eventSystemService;
            _currentlyPlayedSceneProvider = currentlyPlayedSceneProvider;
            _currentlyActiveEnemies = new List<AEnemy>(30);
        }
        
        

        public AEnemy Create(EnemyID enemyID, Vector3 position, Quaternion rotation)
        {
            AEnemy enemy = _enemyTypeToFactoryCreator[enemyID].Create(enemyID,position,rotation);
            enemy.SetBelongingScene(_currentlyPlayedSceneProvider.CurrentlyPlayedScene);
            AddCurrentlyActiveEnemy(enemy);
            return enemy;
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