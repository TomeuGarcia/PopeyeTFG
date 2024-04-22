using Popeye.Timers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class EnteringSpecialAttack_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;
        private readonly PerformingSpecialAttack_PlayerState.TransitionExitData _exitData;
        private Timer _ragingActionTimer;
        private bool _wasInterrupted;
        
        public EnteringSpecialAttack_PlayerState(PlayerStatesBlackboard blackboard, 
            PerformingSpecialAttack_PlayerState.TransitionExitData exitData)
        {
            _blackboard = blackboard;
            _exitData = exitData;
            _exitData.enterState = PlayerStates.None;
        }
        
        
        protected override void DoEnter()
        {
            _exitData.enterState = _blackboard.CameFromState;
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
                    //NextState = _endNextState;
                    //return true;
                    
                    NextState = PlayerStates.PerformingSpecialAttack;
                    return true;
                }
            }
            else if (_blackboard.MovesetInputsController.SpecialAttack_Released())
            {
                _wasInterrupted = true;
                NextState = _exitData.enterState;
                return true;
            }

            return false;
        }
    }
}