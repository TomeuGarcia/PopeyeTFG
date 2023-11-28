using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class MovingWithAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;
        private readonly MovingWithAnchor_PlayerStateConfig _config;

        public MovingWithAnchor_PlayerState(PlayerStatesBlackboard blackboard, MovingWithAnchor_PlayerStateConfig config)
        {
            _blackboard = blackboard;
            _config = config;
        }
        
        protected override void DoEnter()
        {
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_config.MovementSpeed);
        }

        public override void Exit()
        {
            
        }

        public override bool Update(float deltaTime)
        {
            if (_blackboard.MovesetInputsController.Throw_Pressed())
            {
                NextState = PlayerStates.AimingThrowAnchor;
                return true;
            }

            return false;
        }
    }
}