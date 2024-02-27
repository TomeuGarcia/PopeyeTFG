using Cysharp.Threading.Tasks;
using Popeye.Modules.Camera.CameraShake;
using Project.Scripts.Time.TimeHitStop;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerGameFeelEffectsView : IPlayerView
    {
        private readonly PlayerGameFeelEffectsViewConfig _viewConfig;
        private readonly IHitStopManager _hitStopManager;
        private readonly ICameraShaker _cameraShaker;

        
        
        public PlayerGameFeelEffectsView(PlayerGameFeelEffectsViewConfig viewConfig,
            IHitStopManager hitStopManager, ICameraShaker cameraShaker)
        {
            _viewConfig = viewConfig;
            _hitStopManager = hitStopManager;
            _cameraShaker = cameraShaker;
        }
        
        
        public void StartTired()
        {
        }

        public void EndTired()
        {
        }

        public void PlayTakeDamageAnimation()
        {
            _hitStopManager.QueueHitStop(_viewConfig.TakeDamageHitStop);
            _cameraShaker.PlayShake(_viewConfig.TakeDamageCameraShake);
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