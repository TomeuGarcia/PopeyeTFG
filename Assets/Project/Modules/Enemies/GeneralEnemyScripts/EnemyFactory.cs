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

        private readonly EnemyConfiguration _enemyConfiguration;

        public EnemyFactory(EnemyConfiguration enemyConfiguration)
        {
            _enemyConfiguration = enemyConfiguration;
            _enemyConfiguration.Init();
        }
        public AEnemy Create(Guid id, Vector3 position, Quaternion initialRotation)
        {
            return Object.Instantiate(_enemyConfiguration.GetEnemyPrefabById(id),position,initialRotation);
        }
    }
}
