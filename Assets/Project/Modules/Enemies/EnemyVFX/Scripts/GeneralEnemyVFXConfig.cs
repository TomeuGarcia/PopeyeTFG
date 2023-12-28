using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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

        [System.Serializable]
        public class VFXInterpolateData
        {
            public float _startScale;
            public float _endScale;
            public float _fadeOutDelay;
            public float _fadeOutTime;
            public float _totalTime => _fadeOutDelay + _fadeOutTime;
        }

        [Header("CIRCLE")]
        public GameObject _circlePrefab; //TODO fix later
        public VFXInterpolateData _circleInterpolateData;

        [Header("PARTICLES")]
        public GameObject _particlesPrefab; //TODO fix later
        //saber a quina pool referirse??
        
        
        [Header("MATERIAL BLINK")]
        public List<MaterialFlash> _flashSequence = new();
    }
}