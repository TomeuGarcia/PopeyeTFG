namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class TiredPickingUpAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;

        public TiredPickingUpAnchor_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        protected override void DoEnter()
        {
            _blackboard.PlayerMediator.PlayerView.PlayPickUpAnchorAnimation();
            
            StartPickingUpAnchor();
        }

        public override void Exit()
        {
            
        }

        public override bool Update(float deltaTime)
        {
            NextState = PlayerStates.Tired;
            return true;
        }

        private void StartPickingUpAnchor()
        {
            _blackboard.PlayerMediator.PickUpAnchor();
        }
    }
}