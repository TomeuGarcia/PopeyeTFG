using System;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class Dead_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;
        
        private bool _finishedDying;

        public Dead_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        
        protected override void DoEnter()
        {
            _blackboard.PlayerMediator.SetMaxMovementSpeed(0);
            _blackboard.PlayerMediator.SetCanRotate(false);
            _blackboard.PlayerMediator.PlayerView.PlayDeathAnimation();
            _blackboard.PlayerMediator.SetEnabledFallingPhysics(false);

            _blackboard.PlayerDeathAnimationSequencer.PlayDeathAnimation();
            
            WaitForDeathToFinish().Forget();
        }

        public override void Exit()
        {
            _blackboard.PlayerMediator.SetCanRotate(true);
            _blackboard.PlayerMediator.SetEnabledFallingPhysics(true);
        }

        public override bool Update(float deltaTime)
        {
            if (_finishedDying)
            {
                _blackboard.PlayerMediator.RespawnFromDeath();
                NextState = PlayerStates.Spawning;
                return true;
            }

            return false;
        }

        private async UniTaskVoid WaitForDeathToFinish()
        {
            _finishedDying = false;

            await UniTask.WaitUntil(() => !_blackboard.PlayerDeathAnimationSequencer.IsPlayingDeathAnimation);
            
            await _blackboard.PlayerDeathAnimationSequencer.FinishAnimation(_blackboard.PlayerView);
            
            _finishedDying = true;
        }
        
    }
}