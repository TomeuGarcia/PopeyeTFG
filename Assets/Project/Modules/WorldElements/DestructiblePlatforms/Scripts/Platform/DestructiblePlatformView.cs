using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.VFX.ParticleFactories;
using UnityEngine;

namespace Project.Modules.WorldElements.DestructiblePlatforms
{
    public class DestructiblePlatformView
    {
        private readonly Transform _meshTransform;
        private readonly Material _meshMaterial;
        private IParticleFactory _particleFactory;
        private readonly DestructiblePlatformConfig.AnimationConfigData _config;

        private readonly Vector3 _originalLocalPosition;
        
        public DestructiblePlatformView(Transform meshTransform, Material meshMaterial, IParticleFactory particleFactory,
            DestructiblePlatformConfig.AnimationConfigData config)
        {
            _meshTransform = meshTransform;
            _meshMaterial = meshMaterial;
            _particleFactory = particleFactory;
            _config = config;

            _originalLocalPosition = _meshTransform.localPosition;
            
            _meshMaterial.SetFloat(_config.AnimationPropertyID, 0.0f);
        }

        public async UniTaskVoid StartPlayingBreakingOverTimeAnimation(float duration)
        {
            float punchDuration = duration * 0.4f;
            float remainingDuration = duration - punchDuration;
            
            _meshTransform.DOPunchPosition(Vector3.down * 0.1f, duration, 8)
                .SetEase(Ease.InOutSine);
            await _meshTransform.DOPunchRotation(Vector3.up * 5.0f, punchDuration, 8)
                .SetEase(Ease.InOutSine).AsyncWaitForCompletion();
            
            _meshTransform.DOPunchPosition(Vector3.down * 0.1f, remainingDuration, 8)
                .SetEase(Ease.OutSine);
        }
        
        public void FinishPlayingBreakingOverTimeAnimation()
        {
            _meshTransform.DOPunchRotation(Vector3.up * 5.0f, _config.BreakingDuration, 8)
                .SetEase(Ease.InOutSine);
        }
        
        public void PlayBreakAnimation()
        {
            PlayShaderAnimation(0, 1, _config.BreakingDuration, _config.BreakingColor, Ease.InOutSine);
        }

        public void PlayRegenerateAnimation()
        {
            _meshTransform.localPosition = _originalLocalPosition; 
            PlayShaderAnimation(1, 0, _config.RegeneratingDuration, _config.RegeneratingColor, Ease.InOutCubic);
        }


        private void PlayShaderAnimation(float start, float end, float duration, Color color, Ease ease)
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
                .SetEase(ease);
        }
        
    }
}