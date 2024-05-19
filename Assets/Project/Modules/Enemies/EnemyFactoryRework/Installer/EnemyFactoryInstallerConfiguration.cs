using System.Collections.Generic;
using NaughtyAttributes;
using Popeye.Modules.Enemies.General;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.Enemies.EnemyFactories
{
    [CreateAssetMenu(fileName = "EnemyFactoryInstallerConfig", 
        menuName = ScriptableObjectsHelper.ENEMIES_ASSET_PATH + "EnemyFactoryInstallerConfig")]
    public class EnemyFactoryInstallerConfiguration: ScriptableObject
    {
        [System.Serializable]
        private struct IdToPrefab
        {
            [SerializeField] private EnemyID _enemyID;
            [SerializeField] private EnemyMindPrefabSpawnData _prefabSpawnData;

            public EnemyID EnemyID => _enemyID;
            public EnemyMindPrefabSpawnData PrefabSpawnData => _prefabSpawnData;
        }

        [System.Serializable]
        public struct EnemyMindPrefabSpawnData
        {
            [SerializeField] private AEnemy _prefab;
            [SerializeField] private int _numberOfInitialObjects;

            public AEnemy Prefab => _prefab;
            public int NumberOfInitialObjects => _numberOfInitialObjects;
        }

        [Header("GENERIC ENEMIES")]
        [SerializeField] private IdToPrefab[] _idToPrefabs;

        [Header("SLIMES")] 
        [Expandable] [SerializeField] private SlimeFactoryConfiguration _slimeFactoryConfiguration;
        [Expandable] [SerializeField] private SlimeFactoryConfiguration _explosiveSlimeFactoryConfiguration;
        public SlimeFactoryConfiguration SlimeFactoryConfiguration => _slimeFactoryConfiguration;
        public SlimeFactoryConfiguration ExplosiveSlimeFactoryConfiguration => _explosiveSlimeFactoryConfiguration;

        public Dictionary<EnemyID, EnemyMindPrefabSpawnData> GetEnemyToPrefabDictionary()
        {
            Dictionary<EnemyID, EnemyMindPrefabSpawnData> enemyToPrefab = new Dictionary<EnemyID, EnemyMindPrefabSpawnData>(_idToPrefabs.Length);
            foreach (var idToPrefab in _idToPrefabs)
            {
                enemyToPrefab[idToPrefab.EnemyID] = idToPrefab.PrefabSpawnData;
            }

            return enemyToPrefab;
        }

        public EnemyID[] GetAllIds()
        {
            EnemyID[] slimeIds = SlimeFactoryConfiguration.GetSlimeEnemyIDs();

            List<EnemyID> enemyIds = new List<EnemyID>((slimeIds.Length * 2) + _idToPrefabs.Length);
            
            
            for (int i = 0; i < slimeIds.Length; ++i)
            {
                enemyIds.Add(slimeIds[i]);
            }
            
            slimeIds = ExplosiveSlimeFactoryConfiguration.GetSlimeEnemyIDs();
            for (int i = 0; i < slimeIds.Length; ++i)
            {
                enemyIds.Add(slimeIds[i]);
            }

            for (int i = 0; i < _idToPrefabs.Length; ++i)
            {
                enemyIds.Add(_idToPrefabs[i].EnemyID);
            }


            return enemyIds.ToArray();
        }
    }
}