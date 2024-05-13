using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Popeye.Modules.WorldElements.WorldInteractors
{
    public class InteractableTorch : AWorldInteractor
    {
        [SerializeField] private InteractableTorchConfig _config;
    
        [SerializeField] private ParticleSystem _fireParticles;
        [SerializeField] private Light _light;
        private float _startLightIntensity;
        
        protected override void DoAwake()
        {
            _startLightIntensity = _light.intensity;
            
            StopFire();
        }

        protected override void DoEnterActivatedState()
        {
            StartFire();
        }

        protected override void DoEnterDeactivatedState()
        {
            StopFire();
        }

        private void StartFire()
        {
            _fireParticles.Play();
            _light.DOIntensity(_startLightIntensity, _config.LightOnEase.Duration)
                .SetEase(_config.LightOnEase.Ease);
        }

        private void StopFire()
        {
            _fireParticles.Stop();
            _light.DOIntensity(0f, _config.LightOffEase.Duration)
                .SetEase(_config.LightOffEase.Ease);
        }
        
        
        public override async UniTask EnterActivatedStateAwait()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_config.LightOnEase.Duration), ignoreTimeScale: true);
        }
    }
}