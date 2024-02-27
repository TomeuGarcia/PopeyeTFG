using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerAnimationsView : IPlayerView
    {
        private readonly PlayerAnimatorViewConfig _config;
        private readonly Animator _animator;


        public PlayerAnimationsView(PlayerAnimatorViewConfig config, Animator animator)
        {
            _config = config;
            _animator = animator;

            _config.OnValidate();
        }
        
        
        public void PlayEnterIdleAnimation()
        {
            SetAnimatorBool(_config.IdleParameterId, true);
        }

        public void PlayExitIdleAnimation()
        {
            SetAnimatorBool(_config.IdleParameterId, false);
        }

        public void UpdateMovingAnimation(float isMovingRatio01)
        {
            SetAnimatorFloat(_config.IdleToMovingParameterId, isMovingRatio01);
        }

        public void PlayEnterMovingWithAnchorAnimation()
        {
            SetAnimatorBool(_config.MovingWithAnchorParameterId, true);
            SetAnimatorBool(_config.MovingWithoutAnchorParameterId, false);
            SetAnimatorBool(_config.AimingParameterId, false);
            
            SetAnimatorBool(_config.PickUpAnchorParameterId, false);
        }

        public void PlayEnterMovingWithoutAnchorAnimation()
        {
            SetAnimatorBool(_config.MovingWithAnchorParameterId, false);
            SetAnimatorBool(_config.MovingWithoutAnchorParameterId, true);
            SetAnimatorBool(_config.AimingParameterId, false);
            
            SetAnimatorBool(_config.ThrowingAnchorParameterId, false);
            SetAnimatorBool(_config.PullingAnchorParameterId, false);
        }

        public void PlayEnterAimingAnimation()
        {
            SetAnimatorBool(_config.MovingWithAnchorParameterId, false);
            SetAnimatorBool(_config.MovingWithoutAnchorParameterId, false);
            SetAnimatorBool(_config.AimingParameterId, true);
            
            SetAnimatorBool(_config.ThrowingAnchorParameterId, false); //
        }

        public void PlayPickUpAnchorAnimation()
        {
            SetAnimatorBool(_config.PickUpAnchorParameterId, true);
        }

        public void PlayThrowAnimation()
        {
            SetAnimatorBool(_config.AimingParameterId, false);
            SetAnimatorBool(_config.ThrowingAnchorParameterId, true);
            
            SetAnimatorBool(_config.MovingWithoutAnchorParameterId, true); //
        }

        public async UniTaskVoid PlayPullAnimation(float delay)
        {
            SetAnimatorBool(_config.PullingAnchorParameterId, true);
            
            SetAnimatorBool(_config.MovingWithAnchorParameterId, false); //
            SetAnimatorBool(_config.MovingWithoutAnchorParameterId, true); //
            SetAnimatorBool(_config.AimingParameterId, false); //
        }

        public void PlayDashAnimation(float duration)
        {
            SetAnimatorBool(_config.MovingWithAnchorParameterId, false);
        }
        
        public void StartTired()
        {
            SetAnimatorBool(_config.TiredParameterId, true);
        }

        public void EndTired()
        {
            SetAnimatorBool(_config.TiredParameterId, false);
        }



        private void SetAnimatorBool(int parameterId, bool value)
        {
            _animator.SetBool(parameterId, value);
        }

        private void SetAnimatorFloat(int parameterId, float value)
        {
            _animator.SetFloat(parameterId, value);
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

        

        public void PlayKickAnimation()
        {
        }
        

        public void PlayAnchorObstructedAnimation()
        {
        }

    }
}