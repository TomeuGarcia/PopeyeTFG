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
        }

        public override void Exit()
        {
            
        }

        public override bool Update(float deltaTime)
        {
            if (PlayerCanAimAnchor() || PlayerHoldingAim(deltaTime))
            {
                NextState = PlayerStates.AimingThrowAnchor;
                return true;
            }

            if (PlayerCanDash())
            {
                NextState = PlayerStates.DashingDroppingAnchor;
                return true;
            }
            
            if (LateAnchorThrow(deltaTime))
            {
                NextState = PlayerStates.ThrowingAnchor;
                return true;
            }
            
            /*
            if (PlayerTriesToSpinAnchor())
            {
                NextState = PlayerStates.SpinningAnchor;
                return true;
            }
            */
            
            if (PlayerCanHeal())
            {
                NextState = PlayerStates.Healing;
                return true;
            }
            
            return false;
        }


        private bool PlayerCanAimAnchor()
        {
            if (_blackboard.queuedAnchorThrow)
            {
                _blackboard.queuedAnchorThrow = false;
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

        private bool PlayerCanHeal()
        {
            return _blackboard.MovesetInputsController.Heal_Pressed() && _blackboard.PlayerMediator.CanHeal();
        }
    }
}