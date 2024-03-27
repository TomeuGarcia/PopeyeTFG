
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using Popeye.Timers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class MovingWithoutAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;

        private Timer _enterPullingCooldown;
        private Timer _enterDashCooldown;
        

        public MovingWithoutAnchor_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;

            _enterPullingCooldown = new Timer(0);
            _enterDashCooldown = new Timer(0);
        }
        
        protected override void DoEnter()
        {
            _blackboard.PlayerStatesConfig.OnSpeedValueChanged += UpdateMovementSpeed;
            UpdateMovementSpeed();
            
            
            _blackboard.PlayerMediator.DestructiblePlatformBreaker.SetBreakOverTimeMode();
            _blackboard.PlayerMediator.DestructiblePlatformBreaker.SetEnabled(true);
            
            _blackboard.PlayerMediator.PlayerView.PlayEnterMovingWithoutAnchorAnimation();



            if (_blackboard.CameFromState == PlayerStates.DashingDroppingAnchor)
            {
                _enterPullingCooldown.SetDuration(_blackboard.PlayerStatesConfig.RollIntoPullCooldown);
                _enterDashCooldown.SetDuration(_blackboard.PlayerStatesConfig.RollIntoDashCooldown);
            }
            else if (_blackboard.CameFromState == PlayerStates.ThrowingAnchor)
            {
                _enterPullingCooldown.SetDuration(_blackboard.PlayerStatesConfig.ThrowIntoPullCooldown);
                _enterDashCooldown.SetDuration(0);
            }
            else
            {
                _enterPullingCooldown.SetDuration(0);
                _enterDashCooldown.SetDuration(0);
            }
            _enterPullingCooldown.Clear();
            _enterDashCooldown.Clear();
        }

        public override void Exit()
        {
            _blackboard.PlayerMediator.DestructiblePlatformBreaker.SetEnabled(false);
            _blackboard.PlayerStatesConfig.OnSpeedValueChanged -= UpdateMovementSpeed;
        }

        public override bool Update(float deltaTime)
        {
            _enterPullingCooldown.Update(deltaTime);
            _enterDashCooldown.Update(deltaTime);
            
            
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
            bool pullInput = _blackboard.MovesetInputsController.Pull_Pressed();
            
            if (!_enterPullingCooldown.HasFinished())
            {
                if (pullInput)
                {
                    _blackboard.QueuedAnchorPull = true;
                }
                return false;
            }
            
            if (_blackboard.QueuedAnchorPull && _blackboard.AnchorMediator.IsRestingOnFloor())
            {
                _blackboard.QueuedAnchorPull = false;
                return true;
            }
            
            return pullInput;
        }

        private bool PlayerCanDashTowardsAnchor()
        {
            bool dashInput = _blackboard.MovesetInputsController.Dash_Pressed();
            if (!_enterDashCooldown.HasFinished())
            {
                if (dashInput)
                {
                    _blackboard.QueuedDashTowardsAnchor = true;
                }
                return false;
            }
            
            if (_blackboard.QueuedDashTowardsAnchor)
            {
                _blackboard.QueuedDashTowardsAnchor = false;
                return true;
            }

            return dashInput;
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
            float maxMovementSpeed = _blackboard.PlayerStatesConfig.WithoutAnchorMoveSpeed;
            _blackboard.PlayerMediator.SetMaxMovementSpeed(maxMovementSpeed);
            _blackboard.PlayerMovementChecker.MaxMovementSpeed = maxMovementSpeed;
        }
    }
}