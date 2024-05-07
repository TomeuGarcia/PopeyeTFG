using Popeye.Modules.PlayerAnchor.Player.PlayerFocus;
using Popeye.Modules.PlayerController.Inputs;
using Popeye.Timers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class EnteringSpecialAttack_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;
        private readonly IPlayerSpecialAttackController _specialAttackController;
        private readonly SpecialAttackInput _specialAttackInput;
        private readonly PlayerStates _performState;
        private Timer _specialAttackTimer;
        private bool _wasInterrupted;
        
        public EnteringSpecialAttack_PlayerState(PlayerStatesBlackboard blackboard, 
            IPlayerSpecialAttackController specialAttackController,
            SpecialAttackInput specialAttackInput,
            PlayerStates performState)
        {
            _blackboard = blackboard;
            _specialAttackController = specialAttackController;
            _specialAttackInput = specialAttackInput;
            _performState = performState;
        }
        
        
        protected override void DoEnter()
        {
            _wasInterrupted = false;

            float durationToComplete = _specialAttackController.PreparationDuration;
            
            _specialAttackTimer = new Timer(durationToComplete);
            
            _blackboard.PlayerMediator.SetMaxMovementSpeed(_blackboard.PlayerStatesConfig.EnteringSpecialAttackMoveSpeed);
            _blackboard.PlayerMediator.OnSpecialAttackPreparationStart(_specialAttackController, durationToComplete);
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
            if (_specialAttackInput.IsBeingHeldPressed())
            {
                _specialAttackTimer.Update(deltaTime);
                if (_specialAttackTimer.HasFinished())
                {
                    _blackboard.PlayerMediator.OnSpecialAttackPerformed();

                    NextState = _performState;
                    return true;
                }
            }
            else if (_specialAttackInput.WasReleased())
            {
                _wasInterrupted = true;
                NextState = _blackboard.CameFromState;
                return true;
            }

            return false;
        }
    }
}