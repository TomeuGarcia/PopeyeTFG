using System.Collections.Generic;
using Popeye.Core.Pool;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops
{
    
    [CreateAssetMenu(fileName = "PowerBoostDropFactoryConfig", 
        menuName = ScriptableObjectsHelper.PLAYERPOWERBOOSTDROPS_ASSETS_PATH + "PowerBoostDropFactoryConfig")]
    public class PowerBoostDropFactoryConfig : ScriptableObject
    {
        [System.Serializable]
        private class DropConfigToPoolData
        {
            [SerializeField] private AutoCollectPowerBoostDrop _dropPrefab;
            [SerializeField, Range(1, 50)] private int _experienceAmount = 1;
            [SerializeField] private int _initialInstances = 20;
            
            public RecyclableObject DropPrefab => _dropPrefab;
            public int ExperienceAmount => _experienceAmount;
            public int InitialInstances => _initialInstances;
        }

        [Header("DEFAULT")]
        [SerializeField] private AutoCollectPowerBoostDrop _defaultDropPrefab;
        [SerializeField] private int _defaultInitialInstances = 10;

        
        [Header("DICTIONARY")]
        [SerializeField] private DropConfigToPoolData[] _dropConfigToPoolsData;
        
        
        
        public Dictionary<int, ObjectPool> GetExperienceAmountToPool(Transform parent, out HashSet<int> experiencePartitionSet)
        {
            Dictionary<int, ObjectPool> experienceAmountToPool =
                new Dictionary<int, ObjectPool>(_dropConfigToPoolsData.Length);

            experiencePartitionSet = new HashSet<int>(_dropConfigToPoolsData.Length);
            
            foreach (DropConfigToPoolData dropConfigToPoolData in _dropConfigToPoolsData)
            {
                ObjectPool pool = new ObjectPool(dropConfigToPoolData.DropPrefab, parent);
                pool.Init(dropConfigToPoolData.InitialInstances);
                
                experienceAmountToPool.Add(dropConfigToPoolData.ExperienceAmount, pool);

                experiencePartitionSet.Add(dropConfigToPoolData.ExperienceAmount);
            }

            return experienceAmountToPool;
        }

        public ObjectPool GetDefaultExperiencePool(Transform parent)
        {
            ObjectPool pool = new ObjectPool(_defaultDropPrefab, parent);
            pool.Init(_defaultInitialInstances);
                
            return pool;
        }
        

    }
    
}