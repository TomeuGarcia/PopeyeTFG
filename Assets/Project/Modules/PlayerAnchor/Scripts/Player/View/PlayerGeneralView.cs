using System;
using Cysharp.Threading.Tasks;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerGeneralView : IPlayerView
    {
        private readonly IPlayerView[] _subPlayerViews;


        public PlayerGeneralView(IPlayerView[] subPlayerViews)
        {
            _subPlayerViews = subPlayerViews;
        }
        
        
        public void StartTired()
        {
            foreach (IPlayerView playerView in _subPlayerViews)
            {
                playerView.StartTired();
            }
        }

        public void EndTired()
        {
            foreach (IPlayerView playerView in _subPlayerViews)
            {
                playerView.EndTired();
            }
        }

        public void PlayTakeDamageAnimation()
        {
            foreach (IPlayerView playerView in _subPlayerViews)
            {
                playerView.PlayTakeDamageAnimation();
            }
        }

        public void PlayRespawnAnimation()
        {
            foreach (IPlayerView playerView in _subPlayerViews)
            {
                playerView.PlayRespawnAnimation();
            }
        }

        public void PlayDeathAnimation()
        {
            foreach (IPlayerView playerView in _subPlayerViews)
            {
                playerView.PlayDeathAnimation();
            }
        }

        public void PlayHealAnimation()
        {
            foreach (IPlayerView playerView in _subPlayerViews)
            {
                playerView.PlayHealAnimation();
            }
        }

        public void PlayDashAnimation(float duration)
        {
            foreach (IPlayerView playerView in _subPlayerViews)
            {
                playerView.PlayDashAnimation(duration);
            }
        }

        public void PlayKickAnimation()
        {
            foreach (IPlayerView playerView in _subPlayerViews)
            {
                playerView.PlayKickAnimation();
            }
        }

        public void PlayThrowAnimation()
        {
            foreach (IPlayerView playerView in _subPlayerViews)
            {
                playerView.PlayThrowAnimation();
            }
        }

        public async UniTaskVoid PlayPullAnimation(float delay)
        {
            foreach (IPlayerView playerView in _subPlayerViews)
            {
                playerView.PlayPullAnimation(delay).Forget();
            }
        }

        public void PlayAnchorObstructedAnimation()
        {
            foreach (IPlayerView playerView in _subPlayerViews)
            {
                playerView.PlayAnchorObstructedAnimation();
            }
        }

        public void PlayEnterIdleAnimation()
        {
            foreach (IPlayerView playerView in _subPlayerViews)
            {
                playerView.PlayEnterIdleAnimation();
            }
        }

        public void PlayExitIdleAnimation()
        {
            foreach (IPlayerView playerView in _subPlayerViews)
            {
                playerView.PlayExitIdleAnimation();
            }
        }

        public void UpdateMovingAnimation(float isMovingRatio01)
        {
            foreach (IPlayerView playerView in _subPlayerViews)
            {
                playerView.UpdateMovingAnimation(isMovingRatio01);
            }
        }

        public void PlayEnterMovingWithAnchorAnimation()
        {
            foreach (IPlayerView playerView in _subPlayerViews)
            {
                playerView.PlayEnterMovingWithAnchorAnimation();
            }
        }

        public void PlayEnterMovingWithoutAnchorAnimation()
        {
            foreach (IPlayerView playerView in _subPlayerViews)
            {
                playerView.PlayEnterMovingWithoutAnchorAnimation();
            }
        }

        public void PlayEnterAimingAnimation()
        {
            foreach (IPlayerView playerView in _subPlayerViews)
            {
                playerView.PlayEnterAimingAnimation();
            }
        }

        public void PlayPickUpAnchorAnimation()
        {
            foreach (IPlayerView playerView in _subPlayerViews)
            {
                playerView.PlayPickUpAnchorAnimation();
            }
        }
    }
}