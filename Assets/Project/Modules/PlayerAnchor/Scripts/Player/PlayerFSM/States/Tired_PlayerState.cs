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
            UpdateMovementSpeed();
            _blackboard.PlayerStatesConfig.OnSpeedValueChanged += UpdateMovementSpeed;
            
            if (_blackboard.cameFromState == PlayerStates.TiredPickingUpAnchor) return;

            _blackboard.PlayerMediator.SetCanRotate(true);
            _blackboard.PlayerView.StartTired();

            if (ShouldDropAnchor())
            {
                _blackboard.AnchorMediator.SnapToFloor(_blackboard.PlayerMediator.Position).Forget();
            }
        }

        public override void Exit()
        {
            _blackboard.PlayerStatesConfig.OnSpeedValueChanged -= UpdateMovementSpeed;
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
         
            if (_blackboard.AnchorMediator.IsGrabbedBySnapper() &&
                _blackboard.cameFromState == PlayerStates.DashingTowardsAnchor)
            {
                NextState = PlayerStates.TiredPickingUpAnchor;
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

        private bool ShouldDropAnchor()
        {
            return _blackboard.PlayerStatesConfig.DropAnchorWhenTired &&
                   _blackboard.cameFromState == PlayerStates.PullingAnchor;
        }

        private void UpdateMovementSpeed()
        {
            bool cameMovingWithAnchor = 
                _blackboard.cameFromState == PlayerStates.MovingWithAnchor ||
                _blackboard.cameFromState == PlayerStates.TiredPickingUpAnchor;

            float maxMovementSpeed = cameMovingWithAnchor
                ? _blackboard.PlayerStatesConfig.TiredWithAnchorMoveSpeed
                : _blackboard.PlayerStatesConfig.TiredWithoutAnchorMoveSpeed;

            _blackboard.PlayerMediator.SetMaxMovementSpeed(maxMovementSpeed);
            _blackboard.PlayerMovementChecker.MaxMovementSpeed = maxMovementSpeed;
        }
    }
}