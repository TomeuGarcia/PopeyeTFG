using Popeye.Timers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class EnteringSpecialAttack_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;
        private readonly PerformingSpecialAttack_PlayerState.TransitionExitData _exitData;
        private Timer _specialAttackTimer;
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

            float durationToComplete = PopeyePlayer.debugIsSpinning 
                ? 0.5f
                : _blackboard.PlayerStatesConfig.EnteringSpecialAttackDuration;
            
            _specialAttackTimer = new Timer(durationToComplete);
            
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
                _specialAttackTimer.Update(deltaTime);
                if (_specialAttackTimer.HasFinished())
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