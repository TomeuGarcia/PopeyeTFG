using Cysharp.Threading.Tasks;
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
        }
        
        
        public void PlayEnterIdleAnimation()
        {
            Debug.Log("Enter Idle");
            
            _animator.SetBool(_config.IdleParameter, true);
        }

        public void PlayExitIdleAnimation()
        {
            Debug.Log("Exit Idle");
            
            _animator.SetBool(_config.IdleParameter, false);
        }

        public void PlayEnterMovingWithAnchorAnimation()
        {
            Debug.Log("Moving with Anchor");
            
            _animator.SetBool(_config.MovingWithAnchorParameter, true);
            _animator.SetBool(_config.MovingWithoutAnchorParameter, false);
            _animator.SetBool(_config.AimingParameter, false);
            
            _animator.ResetTrigger(_config.ThrowingAnchorParameter);
        }

        public void PlayEnterMovingWithoutAnchorAnimation()
        {
            Debug.Log("Moving without Anchor");
            
            _animator.SetBool(_config.MovingWithAnchorParameter, false);
            _animator.SetBool(_config.MovingWithoutAnchorParameter, true);
            _animator.SetBool(_config.AimingParameter, false);
            
            _animator.ResetTrigger(_config.PullingAnchorParameter);
            _animator.ResetTrigger(_config.PickUpAnchorParameter);
        }

        public void PlayEnterAimingAnimation()
        {
            Debug.Log("Aiming");
            
            _animator.SetBool(_config.MovingWithAnchorParameter, false);
            _animator.SetBool(_config.MovingWithoutAnchorParameter, false);
            _animator.SetBool(_config.AimingParameter, true);
        }

        public void PlayPickUpAnchorAnimation()
        {
            Debug.Log("Pick Up Anchor");
            
            _animator.SetTrigger(_config.PickUpAnchorParameter);
        }

        public void PlayThrowAnimation()
        {
            Debug.Log("Throwing Anchor");
            
            _animator.SetTrigger(_config.ThrowingAnchorParameter);
        }

        public async UniTaskVoid PlayPullAnimation(float delay)
        {
            Debug.Log("Pulling Anchor");
            
            _animator.SetTrigger(_config.PullingAnchorParameter);
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

        public void PlayDashAnimation(float duration)
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