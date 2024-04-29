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
        
        //TODO - Delete old
        [Header("PREPARE")]
        [SerializeField] private TweenConfig _prepareRotation;
        [SerializeField] private TweenConfig _prepareScale;
        
        [Header("DISPENSE")]
        [SerializeField] private TweenPunchConfig _dispenseScalePunch;
        
        [Header("READY")]
        [SerializeField] private TweenPunchConfig _readyRotationPunch;
        
        
        public TweenConfig PrepareRotation => _prepareRotation;
        public TweenConfig PrepareScale => _prepareScale;
        public TweenPunchConfig DispenseScalePunch => _dispenseScalePunch;
        public TweenPunchConfig ReadyRotationPunch => _readyRotationPunch;
    }
}