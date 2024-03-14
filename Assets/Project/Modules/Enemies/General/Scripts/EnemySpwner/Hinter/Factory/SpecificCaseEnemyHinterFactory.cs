using System.Collections.Generic;
using Popeye.Core.Pool;
using Unity.VisualScripting;
using UnityEngine;

namespace Popeye.Modules.Enemies.General
{
    public class SpecificCaseEnemyHinterFactory : IEnemyHinterFactory
    {
        private readonly EnemySpawnHinterConfig _defaultSpawnHinterConfig;
        private readonly Dictionary<EnemyID, EnemySpawnHinterConfig> _idsToConfigsDictionary;

        private readonly ObjectPool _enemySpawnHintersPool;


        public SpecificCaseEnemyHinterFactory(SpecificCaseEnemyHinterFactoryConfig config, Transform parent)
        {
            config.Init();
            _defaultSpawnHinterConfig = config.DefaultSpawnHinterConfig;
            _idsToConfigsDictionary = config.GetIdsToConfigsDictionary();

            _enemySpawnHintersPool = new ObjectPool(config.EnemySpawnHinterPrefab, parent);
            _enemySpawnHintersPool.Init(config.NumberOfInitialInstances);
        }

        
        
        public EnemySpawnHinter Create(Vector3 position, Quaternion rotation, EnemyID enemyID, out float duration)
        {
            EnemySpawnHinter enemySpawnHinter = _enemySpawnHintersPool.Spawn<EnemySpawnHinter>(position, rotation);

            EnemySpawnHinterConfig enemySpawnHinterConfig = GetEnemySpawnHinterConfig(enemyID);
            enemySpawnHinter.SetConfiguration(enemySpawnHinterConfig);
            enemySpawnHinter.PlayAnimation().Forget();
            
            duration = enemySpawnHinterConfig.UserWaitDuration;

            return enemySpawnHinter;
        }

        private EnemySpawnHinterConfig GetEnemySpawnHinterConfig(EnemyID enemyID)
        {
            if (!_idsToConfigsDictionary.TryGetValue(enemyID, out EnemySpawnHinterConfig spawnHinterConfig))
            {
                spawnHinterConfig = _defaultSpawnHinterConfig;
            }

            return spawnHinterConfig;
        }
        
    }
}