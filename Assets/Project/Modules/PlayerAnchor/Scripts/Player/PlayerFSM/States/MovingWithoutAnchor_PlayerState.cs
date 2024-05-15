
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using Popeye.Timers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class MovingWithoutAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;

        private readonly Timer _enterPullingCooldown;
        private readonly Timer _enterDashCooldown;
        

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

            if (PlayerCanHeal(out bool hasHealsLeft))
            {
                NextState = PlayerStates.Healing;
                return true;
            }
            if (PlayerCanDoAnchorSpinAttack())
            {
                NextState = PlayerStates.EnteringAnchorSpinAttack;
                return true;
            }
            if (PlayerCanDoChainSpikesAttack())
            {
                NextState = PlayerStates.EnteringChainSpikesAttack;
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
                return false;
            }
            
            if (pullInput && _blackboard.AnchorMediator.IsRestingOnFloor())
            {
                return true;
            }
            
            return pullInput;
        }

        private bool PlayerCanDashTowardsAnchor()
        {
            bool dashInput = _blackboard.MovesetInputsController.DashTowardsAnchor_Pressed();
            if (!_enterDashCooldown.HasFinished())
            {
                return false;
            }

            return dashInput;
        }

        private bool PlayerCanHeal(out bool hasHealsLeft)
        {
            hasHealsLeft = false;
            
            return _blackboard.MovesetInputsController.Heal_Pressed() && 
                   _blackboard.PlayerMediator.PlayerHealing.CanHeal(out hasHealsLeft);
        }
        
        private bool PlayerCanDoAnchorSpinAttack()
        {
            return _blackboard.MovesetInputsController.SpinAttack_Pressed() && 
                   _blackboard.AnchorSpinAttackController.CanDoSpecialAttack();
        }
        
        private bool PlayerCanDoChainSpikesAttack()
        {
            return _blackboard.MovesetInputsController.SpikesAttack_Pressed() && 
                   _blackboard.ChainSpikesAttackController.CanDoSpecialAttack();
        }


        private void UpdateMovementSpeed()
        {
            float maxMovementSpeed = _blackboard.PlayerStatesConfig.WithoutAnchorMoveSpeed;
            _blackboard.PlayerMediator.SetMaxMovementSpeed(maxMovementSpeed);
            _blackboard.PlayerMovementChecker.MaxMovementSpeed = maxMovementSpeed;
        }
    }
}