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
        [System.Serializable]
        public class MaterialFlash
        {
            public Material _flashMaterial;
            public float _waitTime;
        }

        [Header("ONHIT")]
        public ParticleTypes _waveParticles;
        public ParticleTypes _splatterParticles;
        
        
        [Header("MATERIAL BLINK")]
        public List<MaterialFlash> _flashSequence = new();
    }
}