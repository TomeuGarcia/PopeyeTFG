using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class SpinningAnchor_PlayerState : APlayerState
    {
        private readonly SpinningAnchor_PlayerStateConfig _config;

        public SpinningAnchor_PlayerState(SpinningAnchor_PlayerStateConfig config)
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