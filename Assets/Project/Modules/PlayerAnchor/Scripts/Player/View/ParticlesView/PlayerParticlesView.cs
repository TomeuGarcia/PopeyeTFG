using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.VFX.Generic.ParticleBehaviours;
using Popeye.Modules.VFX.ParticleFactories;
using Unity.Mathematics;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerParticlesView : IPlayerView
    {
        private readonly PlayerParticlesViewConfig _config;
        private readonly Material _material;
        private readonly Transform _transformHolder;
        private readonly IParticleFactory _particleFactory;

        public PlayerParticlesView(PlayerParticlesViewConfig config, Renderer renderer, Transform transformHolder, IParticleFactory particleFactory)
        {
            _config = config;
            _material = renderer.material;
            _transformHolder = transformHolder;
            _particleFactory = particleFactory;
        }
        
        public void StartTired()
        {
        }

        public void EndTired()
        {
        }

        public void PlayTakeDamageAnimation()
        {
        }

        public void PlayRespawnAnimation()
        {
        }

        public void PlayDeathAnimation()
        {
        }

        public void PlayHealAnimation()
        {
        }

        public void PlayDashAnimation(float duration, Vector3 dashDirection)
        {
            DoPlayDash(duration, dashDirection).Forget();
        }

        private async UniTaskVoid DoPlayDash(float duration, Vector3 dashDirection)
        {
            _particleFactory.Create(_config.DashDisappearParticleType, Vector3.zero, quaternion.identity, _transformHolder);
            await UniTask.Delay(TimeSpan.FromSeconds(Mathf.Max(0.0f, _config.TrailSpawnDelay)));
            
            Transform trail = _particleFactory.Create(_config.DashTrailParticleType, Vector3.zero, quaternion.identity, _transformHolder);
            Vector3 trailScale = trail.localScale;
            trail.DOLocalRotate(_config.DashTrailRotation, duration - _config.TrailSpawnDelay, RotateMode.LocalAxisAdd)
                .SetEase(_config.DashTrailRotationEase);
            trail.DOScale(Vector3.zero, duration - _config.TrailSpawnDelay)
                .SetEase(_config.DashTrailScaleEase).OnComplete(() =>
                {
                    trail.SetParent(null);
                    trail.localScale = trailScale;
                });
            await UniTask.Delay(TimeSpan.FromSeconds(duration - _config.TrailSpawnDelay));
            
            _particleFactory.Create(_config.DashAppearParticleType, Vector3.zero, quaternion.identity, _transformHolder);
        }

        public void PlayKickAnimation()
        {
        }

        public void PlayThrowAnimation()
        {
        }

        public async UniTaskVoid PlayPullAnimation(float delay)
        {
        }

        public void PlayAnchorObstructedAnimation()
        {
        }

        public void PlayEnterIdleAnimation()
        {
        }

        public void PlayExitIdleAnimation()
        {
        }

        public void UpdateMovingAnimation(float isMovingRatio01)
        {
        }

        public void PlayEnterMovingWithAnchorAnimation()
        {
        }

        public void PlayEnterMovingWithoutAnchorAnimation()
        {
        }

        public void PlayEnterAimingAnimation()
        {
        }

        public void PlayPickUpAnchorAnimation()
        {
        }
    }
}