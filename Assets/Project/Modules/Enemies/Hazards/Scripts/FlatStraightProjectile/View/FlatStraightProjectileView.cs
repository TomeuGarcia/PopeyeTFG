using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.VFX.ParticleFactories;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public class FlatStraightProjectileView : MonoBehaviour, IFlatStraightProjectileView
    {
        [SerializeField] private Transform _testTweenTransform;
        private FlatStraightProjectileViewConfig _config;
        
        private IParticleFactory _particleFactory;
        
        public void Configure(IParticleFactory particleFactory, FlatStraightProjectileViewConfig config)
        {
            _particleFactory = particleFactory;
            _config = config;
        }

        public void ResetView()
        {
            _testTweenTransform.localScale = Vector3.one;
        }

        public void PlayStartShootAnimation()
        {
            _testTweenTransform.PunchScale(_config.StartShootScalePunch);
        }

        public async UniTask PlayObjectContactAnimation()
        {
            _testTweenTransform.DOComplete();
            await _testTweenTransform.PunchScale(_config.ObjectContactScalePunch)
                .AsyncWaitForCompletion();
        }

        public void PlayDisappearAnimation(float duration)
        {
            _config.DisappearScale.SetDuration(duration);
            _testTweenTransform.Scale(_config.DisappearScale);
        }
        
    }
}