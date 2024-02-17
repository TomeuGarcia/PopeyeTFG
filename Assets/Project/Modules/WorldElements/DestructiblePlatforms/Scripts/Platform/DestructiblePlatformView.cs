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
        
        public DestructiblePlatformView(Transform meshTransform, Material meshMaterial, IParticleFactory particleFactory)
        {
            _meshTransform = meshTransform;
            _meshMaterial = meshMaterial;
            _particleFactory = particleFactory;
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
            _meshTransform.gameObject.SetActive(false);
        }

        public void PlayRegenerateAnimation()
        {
            _meshTransform.gameObject.SetActive(true);
        }
    }
}