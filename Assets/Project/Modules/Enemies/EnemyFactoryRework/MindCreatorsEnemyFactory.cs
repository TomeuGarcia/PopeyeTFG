using System.Collections.Generic;
using Popeye.Modules.Enemies.General;
using Popeye.Scripts.Core.Scenes.ObjectTracking;
using UnityEngine;

namespace Popeye.Modules.Enemies.EnemyFactories
{
    public class MindCreatorsEnemyFactory: IEnemyFactory
    {
        private readonly Dictionary<EnemyID,IEnemyMindFactoryCreator> _enemyTypeToFactoryCreator;
        private readonly ISceneObjectsTracker _createdEnemiesRecycler;
        
        public MindCreatorsEnemyFactory(Dictionary<EnemyID, IEnemyMindFactoryCreator> enemyTypeToFactoryCreator,
            ISceneObjectsTracker createdEnemiesRecycler)
        {
            _enemyTypeToFactoryCreator = enemyTypeToFactoryCreator;
            _createdEnemiesRecycler = createdEnemiesRecycler;
        }
        

        public AEnemy Create(EnemyID enemyID, Vector3 position, Quaternion rotation)
        {
            AEnemy enemy = _enemyTypeToFactoryCreator[enemyID].Create(enemyID,position,rotation);
            
            _createdEnemiesRecycler.StartTrackingObject(enemy);
            
            return enemy;
        }

    }
}