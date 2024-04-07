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
        private readonly CreatedEnemiesRecycler _createdEnemiesRecycler;
        
        public MindCreatorsEnemyFactory(Dictionary<EnemyID, IEnemyMindFactoryCreator> enemyTypeToFactoryCreator,
            CreatedEnemiesRecycler createdEnemiesRecycler)
        {
            _enemyTypeToFactoryCreator = enemyTypeToFactoryCreator;
            _createdEnemiesRecycler = createdEnemiesRecycler;
        }
        
        

        public AEnemy Create(EnemyID enemyID, Vector3 position, Quaternion rotation)
        {
            AEnemy enemy = _enemyTypeToFactoryCreator[enemyID].Create(enemyID,position,rotation);
            
            _createdEnemiesRecycler.StartTrackingEnemy(enemy);
            
            return enemy;
        }



        
        
    }
}