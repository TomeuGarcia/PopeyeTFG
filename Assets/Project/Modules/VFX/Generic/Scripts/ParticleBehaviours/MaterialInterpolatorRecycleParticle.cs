using UnityEngine;

namespace Popeye.Modules.VFX.Generic.ParticleBehaviours
{
    public class MaterialInterpolatorRecycleParticle : InterpolatorRecycleParticle
    {
        [SerializeField] private MeshRenderer[] _meshRenderers;

        private void Awake()
        {
            foreach (var mesh in _meshRenderers)
            {
                _materials.Add(mesh.material);
            }
        }

        internal override void Release() { }
    }
}