using System;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class DashingTowardsAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;

        private bool _finishedDashing;

        public DashingTowardsAnchor_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        protected override void DoEnter()
        {
            _blackboard.queuedDashTowardsAnchor = false;
            
            StartDashing().Forget();
        }

        public override void Exit()
        {
        }

        public override bool Update(float deltaTime)
        {
            if (_finishedDashing)
            {
                NextState = PlayerStates.PickingUpAnchor;
                //NextState = PlayerStates.MovingWithoutAnchor; // Player won't pick up anchor if on snap target
                return true;
            }

            return false;
        }

        private async UniTaskVoid StartDashing()
        {
            _finishedDashing = false;
            
            await _blackboard.PlayerMediator.DashTowardsAnchor();

            _finishedDashing = true;
        }
    }
}