using System.Collections.Generic;
using Popeye.Core.Pool;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.Enemies.General
{
    [CreateAssetMenu(fileName = "SpecificCaseEnemyHinterFactoryConfig", 
        menuName = ScriptableObjectsHelper.ENEMYHINTS_ASSET_PATH + "SpecificCaseEnemyHinterFactoryConfig")]
    public class SpecificCaseEnemyHinterFactoryConfig : ScriptableObject
    {
        [System.Serializable]
        private struct EnemyIDToHinterConfig
        {
            [SerializeField] private EnemyID _enemyID;
            [SerializeField] private EnemySpawnHinterConfig _spawnHinterConfig;
            
            public EnemyID EnemyID => _enemyID;
            public EnemySpawnHinterConfig SpawnHinterConfig => _spawnHinterConfig;
        }

        [Header("POOL")]
        [SerializeField, Range(1, 15)] private int _numberOfInitialInstances = 5;
        [SerializeField] private RecyclableObject _enemySpawnHinterPrefab;
        
        [Header("CONFIGURATION")]
        [SerializeField] private EnemySpawnHinterConfig _defaultSpawnHinterConfig;
        [SerializeField] private EnemyIDToHinterConfig[] _idsToConfigs;


        public int NumberOfInitialInstances => _numberOfInitialInstances;
        public RecyclableObject EnemySpawnHinterPrefab => _enemySpawnHinterPrefab;
        public EnemySpawnHinterConfig DefaultSpawnHinterConfig => _defaultSpawnHinterConfig;


        public void Init()
        {
            _defaultSpawnHinterConfig.InitMaterials(NumberOfInitialInstances);

            foreach (var enemyIDToHinterConfig in _idsToConfigs)
            {
                enemyIDToHinterConfig.SpawnHinterConfig.InitMaterials(NumberOfInitialInstances);
            }
            
        }

        public Dictionary<EnemyID, EnemySpawnHinterConfig> GetIdsToConfigsDictionary()
        {
            Dictionary<EnemyID, EnemySpawnHinterConfig> idsToConfigsDictionary =
                new Dictionary<EnemyID, EnemySpawnHinterConfig>(_idsToConfigs.Length);

            foreach (EnemyIDToHinterConfig idToConfig in _idsToConfigs)
            {
                idsToConfigsDictionary.Add(idToConfig.EnemyID, idToConfig.SpawnHinterConfig);
            }
            
            return idsToConfigsDictionary;
        }


    }
}