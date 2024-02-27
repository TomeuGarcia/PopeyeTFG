using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class Tired_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;

        
        public Tired_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }


        protected override void DoEnter()
        {
            if (_blackboard.cameFromState == PlayerStates.TiredPickingUpAnchor) return;
            
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.TiredMoveSpeed);
            _blackboard.PlayerMediator.SetCanRotate(true);
            _blackboard.PlayerView.StartTired();
        }

        public override void Exit()
        {
            
        }

        public override bool Update(float deltaTime)
        {
            if (_blackboard.PlayerMediator.HasMaxStamina())
            {
                _blackboard.PlayerView.EndTired();

                NextState = _blackboard.AnchorMediator.IsBeingCarried()
                    ? PlayerStates.MovingWithAnchor
                    : PlayerStates.MovingWithoutAnchor;
                
                return true;
            }
            
            if (PlayerCanPickUpAnchor())
            {
                NextState = PlayerStates.TiredPickingUpAnchor;
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