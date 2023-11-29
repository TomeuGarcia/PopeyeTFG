
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class MovingWithoutAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;

        public MovingWithoutAnchor_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        protected override void DoEnter()
        {
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.WithoutAnchorMoveSpeed);
        }

        public override void Exit()
        {
            
        }

        public override bool Update(float deltaTime)
        {
            //if (_blackboard.MovesetInputsController.PickUp_Pressed() && PlayerCanPickUpAnchor())
            if (PlayerCanPickUpAnchor())
            {
                NextState = PlayerStates.PickingUpAnchor;
                return true;
            }
            
            return false;
        }

        
        private bool PlayerCanPickUpAnchor()
        {
            return _blackboard.AnchorMediator.IsRestingOnFloor() && 
                   _blackboard.PlayerMediator.GetDistanceFromAnchor() < _blackboard.PlayerStatesConfig.AnchorPickUpDistance;
        }
        
    }
}