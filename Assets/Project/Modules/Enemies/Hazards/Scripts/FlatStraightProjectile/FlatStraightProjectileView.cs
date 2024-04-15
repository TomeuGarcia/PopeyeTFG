using DG.Tweening;
using Popeye.Modules.VFX.ParticleFactories;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public class FlatStraightProjectileView : MonoBehaviour, IFlatStraightProjectileView
    {
        [SerializeField] private Transform _testTweenTransform;
        
        private IParticleFactory _particleFactory;
        
        public void Configure(IParticleFactory particleFactory)
        {
            _particleFactory = particleFactory;
        }

        public void ResetView()
        {
            _testTweenTransform.localScale = Vector3.one;
        }

        public void PlayStartShootAnimation()
        {
            _testTweenTransform.DOPunchScale(Vector3.forward, 0.5f, 4);
        }

        public void PlayHitObjectAnimation()
        {
            _testTweenTransform.DOPunchScale(Vector3.back, 0.2f, 4);
        }

        public void PlayDisappearAnimation(float duration)
        {
            _testTweenTransform.DOScale(Vector3.zero, duration)
                .SetEase(Ease.InOutQuad);
        }
        
    }
}