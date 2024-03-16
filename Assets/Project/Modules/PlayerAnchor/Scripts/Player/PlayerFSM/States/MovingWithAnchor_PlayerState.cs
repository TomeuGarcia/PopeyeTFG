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
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.WithAnchorMoveSpeed);
            
            _lateAnchorThrowTimer.SetDuration(_blackboard.PlayerStatesConfig.AnchorLateThrowTime);
            _lateAnchorThrowTimer.Clear();
            
            _anchorHeldAimTimer.SetDuration(_blackboard.PlayerStatesConfig.AnchorAimHeldWaitTime);
            _anchorHeldAimTimer.Clear();
            
            _blackboard.PlayerMediator.DestructiblePlatformBreaker.SetBreakOverTimeMode();
            _blackboard.PlayerMediator.DestructiblePlatformBreaker.SetEnabled(true);

            _blackboard.PlayerMediator.PlayerView.PlayEnterMovingWithAnchorAnimation();

            _blackboard.PlayerMovementChecker.MaxMovementSpeed = _blackboard.PlayerStatesConfig.WithAnchorMoveSpeed;
        }

        public override void Exit()
        {
            _blackboard.PlayerMediator.DestructiblePlatformBreaker.SetEnabled(false);
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
            
            return false;
        }


        private bool PlayerCanAimAnchor()
        {
            if (_blackboard.queuedAnchorAim)
            {
                _blackboard.queuedAnchorAim = false;
                return true;
            }
            
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
            return _blackboard.MovesetInputsController.Dash_Pressed();
        }

        private bool LateAnchorThrow(float deltaTime)
        {
            if (_lateAnchorThrowTimer.HasFinished())
            {
                return false;
            }
            _lateAnchorThrowTimer.Update(deltaTime);
            
            return _blackboard.MovesetInputsController.Throw_Pressed();
        }
        
        private bool PlayerTriesToSpinAnchor()
        {
            return _blackboard.MovesetInputsController.SpinAttack_Pressed(out _blackboard.spinAttackTowardsRight) && 
                   _blackboard.PlayerMediator.CanSpinAnchor();
        }

        private bool PlayerCanHeal(out bool hasHealsLeft)
        {
            hasHealsLeft = false;
            
            return _blackboard.MovesetInputsController.Heal_Pressed() && 
                   _blackboard.PlayerMediator.PlayerHealing.CanHeal(out hasHealsLeft);
        }
    }
}