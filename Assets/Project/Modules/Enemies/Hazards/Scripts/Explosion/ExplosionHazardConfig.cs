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
        [Header("SIZE")]
        [SerializeField] private ExplosionBySizeConfig[] _sizeConfigs;
        
        [Header("LOGIC")]
        [SerializeField, Range(0f, 5.0f)] private float _lifeTime = 1f;

        [Header("AUDIO")]
        [SerializeField] private FMODExplosionAudio _explosionAudio;
        public IExplosionAudio ExplosionAudio => _explosionAudio;
        
        
        public float LifeTime => _lifeTime;
        
        
        public DamageHit GetPlayerDamageHitBySize(ExplosionSize size)
        {
            return new DamageHit(GetSizeConfig(size).playerDamageHitConfig);
        }
        public DamageHit GetOtherDamageHitBySize(ExplosionSize size)
        {
            return new DamageHit(GetSizeConfig(size).otherDamageHitConfig);
        }

        public float GetScaleBySize(ExplosionSize size)
        {
            return GetSizeConfig(size).scale;
        }

        private ExplosionBySizeConfig GetSizeConfig(ExplosionSize size)
        {
            foreach(ExplosionBySizeConfig config in _sizeConfigs)
            {
                if (config.size == size)
                {
                    return config;
                }
            }

            return default;
        }
        
    }
}

