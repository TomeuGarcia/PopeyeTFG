using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.CombatSystem;
using Popeye.ProjectHelpers;
using UnityEngine;


namespace Popeye.Modules.Enemies.Hazards
{
    [CreateAssetMenu(fileName = "ExplosionHazardConfig",
        menuName = ScriptableObjectsHelper.HAZARDS_ASSET_PATH + "ExplosionHazardConfig")]
    public class ExplosionHazardConfig : ScriptableObject
    {
        [SerializeField] private ExplosionBySizeConfig[] _sizeConfigs;
        [SerializeField] private DamageHit _defaultDamageHitConfig;
        [SerializeField] private float _defaultScale = 3;

        public DamageHit  GetDamageHitBySize(ExplosionSize size)
        {
            foreach(ExplosionBySizeConfig config in _sizeConfigs)
            {
                if (config.size == size)
                {
                    return config.damageHitConfig;
                }
            }

            return _defaultDamageHitConfig;
        }

        public float GetScaleBySize(ExplosionSize size)
        {
            foreach(ExplosionBySizeConfig config in _sizeConfigs)
            {
                if (config.size == size)
                {
                    return config.scale;
                }
            }

            return _defaultScale;
        }
        
    }
}

