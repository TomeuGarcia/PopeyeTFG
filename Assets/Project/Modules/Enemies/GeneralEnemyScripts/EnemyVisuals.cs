using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisuals : MonoBehaviour
{
    //This is the base script, any unique additions to these
    //effects must be done by creating a new unique script
    //that inherits from this one.
    
    private struct MaterialFlash
    {
        [SerializeField] private Material _flashMaterial;
        [SerializeField] private float _waitTime;
    }

    [SerializeField] private List<Material> _originalMaterials;
    [SerializeField] private List<MaterialFlash> _flashSequence;
    
    public virtual void OnHitEffects()
    {
        
    }
    
    public virtual void OnDeathEffects()
    {
        
    }
}
