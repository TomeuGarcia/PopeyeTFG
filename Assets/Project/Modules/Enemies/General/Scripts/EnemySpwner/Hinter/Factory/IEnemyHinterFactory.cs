using UnityEngine;

namespace Popeye.Modules.Enemies.General
{
    public interface IEnemyHinterFactory
    {
        EnemySpawnHinter Create(Vector3 position, Quaternion rotation, EnemyID enemyID, out float duration);
    }
}