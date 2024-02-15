using System.Collections.Generic;
using Popeye.Modules.Enemies.General;
using UnityEngine;

namespace Popeye.Modules.Enemies.EnemyFactories
{
    public class MindCreatorsEnemyFactory: IEnemyFactory
    {
        private readonly Dictionary<EnemyID,IEnemyMindFactoryCreator> _enemyTypeToFactoryCreator;

        public MindCreatorsEnemyFactory(Dictionary<EnemyID, IEnemyMindFactoryCreator> enemyTypeToFactoryCreator)
        {
            _enemyTypeToFactoryCreator = enemyTypeToFactoryCreator;
        }

        public AEnemy Create(EnemyID enemyID, Vector3 position, Quaternion rotation)
        {
            return _enemyTypeToFactoryCreator[enemyID].Create(enemyID,position,rotation);
        }
    }
}