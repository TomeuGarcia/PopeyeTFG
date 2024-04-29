using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Core.Pool;
using Popeye.Modules.VFX.Generic.MaterialInterpolationConfiguration;
using UnityEngine;

namespace Popeye.Modules.VFX.Generic.ParticleBehaviours
{
    public class InterpolatorRecycleParticle : RecyclableObject
    {
        [SerializeField] internal bool _interpolateOnInit;
        [SerializeField] internal InterpolatorRecycleParticleData[] _interpolations;
        
        [Header("LIGHT")]
        [SerializeField] private Light _light;
        [SerializeField] private float _duration;
        [SerializeField] private float _intensityGoal;
        [SerializeField] private Ease _lightEase = Ease.Linear;
        private float _initialIntensity;
        
        internal List<TrailRenderer> _trailRenderers = new();
        
        private int _completedInterpolations;
        private int _totalInterpolations;

        private bool _waitingForLight;

        private void Awake()
        {
            if (_light != null)
            {
                _initialIntensity = _light.intensity;
                _waitingForLight = true;
            }
            else
            {
                _waitingForLight = false;
            }
            
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

            TrailInit().Forget();
        }

        private async UniTaskVoid TrailInit()
        {
            await UniTask.WaitForEndOfFrame();
            TrailEmission(true);
        }

        private void TrailEmission(bool emission)
        {
            foreach (var trail in _trailRenderers)
            {
                trail.Clear();
                trail.emitting = emission;
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

        public void ForceStop()
        {
            Reset();
        }
        
        private async UniTaskVoid ApplyInterpolations(Material material, MaterialFloatInterpolationConfig[] interpolationConfigs)
        {
            if (_light != null)
            {
                Debug.Log("A");
                _light.DOIntensity(_intensityGoal, _duration).SetEase(_lightEase).OnComplete(LightCompleted);
            }
            
            await MaterialInterpolator.ApplyInterpolations(material, interpolationConfigs);
            _completedInterpolations++;
            
            if (_completedInterpolations >= _interpolations.Length + 1)
            {
                Reset();
            }
        }

        private void LightCompleted()
        {
            Debug.Log("A");
            _light.intensity = 0.0f;
            _completedInterpolations++;
            
            if (_completedInterpolations >= _interpolations.Length + 1)
            {
                Reset();
            }
        }

        internal virtual void Reset()
        {
            if (_light != null)
            {
                _light.intensity = _initialIntensity;
            }
            
            TrailEmission(false);
            Recycle();
        }
        
        internal override void Release() { }
    }
}