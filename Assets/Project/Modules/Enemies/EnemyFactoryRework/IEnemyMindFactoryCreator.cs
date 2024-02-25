using Popeye.Modules.Enemies.General;
using UnityEngine;

namespace Popeye.Modules.Enemies.EnemyFactories
{
    public interface IEnemyMindFactoryCreator
    {
        AEnemy Create(EnemyID enemyID,Vector3 position, Quaternion rotation);
    }
}