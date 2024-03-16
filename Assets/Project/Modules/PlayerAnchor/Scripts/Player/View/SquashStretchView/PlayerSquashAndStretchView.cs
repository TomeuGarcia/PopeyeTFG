using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerSquashAndStretchView : IPlayerView
    {
        private readonly PlayerSquashStretchViewConfig _config;
        private readonly Transform _meshHolderTransform;



        public PlayerSquashAndStretchView(PlayerSquashStretchViewConfig config, Transform meshHolderTransform)
        {
            _config = config;
            _meshHolderTransform = meshHolderTransform;
        }
        
        
        
        public void StartTired()
        {
        }

        public void EndTired()
        {
        }

        public void PlayTakeDamageAnimation()
        {
            ClearTweens();
            PunchScale(_config.TakeDamageScalePunch);
        }

        public void PlayRespawnAnimation()
        {
            _meshHolderTransform.localRotation = Quaternion.identity;
            _meshHolderTransform.localPosition = Vector3.zero;
        }

        public void PlayDeathAnimation()
        {            
            _meshHolderTransform.Rotate(_config.DeathRotation);
            _meshHolderTransform.BlendableLocalMoveBy(_config.DeathMoveBy);
        }

        public void PlayHealAnimation()
        {
            ClearTweens();
            PunchScale(_config.HealScalePunch);
        }
        public void PlayStartHealingAnimation(float durationToComplete)
        {
            PunchScale(_config.StartHealingScalePunch);
        }
        public void PlayHealingInterruptedAnimation()
        {
            KillTweens();
            _meshHolderTransform.DOScale(Vector3.one, 0.1f);
        }


        public void PlayDashAnimation(float duration, Vector3 dashDirection)
        {
            ClearTweens();

            _config.DashScalePunch.Duration = duration;
            _config.DashRotationPunch.Duration = duration;
            PunchScale(_config.DashScalePunch);
            PunchRotate(_config.DashRotationPunch);
        }

        public void PlayKickAnimation()
        {
            ClearTweens();
            PunchScale(_config.KickScalePunch);
            PunchRotate(_config.KickRotationPunch);
        }

        public void PlayThrowAnimation()
        {
            ClearTweens();
            PunchScale(_config.ThrowScalePunch);
            PunchRotate(_config.ThrowRotationPunch);
        }

        public async UniTaskVoid PlayPullAnimation(float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            
            ClearTweens();
            PunchScale(_config.PullScalePunch);
            PunchRotate(_config.PullRotationPunch);
        }

        public void PlayAnchorObstructedAnimation()
        {
            ClearTweens();
            PunchRotate(_config.AnchorObstructedRotationPunch);
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

        private void ClearTweens()
        {
            _meshHolderTransform.DOComplete();
        }
        private void KillTweens()
        {
            _meshHolderTransform.DOKill();
        }
        
        private void PunchRotate(TweenPunchConfig tweenConfig)
        {
            _meshHolderTransform.PunchRotation(tweenConfig);
        }
        private void PunchScale(TweenPunchConfig punchConfig)
        {
            _meshHolderTransform.PunchScale(punchConfig);
        }
        
    }
}