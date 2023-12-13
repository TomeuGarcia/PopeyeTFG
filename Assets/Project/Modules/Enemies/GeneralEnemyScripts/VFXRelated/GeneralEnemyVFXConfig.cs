using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.Enemies.VFX
{
    [CreateAssetMenu(fileName = "GeneralEnemyVFXConfig",
        menuName = "Popeye/Enemies/VFX/GeneralEnemyVFXConfig")]
    public class GeneralEnemyVFXConfig : ScriptableObject
    {
        [System.Serializable]
        public class MaterialFlash
        {
            public Material _flashMaterial;
            public float _waitTime;
        }

        public List<MaterialFlash> _flashSequence = new();
    }
}