using System.Collections.Generic;
using Popeye.Core.Pool;
using Popeye.Modules.Enemies.General;
using Popeye.Modules.Enemies.Slime;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.Enemies.EnemyFactories
{
    [CreateAssetMenu(fileName = "SlimeFactoryConfig", 
        menuName = ScriptableObjectsHelper.ENEMIES_ASSET_PATH + "SlimeFactoryConfig")]
    public class SlimeFactoryConfiguration : ScriptableObject
    {
        [System.Serializable]
        private struct SlimeMindAndSizeData
        {
            [SerializeField] private EnemyID _slimeMindId;
            [SerializeField] private AEnemyMediator _mediatorPrefab;
            [SerializeField] private int _numberOfInitialObjects;
            
            [SerializeField] private SlimeSizeID _slimeSizeId;
            [SerializeField] private SlimeFactory.SlimeChildSpawnData _nextChildrenSpawnData;
            
            public EnemyID SlimeMindId => _slimeMindId;
            public AEnemyMediator MediatorPrefab => _mediatorPrefab;
            public int NumberOfInitialObjects => _numberOfInitialObjects;
            
            public SlimeSizeID SlimeSizeId => _slimeSizeId;
            public SlimeFactory.SlimeChildSpawnData NextChildrenSpawnData => _nextChildrenSpawnData;
        }

        [SerializeField] private AEnemy _slimeMindPrefab;
        [SerializeField] private int _numberOfInitialSlimes = 30;
        
        [Space(20)]
        [SerializeField] private SlimeMindAndSizeData[] _slimeMindAndSizeDatas;
        
        public AEnemy SlimeMindPrefab => _slimeMindPrefab;
        public int NumberOfInitialSlimes => _numberOfInitialSlimes;
        
        

        public void SetupSlimeDictionaries(Transform parent, 
            out Dictionary<SlimeSizeID, ObjectPool> slimeSizeToPool,
            out Dictionary<SlimeSizeID, SlimeFactory.SlimeChildSpawnData> slimeSizeToNextSize,
            out Dictionary<EnemyID, SlimeSizeID> slimeTypeToSize)
        {
            slimeSizeToPool = new Dictionary<SlimeSizeID, ObjectPool>(_slimeMindAndSizeDatas.Length);
            
            slimeSizeToNextSize = new Dictionary<SlimeSizeID, SlimeFactory.SlimeChildSpawnData>(_slimeMindAndSizeDatas.Length);

            slimeTypeToSize = new Dictionary<EnemyID, SlimeSizeID>(_slimeMindAndSizeDatas.Length);
            
            
            foreach (var slimeMindAndSizeData in _slimeMindAndSizeDatas)
            {
                ObjectPool pool = new ObjectPool(slimeMindAndSizeData.MediatorPrefab, parent);
                pool.Init(slimeMindAndSizeData.NumberOfInitialObjects);
                
                slimeSizeToPool[slimeMindAndSizeData.SlimeSizeId] = pool;

                slimeSizeToNextSize[slimeMindAndSizeData.SlimeSizeId] = slimeMindAndSizeData.NextChildrenSpawnData;

                slimeTypeToSize[slimeMindAndSizeData.SlimeMindId] = slimeMindAndSizeData.SlimeSizeId;
            }
        }
        
        
        public EnemyID[] GetSlimeEnemyIDs()
        {
            EnemyID[] slimeEnemyIDs = new EnemyID[_slimeMindAndSizeDatas.Length];
            for (int i = 0; i < _slimeMindAndSizeDatas.Length; ++i)
            {
                slimeEnemyIDs[i] = _slimeMindAndSizeDatas[i].SlimeMindId;
            }

            return slimeEnemyIDs;
        }
        
    }
}