using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyVisuals : MonoBehaviour
{
    //This is the base script, any unique additions to these
    //effects must be done by creating a new unique script
    //that inherits from this one.
    
    [System.Serializable]
    public class MaterialFlash
    {
        public Material _flashMaterial;
        public float _waitTime;
    }
    
    [System.Serializable]
    public class OriginalMeshData
    {
        public MeshRenderer _mesh;
        [HideInInspector] public Material[] _originalMaterials;
    }

    [SerializeField] private List<OriginalMeshData> _originalMeshDatas = new();
    [SerializeField] private List<MaterialFlash> _flashSequence = new();

    private void OnValidate()
    {
        foreach (var data in _originalMeshDatas)
        {
            data._originalMaterials = data._mesh.sharedMaterials;
        }
    }

    public virtual void OnHitEffects()
    {
        
    }
    
    private async UniTaskVoid WaitExample() 
    {
        foreach (var flash in _flashSequence)
        {
            foreach (var data in _originalMeshDatas)
            {
                
            }
            
            //await UniTask.Delay(TimeSpan.FromSeconds(secondsToWait));
        }
    }
    
    public virtual void OnDeathEffects()
    {
        
    }
}
