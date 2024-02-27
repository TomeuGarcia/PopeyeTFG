using Popeye.Core.Pool;
using Popeye.Modules.Enemies.Components;
using Popeye.Modules.Enemies.General;
using Popeye.Modules.Enemies.Hazards;
using UnityEngine;

namespace Popeye.Modules.Enemies.EnemyFactories
{
    public class GenericEnemyMindFactoryCreator:IEnemyMindFactoryCreator
    {
        private readonly IHazardFactory _hazardFactory;
        private readonly ObjectPool _enemyPool;

       public GenericEnemyMindFactoryCreator(AEnemy enemyMindPrefab,Transform parent,int numberOfInitialObjects,
           IHazardFactory hazardFactory)
       {
           _hazardFactory = hazardFactory;
           _enemyPool = new ObjectPool(enemyMindPrefab, parent);
           _enemyPool.Init(numberOfInitialObjects);
       }

       public AEnemy Create(EnemyID enemyID, Vector3 position, Quaternion rotation)
        {
            var enemy = _enemyPool.Spawn<AEnemy>(position, rotation);
            enemy.SetHazardFactory(_hazardFactory);
            return enemy;
        }
    }
}