using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.Camera.CameraZoom;
using Project.General.Scripts.Core.Services.ScreenFade;
using Project.Scripts.Time.TimeHitStop;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerGameFeelEffectsView : IPlayerView
    {
        private readonly PlayerGameFeelEffectsViewConfig _viewConfig;
        private readonly IHitStopManager _hitStopManager;
        private readonly ICameraShaker _cameraShaker;
        private readonly ICameraZoomer _cameraZoomer;

        public PlayerGameFeelEffectsView(PlayerGameFeelEffectsViewConfig viewConfig,
            IHitStopManager hitStopManager, ICameraShaker cameraShaker, ICameraZoomer cameraZoomer)
        {
            _viewConfig = viewConfig;
            _hitStopManager = hitStopManager;
            _cameraShaker = cameraShaker;
            _cameraZoomer = cameraZoomer;
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

        public void PlayDeathFinishAnimation(float duration)
        {
        }

        public void PlayHealAnimation()
        {
        }
        public void PlayStartHealingAnimation(float durationToComplete, int consecutiveHeals)
        {
            if (consecutiveHeals >= 1) return;
            
            /*
            _viewConfig.HealingZoomInOut.ZoomInConfig.SetDuration(durationToComplete);
            _cameraZoomer.ZoomInOutToDefault(_viewConfig.HealingZoomInOut);
            */
            
            _viewConfig.HealingZoomIn.SetDuration(durationToComplete * 3);
            _cameraZoomer.ZoomIn(_viewConfig.HealingZoomIn);
        }
        public void PlayHealingInterruptedAnimation()
        {
            _cameraZoomer.KillCurrentZoom();
            _cameraZoomer.ZoomToDefault(_viewConfig.InterruptedToZoomOut);
        }

        public void PlaySpecialAttackAnimation()
        {
            
        }

        public void PlaySpecialAttackFinishAnimation()
        {
        }

        public void PlayStartEnteringSpecialAttackAnimation(float durationToComplete)
        {
            _viewConfig.SpecialAttackZoomInOut.ZoomInConfig.SetDuration(durationToComplete);
            _cameraZoomer.ZoomInOutToDefault(_viewConfig.SpecialAttackZoomInOut);
        }

        public void PlaySpecialAttackInterruptedAnimation()
        {
            _cameraZoomer.KillCurrentZoom();
            _cameraZoomer.ZoomToDefault(_viewConfig.InterruptedToZoomOut);
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