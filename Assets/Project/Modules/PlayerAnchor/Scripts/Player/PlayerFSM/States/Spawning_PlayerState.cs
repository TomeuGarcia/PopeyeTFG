using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class Spawning_PlayerState : APlayerState
    {
        private readonly Spawning_PlayerStateConfig _config;

        public Spawning_PlayerState(Spawning_PlayerStateConfig config)
        {
            _config = config;
        }
        
        
        
        protected override void DoEnter()
        {
            
        }

        public override void Exit()
        {
            
        }

        public override bool Update(float deltaTime)
        {
            NextState = PlayerStates.WithAnchor;
            return true;
        }
    }
}