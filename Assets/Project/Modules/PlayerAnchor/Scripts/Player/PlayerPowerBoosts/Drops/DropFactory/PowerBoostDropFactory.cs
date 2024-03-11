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

        public PowerBoostDropFactory(PowerBoostDropFactoryConfig config, Transform parent, Transform autoCollectTransform)
        {
            _config = config;
            _autoCollectTransform = autoCollectTransform;
            _experienceAmountToPool = _config.GetExperienceAmountToPool(parent);
        }
        
        
        public void Create(Vector3 position, Quaternion rotation, PowerBoostDropConfig dropConfig)
        {
            int experienceAmount = dropConfig.ExperienceToDrop;

            AutoCollectPowerBoostDrop powerBoostDrop = 
                _experienceAmountToPool[experienceAmount].Spawn<AutoCollectPowerBoostDrop>(position, rotation);
            powerBoostDrop.Init(experienceAmount, _autoCollectTransform);
        }
    }
}