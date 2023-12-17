using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class MovingWithAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;
        
        public MovingWithAnchor_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        protected override void DoEnter()
        {
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.WithAnchorMoveSpeed);
        }

        public override void Exit()
        {
            
        }

        public override bool Update(float deltaTime)
        {
            if (PlayerCanThrowAnchor())
            {
                NextState = PlayerStates.AimingThrowAnchor;
                return true;
            }

            if (PlayerCanDash())
            {
                NextState = PlayerStates.DashingDroppingAnchor;
                return true;
            }

            if (PlayerTriesToSpinAnchor())
            {
                NextState = PlayerStates.SpinningAnchor;
                return true;
            }
            
            if (PlayerCanHeal())
            {
                NextState = PlayerStates.Healing;
                return true;
            }
            
            return false;
        }


        private bool PlayerCanThrowAnchor()
        {
            return _blackboard.MovesetInputsController.Throw_Pressed();
        }

        private bool PlayerCanDash()
        {
            return _blackboard.MovesetInputsController.Dash_Pressed();
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