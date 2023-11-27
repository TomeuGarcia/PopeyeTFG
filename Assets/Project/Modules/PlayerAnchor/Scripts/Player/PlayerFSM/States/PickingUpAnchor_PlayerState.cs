using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class PickingUpAnchor_PlayerState : APlayerState
    {
        private readonly PickingUpAnchor_PlayerStateConfig _config;

        public PickingUpAnchor_PlayerState(PickingUpAnchor_PlayerStateConfig config)
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