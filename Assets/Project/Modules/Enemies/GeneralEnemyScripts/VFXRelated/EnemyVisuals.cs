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
    public class OriginalMeshData
    {
        public MeshRenderer _mesh;
        [HideInInspector] public Material _originalMaterial;
    }

    [SerializeField] private GeneralEnemyVFXConfig _visualConfig;
    [SerializeField] [Tooltip("First mesh on the list will be the one that gets 'hurt'")]
    private List<OriginalMeshData> _originalMeshDatas = new();

    private void Awake()
    {
        foreach (var data in _originalMeshDatas)
        {
            data._originalMaterial = data._mesh.material;
        }
    }

    public virtual void OnHitEffects(float healthCoef)
    {
        _originalMeshDatas[0]._mesh.material.SetFloat("_Damage", healthCoef);
        
        FlashEffect();
    }

    private async UniTaskVoid FlashEffect() 
    {
        foreach (var flash in _visualConfig._flashSequence)
        {
            foreach (var data in _originalMeshDatas)
            {
                data._mesh.material = flash._flashMaterial;
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(flash._waitTime));
        }

        foreach (var data in _originalMeshDatas)
        {
            for (int i = 0; i < data._mesh.materials.Length; i++)
            {
                data._mesh.material = data._originalMaterial;
            }
        }
    }
    
    public virtual void OnDeathEffects()
    {
        
    }
}
