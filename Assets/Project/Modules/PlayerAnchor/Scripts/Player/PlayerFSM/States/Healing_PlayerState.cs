using System;
using Cysharp.Threading.Tasks;
using Popeye.Timers;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class Healing_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;
        private PlayerStates _healEndNextState;

        private Timer _healActionTimer;
        private bool _wasInterrupted;
        
        public Healing_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
            _healEndNextState = PlayerStates.None;
        }
        
        
        protected override void DoEnter()
        {
            _healEndNextState = _blackboard.cameFromState;
            _wasInterrupted = false;

            float durationToComplete = _blackboard.PlayerStatesConfig.HealingDuration;
            _healActionTimer = new Timer(durationToComplete);
            
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.HealingMoveSpeed);
            _blackboard.PlayerMediator.OnHealStart(durationToComplete);
        }

        public override void Exit()
        {
            if (_wasInterrupted)
            {
                _blackboard.PlayerMediator.OnHealInterrupted();
            }
        }

        public override bool Update(float deltaTime)
        {
            if (_blackboard.MovesetInputsController.Heal_HeldPressed())
            {
                _healActionTimer.Update(deltaTime);
                if (_healActionTimer.HasFinished())
                {
                    _blackboard.PlayerMediator.PlayerHealing.UseHeal();
                    NextState = _healEndNextState;
                    return true;
                }
            }
            else if (_blackboard.MovesetInputsController.Heal_Released())
            {
                _wasInterrupted = true;
                NextState = _healEndNextState;
                return true;
            }

            return false;
        }

        

    }
}