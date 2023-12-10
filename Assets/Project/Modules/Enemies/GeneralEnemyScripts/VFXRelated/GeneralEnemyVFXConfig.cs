using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneralEnemyVFXConfig")]
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
