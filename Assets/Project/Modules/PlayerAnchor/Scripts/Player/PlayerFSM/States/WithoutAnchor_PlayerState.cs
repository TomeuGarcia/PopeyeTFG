using System.Collections.Generic;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class WithoutAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;
        private APlayerState _currentState;
        private Dictionary<PlayerStates, APlayerState> _states;


        public WithoutAnchor_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }

        public void Setup(MovingWithoutAnchor_PlayerState movingWithoutAnchor,
                            PickingUpAnchor_PlayerState pickingUpAnchor, 
                            DashingTowardsAnchor_PlayerState dashingTowardsAnchor,
                            KickingAnchor_PlayerState kickingAnchor,
                            PullingAnchor_PlayerState pullingAnchor,
                            SpinningAnchor_PlayerState spinningAnchor,
                            Tired_PlayerState tired,
                            Healing_PlayerState healing)
        {
            _states = new Dictionary<PlayerStates, APlayerState>()
            {
                { PlayerStates.MovingWithoutAnchor , movingWithoutAnchor },
                { PlayerStates.PickingUpAnchor , pickingUpAnchor },
                { PlayerStates.DashingTowardsAnchor , dashingTowardsAnchor },
                { PlayerStates.KickingAnchor , kickingAnchor },
                { PlayerStates.PullingAnchor , pullingAnchor },
                { PlayerStates.SpinningAnchor , spinningAnchor },
                { PlayerStates.Tired , tired },
                { PlayerStates.Healing , healing }
            };
        }
        
        public override bool HasSubState(PlayerStates state)
        {
            return _states.ContainsKey(state);
        }
        
        protected override void DoEnter()
        {
            PlayerStates entryState = PlayerStates.MovingWithoutAnchor;
            if (_blackboard.queuedOverwriteState != PlayerStates.None &&
                _states.ContainsKey(_blackboard.queuedOverwriteState))
            {
                entryState = _blackboard.queuedOverwriteState;
            }

            _currentState = _states[entryState];
            _currentState.Enter();
        }

        public override void Exit()
        {
            
        }

        public override bool Update(float deltaTime)
        {
            if (_currentState.Update(deltaTime))
            {
                _currentState.Exit();
                
                if (_currentState.NextState == PlayerStates.WithAnchor)
                {
                    NextState = PlayerStates.WithAnchor;
                    return true;
                }
                
                _currentState = _states[_currentState.NextState];
                _currentState.Enter();
            }

            return false;
        }
    }
}