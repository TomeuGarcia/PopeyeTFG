using Popeye.Modules.VFX.Generic;
using Popeye.Modules.VFX.Generic.MaterialInterpolationConfiguration;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    [System.Serializable]
    public class HazardDispenserViewConfig
    {
        [Header("CHARGE")]
        [SerializeField] private ParticleTypes _chargeParticleType;
        
        [Header("READY")]
        [SerializeField] private MaterialFloatInterpolationConfig[] _readyInterpolations;
        
        public ParticleTypes ChargeParticleType => _chargeParticleType;
        public MaterialFloatInterpolationConfig[] ReadyActivate => _readyInterpolations;
    }
}