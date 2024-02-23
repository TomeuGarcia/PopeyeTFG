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
        }
        
        
        public void PlayEnterIdleAnimation()
        {
            Debug.Log("Enter Idle");
            
            _animator.SetBool(_config.IdleParameter, true);
            
            _animator.SetLayerWeight(_animator.GetLayerIndex(_config.LegsLayer), 0f);
        }

        public void PlayExitIdleAnimation()
        {
            Debug.Log("Exit Idle");
            
            _animator.SetBool(_config.IdleParameter, false);
            
            _animator.SetLayerWeight(_animator.GetLayerIndex(_config.LegsLayer), 1f);
        }

        public void UpdateMovingAnimation(float isMovingRatio01)
        {
            _animator.SetFloat(_config.IdleToMovingParameter, isMovingRatio01);
            //_animator.SetLayerWeight(_animator.GetLayerIndex(_config.LegsLayer), isMovingRatio01);
        }

        public void PlayEnterMovingWithAnchorAnimation()
        {
            Debug.Log("Moving with Anchor");
            
            _animator.SetBool(_config.MovingWithAnchorParameter, true);
            _animator.SetBool(_config.MovingWithoutAnchorParameter, false);
            _animator.SetBool(_config.AimingParameter, false);
            
            _animator.SetBool(_config.PickUpAnchorParameter, false);
        }

        public void PlayEnterMovingWithoutAnchorAnimation()
        {
            Debug.Log("Moving without Anchor");
            
            _animator.SetBool(_config.MovingWithAnchorParameter, false);
            _animator.SetBool(_config.MovingWithoutAnchorParameter, true);
            _animator.SetBool(_config.AimingParameter, false);
            
            
            _animator.SetBool(_config.ThrowingAnchorParameter, false);
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
            
            _animator.SetBool(_config.PickUpAnchorParameter, true);
        }

        public void PlayThrowAnimation()
        {
            Debug.Log("Throwing Anchor");
            
            _animator.SetBool(_config.AimingParameter, true);
            _animator.SetBool(_config.ThrowingAnchorParameter, true);
        }

        public async UniTaskVoid PlayPullAnimation(float delay)
        {
            Debug.Log("Pulling Anchor");
            
            _animator.SetTrigger(_config.PullingAnchorParameter);
        }

        public void PlayDashAnimation(float duration)
        {
            Debug.Log("Dashing Anchor");
            _animator.SetBool(_config.MovingWithAnchorParameter, false);
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

        

        public void PlayKickAnimation()
        {
        }
        

        public void PlayAnchorObstructedAnimation()
        {
        }

    }
}