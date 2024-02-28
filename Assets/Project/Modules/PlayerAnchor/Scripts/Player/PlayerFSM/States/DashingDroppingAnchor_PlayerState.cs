using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class DashingDroppingAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;
        private bool _finishedDashing;
        
        public DashingDroppingAnchor_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        protected override void DoEnter()
        {
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.DashingMoveSpeed);
            StartDashing().Forget();
        }

        public override void Exit()
        {
            
        }

        public override bool Update(float deltaTime)
        {
            if (_finishedDashing)
            {
                NextState = PlayerStates.MovingWithoutAnchor;
                return true;
            }

            if (_blackboard.MovesetInputsController.Pull_Pressed())
            {
                _blackboard.queuedAnchorPull = true;
            }

            return false;
        }
        
        
        private async UniTaskVoid StartDashing()
        {
            _finishedDashing = false;
            
            await _blackboard.PlayerMediator.DashForward();

            _finishedDashing = true;
        }
    }
}