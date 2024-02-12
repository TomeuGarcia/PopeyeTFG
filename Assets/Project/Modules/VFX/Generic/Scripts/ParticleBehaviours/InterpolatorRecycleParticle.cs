using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Popeye.Core.Pool;
using Popeye.Modules.VFX.Generic.MaterialInterpolationConfiguration;
using UnityEngine;

namespace Popeye.Modules.VFX.Generic.ParticleBehaviours
{
    public class InterpolatorRecycleParticle : RecyclableObject
    {
        [SerializeField] internal bool _interpolateOnInit;
        [SerializeField] internal InterpolatorRecycleParticleData[] _interpolations;
        
        //TODO this list is never used -> it is required for a fix
        internal List<TrailRenderer> _trailRenderers = new();
        
        private int _completedInterpolations;
        private int _totalInterpolations;

        private void Awake()
        {
            foreach (var interpolation in _interpolations)
            {
                interpolation.Awake();
                if (interpolation.TrailRenderers.Count > 0)
                {
                    _trailRenderers.AddRange(interpolation.TrailRenderers);
                }
            }

            TrailEmission(false);
        }

        internal override void Init()
        {
            _completedInterpolations = 0;
            
            TrailEmission(true);

            foreach (var interpolation in _interpolations)
            {
                foreach (var material in interpolation.Materials)
                {
                    Setup(material, interpolation.FloatSetupDatas);
                
                    if (_interpolateOnInit)
                    {
                        ApplyInterpolations(material, interpolation.FloatInterpolationDatas).Forget();
                    }
                }
            }
        }

        private void TrailEmission(bool emission)
        {
            
            foreach (var trail in _trailRenderers)
            {
                //trail.Clear();
                //trail.emitting = emission;
            }
            
        }

        private void Setup(Material material, MaterialFloatSetupConfig[] setupConfigs)
        {
            MaterialInterpolator.Setup(material, setupConfigs);
        }

        public void Play()
        {
            foreach (var interpolation in _interpolations)
            {
                foreach (var material in interpolation.Materials)
                {
                    ApplyInterpolations(material, interpolation.FloatInterpolationDatas).Forget();
                }
            }
        }
        
        private async UniTaskVoid ApplyInterpolations(Material material, MaterialFloatInterpolationConfig[] interpolationConfigs)
        {
            await MaterialInterpolator.ApplyInterpolations(material, interpolationConfigs);
            _completedInterpolations++;
            
            if (_completedInterpolations >= _interpolations.Length)
            {
                Reset();
            }
        }

        internal virtual void Reset()
        {
            TrailEmission(false);
            Recycle();
        }
        
        internal override void Release() { }
    }
}