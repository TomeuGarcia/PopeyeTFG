using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.ProjectHelpers;
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
            public AnimationCurve _scaleFunction;
            public float _fadeOutDelay;
            public float _fadeOutTime;
            public AnimationCurve _fadeOutFunction;
            public float _totalTime => _fadeOutDelay + _fadeOutTime;
        }

        [Header("CIRCLE")]
        public VFXInterpolateData _circleInterpolateData;

        [Header("PARTICLES")]
        //saber a quina pool referirse??
        
        [Header("MATERIAL BLINK")]
        public List<MaterialFlash> _flashSequence = new();
    }
}