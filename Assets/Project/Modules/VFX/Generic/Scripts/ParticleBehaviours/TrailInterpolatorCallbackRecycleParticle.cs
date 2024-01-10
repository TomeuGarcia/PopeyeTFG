using System.Linq;
using Cysharp.Threading.Tasks;
using Popeye.Modules.VFX.Generic.ParticleBehaviours;
using UnityEngine;

public class TrailRendererInterpolatorRecycleParticle : InterpolatorRecycleParticle
{
    [SerializeField] private TrailRenderer[] _trailRenderers;

    private void Awake()
    {
        foreach (var trail in _trailRenderers)
        {
            _materials.Add(trail.material);
            trail.emitting = false;
        }
    }
    
    internal override void Init()
    {
        for (int i = 0; i < _trailRenderers.Length; i++)
        {
            _trailRenderers[i].emitting = true;
        }
        
        base.Init();
    }
    
    internal override void Reset()
    {
        foreach (var trail in _trailRenderers)
        {
            trail.emitting = false;
            trail.Clear();
        }
        
        base.Reset();
    }
    
    internal override void Release() { }
}
