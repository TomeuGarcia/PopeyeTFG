
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

            if (PlayerCanPullAnchor())
            {
                NextState = PlayerStates.PullingAnchor;
                return true;
            }

            if (PlayerCanDashTowardsAnchor())
            {
                NextState = PlayerStates.DashingTowardsAnchor;
                return true;
            }

            if (PlayerCanKickAnchor())
            {
                NextState = PlayerStates.KickingAnchor;
                return true;
            }

            if (PlayerTriesToSpinAnchor())
            {
                if (IsAnchorObstructed())
                {
                    _blackboard.PlayerMediator.OnTryUsingObstructedAnchor();
                }
                else
                {
                    NextState = PlayerStates.SpinningAnchor;
                    return true;   
                }
            }
            
            if (PlayerCanHeal())
            {
                NextState = PlayerStates.Healing;
                return true;
            }
            
            return false;
        }

        
        private bool PlayerCanPickUpAnchor()
        {
            return _blackboard.AnchorMediator.IsRestingOnFloor() && 
                   _blackboard.PlayerMediator.GetDistanceFromAnchor() < _blackboard.PlayerStatesConfig.AnchorPickUpDistance;
        }

        private bool PlayerCanPullAnchor()
        {
            return _blackboard.MovesetInputsController.Pull_Pressed();
        }

        private bool PlayerCanDashTowardsAnchor()
        {
            if (_blackboard.queuedDashTowardsAnchor)
            {
                _blackboard.queuedDashTowardsAnchor = false;
                return true;
            }

            return _blackboard.MovesetInputsController.Dash_Pressed();
        }

        private bool PlayerCanKickAnchor()
        {
            return _blackboard.MovesetInputsController.Kick_Pressed() &&
                   _blackboard.AnchorMediator.IsRestingOnFloor() && 
                   _blackboard.PlayerMediator.GetDistanceFromAnchor() < _blackboard.PlayerStatesConfig.AnchorKickDistance;
        }

        private bool PlayerTriesToSpinAnchor()
        {
            return _blackboard.MovesetInputsController.SpinAttack_Pressed(out _blackboard.spinAttackTowardsRight) && 
                   _blackboard.PlayerMediator.CanSpinAnchor();
        }

        private bool IsAnchorObstructed()
        {
            return _blackboard.AnchorMediator.IsObstructedByObstacles();
        }
        
        private bool PlayerCanHeal()
        {
            return _blackboard.MovesetInputsController.Heal_Pressed() && _blackboard.PlayerMediator.CanHeal();
        }
    }
}