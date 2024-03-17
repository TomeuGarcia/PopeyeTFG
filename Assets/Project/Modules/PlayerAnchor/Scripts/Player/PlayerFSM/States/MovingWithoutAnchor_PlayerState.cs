
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
            _blackboard.PlayerStatesConfig.OnSpeedValueChanged += UpdateMovementSpeed;
            UpdateMovementSpeed();
            
            
            _blackboard.PlayerMediator.DestructiblePlatformBreaker.SetBreakOverTimeMode();
            _blackboard.PlayerMediator.DestructiblePlatformBreaker.SetEnabled(true);
            
            _blackboard.PlayerMediator.PlayerView.PlayEnterMovingWithoutAnchorAnimation();
            
            _blackboard.PlayerMovementChecker.MaxMovementSpeed = _blackboard.PlayerStatesConfig.WithoutAnchorMoveSpeed;
        }

        public override void Exit()
        {
            _blackboard.PlayerMediator.DestructiblePlatformBreaker.SetEnabled(false);
            _blackboard.PlayerStatesConfig.OnSpeedValueChanged -= UpdateMovementSpeed;
        }

        public override bool Update(float deltaTime)
        {
            _blackboard.PlayerMediator.UpdateSafeGroundChecking(deltaTime, out bool playerIsOnVoid, out bool anchorIsOnVoid);
            if (anchorIsOnVoid)
            {
                _blackboard.PlayerMediator.OnAnchorEndedInVoid();
                return false;
            }

            if (playerIsOnVoid)
            {
                _blackboard.PlayerMediator.OnPlayerFellOnVoid();
                NextState = PlayerStates.FallingOnVoid;
                return true;
            }
            
            
            
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

            /*
            if (PlayerCanKickAnchor())
            {
                NextState = PlayerStates.KickingAnchor;
                return true;
            }
            */

            /*
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
            */
            
            if (PlayerCanHeal(out bool hasHealsLeft))
            {
                NextState = PlayerStates.Healing;
                return true;
            }
            if (PlayerCanDoSpecialAttack())
            {
                NextState = PlayerStates.EnteringSpecialAttack;
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
            if (_blackboard.queuedAnchorPull && _blackboard.AnchorMediator.IsRestingOnFloor())
            {
                _blackboard.queuedAnchorPull = false;
                return true;
            }
            
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
        
        private bool PlayerCanHeal(out bool hasHealsLeft)
        {
            hasHealsLeft = false;
            
            return _blackboard.MovesetInputsController.Heal_Pressed() && 
                   _blackboard.PlayerMediator.PlayerHealing.CanHeal(out hasHealsLeft);
        }
        
        private bool PlayerCanDoSpecialAttack()
        {
            return _blackboard.MovesetInputsController.SpecialAttack_Pressed() && 
                   _blackboard.PlayerMediator.CanDoSpecialAttack();
        }

        private void UpdateMovementSpeed()
        {
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.WithoutAnchorMoveSpeed);
        }
    }
}