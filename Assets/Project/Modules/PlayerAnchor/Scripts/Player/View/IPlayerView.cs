using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IPlayerView
    {
        void StartTired();
        void EndTired();
        void PlayTakeDamageAnimation();
        void PlayRespawnAnimation();
        void PlayDeathAnimation();
        void PlayHealAnimation();
        void PlayStartHealingAnimation(float durationToComplete);
        void PlayHealingInterruptedAnimation();
        
        void PlaySpecialAttackAnimation();
        void PlaySpecialAttackFinishAnimation();
        void PlayStartEnteringSpecialAttackAnimation(float durationToComplete);
        void PlaySpecialAttackInterruptedAnimation();
        
        void PlayDashAnimation(float duration, Vector3 dashDirection);
        void PlayKickAnimation();
        void PlayThrowAnimation();
        UniTaskVoid PlayPullAnimation(float delay);
        
        void PlayAnchorObstructedAnimation();


        void PlayEnterIdleAnimation();
        void PlayExitIdleAnimation();
        void UpdateMovingAnimation(float isMovingRatio01);
        void PlayEnterMovingWithAnchorAnimation();
        void PlayEnterMovingWithoutAnchorAnimation();
        void PlayEnterAimingAnimation();
        void PlayPickUpAnchorAnimation();

    }
}