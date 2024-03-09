using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Popeye.Modules.VFX.Generic;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.Enemies.VFX
{
    [CreateAssetMenu(fileName = "GeneralEnemyVFXConfig", 
        menuName = ScriptableObjectsHelper.VFX_ASSETS_PATH + "GeneralEnemyVFXConfig")]
    
    public class GeneralEnemyVFXConfig : ScriptableObject
    {
        [Header("ONHIT")]
        [SerializeField] private ParticleTypes _waveParticles;
        [SerializeField] private ParticleTypes _splatterParticles;
        
        [Header("MATERIAL BLINK")]
        [SerializeField] private List<MaterialFlash> _flashSequence = new();
        
        public ParticleTypes WaveParticleType => _waveParticles;
        public ParticleTypes SplatterParticleType => _splatterParticles;
        public List<MaterialFlash> FlashSequence => _flashSequence;
    }
}