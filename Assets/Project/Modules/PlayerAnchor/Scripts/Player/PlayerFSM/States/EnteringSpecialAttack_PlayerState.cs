using Popeye.Timers;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class EnteringSpecialAttack_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;
        private PlayerStates _endNextState;
        private Timer _ragingActionTimer;
        private bool _wasInterrupted;
        
        public EnteringSpecialAttack_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
            _endNextState = PlayerStates.None;
        }
        
        
        protected override void DoEnter()
        {
            _endNextState = _blackboard.cameFromState;
            _wasInterrupted = false;

            float durationToComplete = _blackboard.PlayerStatesConfig.EnteringSpecialAttackDuration;
            _ragingActionTimer = new Timer(durationToComplete);
            
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.EnteringSpecialAttackMoveSpeed);
            _blackboard.PlayerMediator.OnSpecialAttackPreparationStart(durationToComplete);
        }

        public override void Exit()
        {
            if (_wasInterrupted)
            {
                _blackboard.PlayerMediator.OnSpecialAttackPreparationInterrupted();
            }
        }

        public override bool Update(float deltaTime)
        {
            if (_blackboard.MovesetInputsController.SpecialAttack_HeldPressed())
            {
                _ragingActionTimer.Update(deltaTime);
                if (_ragingActionTimer.HasFinished())
                {
                    _blackboard.PlayerMediator.OnSpecialAttackPerformed();
                    NextState = _endNextState;
                    return true;
                }
            }
            else if (_blackboard.MovesetInputsController.SpecialAttack_Released())
            {
                _wasInterrupted = true;
                NextState = _endNextState;
                return true;
            }

            return false;
        }
    }
}