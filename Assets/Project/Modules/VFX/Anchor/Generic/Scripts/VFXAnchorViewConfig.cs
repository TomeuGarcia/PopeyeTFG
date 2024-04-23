using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.VFX.Generic;
using Popeye.ProjectHelpers;
using Project.Scripts.Time.TimeHitStop;
using UnityEngine;

namespace Popeye.Modules.VFX.Anchor.Generic
{
    [CreateAssetMenu(fileName = "VFXAnchorViewConfig", 
        menuName = ScriptableObjectsHelper.VFX_ASSETS_PATH + "VFXAnchorViewConfig")]
    
    public class VFXAnchorViewConfig : ScriptableObject
    {
        [Header("PARTICLE TYPES")]
        [SerializeField] private ParticleTypes _retrieveTrailParticleType;
        [SerializeField] private ParticleTypes _throwTrailParticleType;
        [SerializeField] private ParticleTypes _throwTrailSoftParticleType;
        [SerializeField] private ParticleTypes _throwHeadParticleType;
        [SerializeField] private ParticleTypes _slamHeadParticleType;
        [SerializeField] private ParticleTypes _slamGroundHitParticleType;
        [SerializeField] private ParticleTypes _slamGroundDecalParticleType;
        [SerializeField] private ParticleTypes _throwGroundDecalParticleType;
        
        [Header("HITSTOP CONFIGS")]
        [SerializeField] private HitStopConfig _hitStopDamageDealt;
        
        [Header("CAMERA SHAKE CONFIGS")]
        [SerializeField] private CameraShakeConfig _shakeConfigDamageDealt;

        [Header("PARAMETERS")]
        [SerializeField] private Vector3 _slamTrailOffset = new Vector3(-0.8f, 0.0f, -0.1f);
        [SerializeField] private Vector3 _throwTrailFallnoffset = new Vector3(-0.8f, 0.0f, -0.1f);
        private Vector3 _slamTrailFlipOffset;
        [SerializeField] private float _throwTrailSpawnDelay = 0.2f;
        [SerializeField] private float _throwTrailFallnDelay = 0.05f;
        [SerializeField] private float _retrieveTrailSpawnDelay = 0.2f;
        [SerializeField] private float _fallImpactDelay = 0.075f;

        public ParticleTypes RetrieveTrailParticleType => _retrieveTrailParticleType;
        public ParticleTypes ThrowTrailParticleType => _throwTrailParticleType;
        public ParticleTypes ThrowTrailSoftParticleType => _throwTrailSoftParticleType;
        public ParticleTypes ThrowHeadParticleType => _throwHeadParticleType;
        public ParticleTypes SlamHeadParticleType => _slamHeadParticleType;
        public ParticleTypes SlamGroundHitParticleType => _slamGroundHitParticleType;
        public ParticleTypes SlamGroundDecalParticleType => _slamGroundDecalParticleType;
        public ParticleTypes ThrowGroundDecalParticleType => _throwGroundDecalParticleType;

        public HitStopConfig HitStopDamageDealt => _hitStopDamageDealt;
        
        public CameraShakeConfig ShakeConfigDamageDealt => _shakeConfigDamageDealt;

        public Vector3 SlamTrailOffset => _slamTrailOffset;
        public Vector3 ThrowTrailFallnoffset => _throwTrailFallnoffset;
        public Vector3 SlamTrailFlipOffset => new Vector3(-_slamTrailOffset.x, -_slamTrailOffset.y, _slamTrailOffset.z);
        public float ThrowTrailSpawnDelay => _throwTrailSpawnDelay;
        public float ThrowTrailFallnDelay => _throwTrailFallnDelay;
        public float RetrieveTrailSpawnDelay => _retrieveTrailSpawnDelay;
        public float FallImpactDelay => _fallImpactDelay;
    }
}