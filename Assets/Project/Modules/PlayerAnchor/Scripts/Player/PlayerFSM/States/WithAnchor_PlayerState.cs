using System.Collections.Generic;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class WithAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;
        private APlayerState _currentState;
        private Dictionary<PlayerStates, APlayerState> _states;


        public WithAnchor_PlayerState(PlayerStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }

        public void Setup(MovingWithAnchor_PlayerState movingWithAnchor,
                            AimingThrowAnchor_PlayerState aimingThrowAnchor,
                            ThrowingAnchor_PlayerState throwingAnchor)
        {
            _states = new Dictionary<PlayerStates, APlayerState>()
            {
                { PlayerStates.MovingWithAnchor , movingWithAnchor },
                { PlayerStates.AimingThrowAnchor , aimingThrowAnchor },
                { PlayerStates.ThrowingAnchor , throwingAnchor }
            };

        }
        
        
        protected override void DoEnter()
        {
            _currentState = _states[PlayerStates.MovingWithAnchor];
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
                
                if (_currentState.NextState == PlayerStates.WithoutAnchor)
                {
                    NextState = PlayerStates.WithoutAnchor;
                    return true;
                }
                
                _currentState = _states[_currentState.NextState];
                _currentState.Enter();
            }

            return false;
        }
    }
}