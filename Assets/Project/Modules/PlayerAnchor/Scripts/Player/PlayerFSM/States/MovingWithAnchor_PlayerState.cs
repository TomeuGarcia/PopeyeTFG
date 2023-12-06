using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;

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

        private bool PlayerCanHeal()
        {
            return _blackboard.MovesetInputsController.Heal_Pressed() && _blackboard.PlayerMediator.CanHeal();
        }
    }
}