using System.Collections.Generic;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class WithoutAnchor_PlayerState : APlayerState
    {
        private readonly PlayerStatesBlackboard _blackboard;
        private readonly WithoutAnchor_PlayerStateConfig _config;
        private APlayerState _currentState;
        private Dictionary<PlayerStates, APlayerState> _states;


        public WithoutAnchor_PlayerState(PlayerStatesBlackboard blackboard, WithoutAnchor_PlayerStateConfig config)
        {
            _blackboard = blackboard;
            _config = config;
        }

        public void Setup(MovingWithoutAnchor_PlayerState movingWithoutAnchor,
                            PickingUpAnchor_PlayerState pickingUpAnchor, 
                            DashingTowardsAnchor_PlayerState dashingTowardsAnchor,
                            KickingAnchor_PlayerState kickingAnchor,
                            PullingAnchor_PlayerState pullingAnchor,
                            SpinningAnchor_PlayerState spinningAnchor)
        {
            _states = new Dictionary<PlayerStates, APlayerState>()
            {
                { PlayerStates.MovingWithoutAnchor , movingWithoutAnchor },
                { PlayerStates.PickingUpAnchor , pickingUpAnchor },
                { PlayerStates.DashingTowardsAnchor , dashingTowardsAnchor },
                { PlayerStates.KickingAnchor , kickingAnchor },
                { PlayerStates.PullingAnchor , pullingAnchor },
                { PlayerStates.SpinningAnchor , spinningAnchor }
            };

        }
        
        
        protected override void DoEnter()
        {
            _currentState = _states[PlayerStates.MovingWithoutAnchor];
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