using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using Popeye.Timers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class MovingWithAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;
        private readonly Timer _lateAnchorThrowTimer;
        private readonly Timer _anchorHeldAimTimer;
        
        
        public MovingWithAnchor_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;

            _lateAnchorThrowTimer = new Timer(_blackboard.PlayerStatesConfig.AnchorLateThrowTime);
            _anchorHeldAimTimer = new Timer(_blackboard.PlayerStatesConfig.AnchorAimHeldWaitTime);
        }
        
        protected override void DoEnter()
        {
            _blackboard.PlayerStatesConfig.OnSpeedValueChanged += UpdateMovementSpeed;
            UpdateMovementSpeed();
            
            _lateAnchorThrowTimer.SetDuration(_blackboard.PlayerStatesConfig.AnchorLateThrowTime);
            _lateAnchorThrowTimer.Clear();
            
            _anchorHeldAimTimer.SetDuration(_blackboard.PlayerStatesConfig.AnchorAimHeldWaitTime);
            _anchorHeldAimTimer.Clear();
            
            _blackboard.PlayerMediator.DestructiblePlatformBreaker.SetBreakOverTimeMode();

            _blackboard.PlayerMediator.PlayerView.PlayEnterMovingWithAnchorAnimation();
        }

        public override void Exit()
        {
            _blackboard.PlayerStatesConfig.OnSpeedValueChanged -= UpdateMovementSpeed;
        }

        public override bool Update(float deltaTime)
        {
            _blackboard.PlayerMediator.UpdateSafeGroundChecking(deltaTime, out bool playerIsOnVoid, out bool anchorIsOnVoid);
            if (playerIsOnVoid)
            {
                _blackboard.PlayerMediator.OnPlayerFellOnVoid();
                NextState = PlayerStates.FallingOnVoid;
                return true;
            }
            
            if (PlayerCanAimAnchor())
            {
                NextState = PlayerStates.AimingThrowAnchor;
                return true;
            }
            
            
            if (PlayerHoldingAim(deltaTime))
            {
                NextState = PlayerStates.AimingThrowAnchor;
                return true;
            }
            
            
            if (PlayerCanDash())
            {
                NextState = PlayerStates.DashingDroppingAnchor;
                return true;
            }
            
            /* // This is very bugged
            if (LateAnchorThrow(deltaTime))
            {
                NextState = PlayerStates.ThrowingAnchor;
                return true;
            }
            */
            
            /*
            if (PlayerTriesToSpinAnchor())
            {
                NextState = PlayerStates.SpinningAnchor;
                return true;
            }
            */
            
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

            return false;
        }


        private bool PlayerCanAimAnchor()
        {
            return _blackboard.MovesetInputsController.Aim_Pressed();
        }

        private bool PlayerHoldingAim(float deltaTime)
        {
            if (_anchorHeldAimTimer.HasFinished())
            {
                return _blackboard.MovesetInputsController.Aim_HeldPressed();
            }
            
            _anchorHeldAimTimer.Update(deltaTime);
            return false;
        }
        

        private bool PlayerCanDash()
        {
            return _blackboard.MovesetInputsController.DashDroppingAnchor_Pressed();
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

        private void UpdateMovementSpeed()
        {
            float maxMovementSpeed = _blackboard.PlayerStatesConfig.WithAnchorMoveSpeed;
            _blackboard.PlayerMediator.SetMaxMovementSpeed(maxMovementSpeed);
            _blackboard.PlayerMovementChecker.MaxMovementSpeed = maxMovementSpeed;
        }
    }
}