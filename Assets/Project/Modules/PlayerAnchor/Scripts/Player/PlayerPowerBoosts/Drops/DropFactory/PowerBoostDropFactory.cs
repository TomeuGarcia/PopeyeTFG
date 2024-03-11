using System.Collections.Generic;
using Popeye.Core.Pool;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops
{
    public class PowerBoostDropFactory : IPowerBoostDropFactory
    {
        private readonly PowerBoostDropFactoryConfig _config;
        private readonly Transform _autoCollectTransform;
        private readonly Dictionary<int, ObjectPool> _experienceAmountToPool;
        private readonly ObjectPool _defaultExperiencePool;
        private readonly HashSet<int> _experiencePartitionSet;

        public PowerBoostDropFactory(PowerBoostDropFactoryConfig config, Transform parent, Transform autoCollectTransform)
        {
            _config = config;
            _autoCollectTransform = autoCollectTransform;
            _experienceAmountToPool = _config.GetExperienceAmountToPool(parent, out _experiencePartitionSet);
            _defaultExperiencePool = _config.GetDefaultExperiencePool(parent);
        }
        
        
        public void Create(Vector3 position, Quaternion rotation, PowerBoostDropConfig dropConfig)
        {
            int experienceToDrop = dropConfig.ExperienceToDrop;

            while (experienceToDrop > 0)
            {
                int experienceAmount = ExperienceToDrop(experienceToDrop);
                experienceToDrop -= experienceAmount;
                
                DoCreate(position, rotation, experienceAmount);
            }
        }
        
        private void DoCreate(Vector3 position, Quaternion rotation, int experienceAmount)
        {
            if (!_experienceAmountToPool.TryGetValue(experienceAmount, out ObjectPool powerBoostPool))
            {
                powerBoostPool = _defaultExperiencePool;
            }
            
            AutoCollectPowerBoostDrop powerBoostDrop =
                powerBoostPool.Spawn<AutoCollectPowerBoostDrop>(position, rotation);

            powerBoostDrop.Init(experienceAmount, _autoCollectTransform);
        }

        
        private int ExperienceToDrop(int experienceToDrop)
        {
            int highest = 1;

            foreach (int experience in _experiencePartitionSet)
            {
                if (experience > highest && experience <= experienceToDrop)
                {
                    highest = experience;
                }
            }

            return highest;
        }
        
    }
}