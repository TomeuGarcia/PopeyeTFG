using Popeye.Core.Pool;
using Popeye.Modules.Enemies.Components;
using Popeye.Modules.Enemies.General;
using UnityEngine;

namespace Popeye.Modules.Enemies.EnemyFactories
{
    public class GenericEnemyMindFactoryCreator:IEnemyMindFactoryCreator
    {
        private readonly ObjectPool _enemyPool;

       public GenericEnemyMindFactoryCreator(AEnemy enemyMindPrefab,Transform parent,int numberOfInitialObjects)
       {
           _enemyPool = new ObjectPool(enemyMindPrefab, parent);
           _enemyPool.Init(numberOfInitialObjects);
       }

       public AEnemy Create(EnemyID enemyID, Vector3 position, Quaternion rotation,EnemyPatrolling.PatrolType patrolType)
        {
            return _enemyPool.Spawn<AEnemy>(position, rotation);
        }
    }
}