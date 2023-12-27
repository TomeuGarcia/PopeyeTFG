using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.Enemies;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Popeye.Modules.Enemies
{
    public class EnemyFactory
    {

        private readonly EnemyFactoryConfiguration _enemyFactoryConfiguration;

        public EnemyFactory(EnemyFactoryConfiguration enemyFactoryConfiguration)
        {
            _enemyFactoryConfiguration = enemyFactoryConfiguration;
            _enemyFactoryConfiguration.Init();
        }
        public AEnemy Create(Guid id, Vector3 position, Quaternion initialRotation)
        {
            return Object.Instantiate(_enemyFactoryConfiguration.GetEnemyPrefabById(id),position,initialRotation);
        }
    }
}
