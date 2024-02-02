using System.Collections.Generic;
using Popeye.Modules.VFX.Generic.MaterialInterpolationConfiguration;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Popeye.Modules.VFX.Generic.ParticleBehaviours
{
    [System.Serializable]
    public class InterpolatorRecycleParticleData
    {
        [SerializeField] private MaterialFloatSetupConfig[] _floatSetupDatas;
        [SerializeField] private MaterialFloatInterpolationConfig[] _floatInterpolationDatas;
        [SerializeField] private GameObject[] _meshSources;
        
        private List<Material> _materials = new();
        private List<TrailRenderer> _trailRenderers = new();

        public MaterialFloatSetupConfig[] FloatSetupDatas => _floatSetupDatas;
        public MaterialFloatInterpolationConfig[] FloatInterpolationDatas => _floatInterpolationDatas;
        public List<Material> Materials => _materials;
        public List<TrailRenderer> TrailRenderers => _trailRenderers;
        
        public void Awake()
        {
            foreach (var source in _meshSources)
            {
                if (source.GetComponent<MeshRenderer>() != null)
                {
                    _materials.Add(source.GetComponent<MeshRenderer>().material);
                }
                else if (source.GetComponent<DecalProjector>() != null)
                {
                    Material mat = new Material(source.GetComponent<DecalProjector>().material);
                    source.GetComponent<DecalProjector>().material = mat;
                    _materials.Add(source.GetComponent<DecalProjector>().material);
                }
                else if (source.GetComponent<TrailRenderer>() != null)
                {
                    _materials.Add(source.GetComponent<TrailRenderer>().material);
                    _trailRenderers.Add(source.GetComponent<TrailRenderer>());
                }
                else
                {
                    Debug.LogError("Material source is invalid or unsupported. Given source: " + source.name);
                }
            }
        }
    }
}