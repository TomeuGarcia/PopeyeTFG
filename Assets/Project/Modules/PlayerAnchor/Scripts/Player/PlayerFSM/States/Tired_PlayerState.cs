namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class Tired_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;

        
        public Tired_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }


        protected override void DoEnter()
        {
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.TiredMoveSpeed);
        }

        public override void Exit()
        {
            
        }

        public override bool Update(float deltaTime)
        {
            if (_blackboard.PlayerMediator.HasMaxStamina())
            {
                NextState = PlayerStates.MovingWithoutAnchor;
                return true;
            }

            return false;
        }
    }
}