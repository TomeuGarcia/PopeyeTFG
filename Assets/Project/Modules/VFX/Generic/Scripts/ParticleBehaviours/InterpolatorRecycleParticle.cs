using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Popeye.Core.Pool;
using Popeye.Modules.VFX.Generic.MaterialInterpolationConfiguration;
using UnityEngine;

namespace Popeye.Modules.VFX.Generic.ParticleBehaviours
{
    public abstract class InterpolatorRecycleParticle : RecyclableObject
    {
        [SerializeField] internal bool _interpolateOnInit;
        [SerializeField] internal MaterialFloatSetupConfig[] _floatSetupDatas;
        [SerializeField] internal MaterialFloatInterpolationConfig[] _floatInterpolationDatas;
        
        internal List<Material> _materials = new();
        private int _completedInterpolations = 0;

        internal override void Init()
        {
            _completedInterpolations = 0;
            
            foreach (var material in _materials)
            {
                Setup(material);
                
                if (_interpolateOnInit)
                {
                    ApplyInterpolations(material).Forget();
                }
            }
        }

        private void Setup(Material material)
        {
            MaterialInterpolator.Setup(material, _floatSetupDatas);
        }

        public void Play()
        {
            foreach (var material in _materials)
            {
                ApplyInterpolations(material).Forget();
            }
        }
        
        private async UniTaskVoid ApplyInterpolations(Material material)
        {
            await MaterialInterpolator.ApplyInterpolations(material, _floatInterpolationDatas);
            _completedInterpolations++;
            
            if (_completedInterpolations >= _materials.Count)
            {
                Reset();
            }
        }

        internal virtual void Reset()
        {
            Recycle();
        }
    }
}