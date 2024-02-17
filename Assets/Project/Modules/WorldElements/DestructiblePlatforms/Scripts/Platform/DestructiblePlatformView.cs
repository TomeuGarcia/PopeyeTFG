using DG.Tweening;
using Popeye.Modules.VFX.ParticleFactories;
using UnityEngine;

namespace Project.Modules.WorldElements.DestructiblePlatforms
{
    public class DestructiblePlatformView
    {
        private Transform _meshTransform;
        private Material _meshMaterial;
        private IParticleFactory _particleFactory;
        private readonly DestructiblePlatformConfig.AnimationConfigData _config;

        
        public DestructiblePlatformView(Transform meshTransform, Material meshMaterial, IParticleFactory particleFactory,
            DestructiblePlatformConfig.AnimationConfigData config)
        {
            _meshTransform = meshTransform;
            _meshMaterial = meshMaterial;
            _particleFactory = particleFactory;
            _config = config;

            _meshMaterial.SetFloat(_config.AnimationPropertyID, 0.0f);
        }

        public void StartPlayingBreakingOverTimeAnimation(float duration)
        {
            float punchDuration = duration * 0.4f;
            _meshTransform.DOPunchPosition(Vector3.down * 0.2f, punchDuration, 8)
                .SetEase(Ease.InOutSine);
            _meshTransform.DOPunchRotation(Vector3.up * 5.0f, punchDuration, 8)
                .SetEase(Ease.InOutSine);
        }
        
        public void FinishPlayingBreakingOverTimeAnimation()
        {
            
        }
        
        public void PlayBreakAnimation()
        {
            PlayShaderAnimation(0, 1, _config.BreakingDuration, _config.BreakingColor);
        }

        public void PlayRegenerateAnimation()
        {
            PlayShaderAnimation(1, 0, _config.RegeneratingDuration, _config.RegeneratingColor);
        }


        private void PlayShaderAnimation(float start, float end, float duration, Color color)
        {
            _meshMaterial.SetColor(_config.ColorPropertyID, color);
            
            float animationT = start;
            DOTween.To(
                    () => animationT,
                    (value) =>
                    {
                        animationT = value;
                        _meshMaterial.SetFloat(_config.AnimationPropertyID, animationT);
                    },
                    end,
                    duration
                )
                .SetEase(Ease.InOutSine);
        }
        
    }
}