using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.VFX.Generic;
using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.Enemies.VFX
{
    [CreateAssetMenu(fileName = "GeneralEnemyVFXConfig", 
        menuName = ScriptableObjectsHelper.VFX_ASSETS_PATH + "GeneralEnemyVFXConfig")]
    
    public class GeneralEnemyVFXConfig : ScriptableObject
    {
        [Header("ONHIT")]
        [SerializeField] private ParticleTypes _waveParticles;
        [SerializeField] private ParticleTypes _splatterParticles;
        [SerializeField] private ParticleTypes _bloodHitDirectionalParticles;
        [SerializeField] private ParticleTypes _bloodHitSplashParticles;
        [SerializeField] private ParticleTypes _bloodDripParticles;
        [SerializeField] private List<MaterialFlash> _flashSequence = new();
        [SerializeField] private CameraShakeConfig _onHitShakeConfig;
        
        [Header("DEATH")]
        [SerializeField] private ParticleTypes _deathParticles;
        [SerializeField] private CameraShakeConfig _deathShakeConfig;
        
        public ParticleTypes WaveParticleType => _waveParticles;
        public ParticleTypes SplatterParticleType => _splatterParticles;
        public ParticleTypes BloodHitDirectionalParticles => _bloodHitDirectionalParticles;
        public ParticleTypes BloodHitSplashParticles => _bloodHitSplashParticles;
        public ParticleTypes BloodDripParticles => _bloodDripParticles;
        public List<MaterialFlash> FlashSequence => _flashSequence;
        public CameraShakeConfig OnHitShakeConfig => _onHitShakeConfig;
        public ParticleTypes DeathParticles => _deathParticles;
        public CameraShakeConfig DeathShakeConfig => _deathShakeConfig;
    }
}