using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class MovingWithAnchor_PlayerState : APlayerState
    {
        private readonly MovingWithAnchor_PlayerStateConfig _config;

        public MovingWithAnchor_PlayerState(MovingWithAnchor_PlayerStateConfig config)
        {
            _config = config;
        }
        
        protected override void DoEnter()
        {
            throw new System.NotImplementedException();
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }

        public override bool Update(float deltaTime)
        {
            throw new System.NotImplementedException();
        }
    }
}