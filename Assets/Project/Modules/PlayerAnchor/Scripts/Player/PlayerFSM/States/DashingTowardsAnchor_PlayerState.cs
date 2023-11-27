using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class DashingTowardsAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;
        private readonly DashingTowardsAnchor_PlayerStateConfig _config;

        public DashingTowardsAnchor_PlayerState(PlayerStatesBlackboard blackboard, DashingTowardsAnchor_PlayerStateConfig config)
        {
            _blackboard = blackboard;
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