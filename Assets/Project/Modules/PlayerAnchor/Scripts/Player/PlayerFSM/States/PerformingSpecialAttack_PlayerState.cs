namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class PerformingSpecialAttack_PlayerState : APlayerState
    {
        public class TransitionExitData
        {
            public PlayerStates enterState;
        }
    
        private readonly PlayerStatesBlackboard _blackboard;
        private readonly TransitionExitData _exitData;

        public PerformingSpecialAttack_PlayerState(PlayerStatesBlackboard blackboard, TransitionExitData exitData)
        {
            _blackboard = blackboard;
            _exitData = exitData;
        }

        
        protected override void DoEnter()
        {
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.PerformingSpecialAttackMoveSpeed);
        }

        public override void Exit()
        {
            if (!_blackboard.PlayerMediator.SpecialAttackHasFinished())
            {
                _blackboard.PlayerMediator.ForceStopSpecialAttack();
            }
        }

        public override bool Update(float deltaTime)
        {
            if (_blackboard.PlayerMediator.SpecialAttackHasFinished())
            {
                if (PopeyePlayer.debugIsSpinning)
                {
                    NextState = PlayerStates.MovingWithoutAnchor;
                }
                else
                {
                    NextState = _exitData.enterState;
                }
                
                return true;
            }

            return false;
        }
    }
}