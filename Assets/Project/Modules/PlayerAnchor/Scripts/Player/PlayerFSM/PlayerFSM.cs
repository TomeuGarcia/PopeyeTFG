using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class PlayerFSM
    {
        private APlayerState _currentState;
        private Dictionary<PlayerStates, APlayerState> _states;

        public PlayerStatesBlackboard Blackboard { get; private set; }

        public void Setup(PlayerStatesBlackboard blackboard)
        {
            Blackboard = blackboard;
            
            Spawning_PlayerState spawningState 
                = new Spawning_PlayerState(blackboard);
            Dead_PlayerState deadState 
                = new Dead_PlayerState(blackboard);
            WithAnchor_PlayerState withAnchorState 
                = new WithAnchor_PlayerState(blackboard);
            WithoutAnchor_PlayerState withoutAnchorState 
                = new WithoutAnchor_PlayerState(blackboard);
            
            MovingWithAnchor_PlayerState movingWithAnchor 
                = new MovingWithAnchor_PlayerState(blackboard);
            AimingThrowAnchor_PlayerState aimingThrowAnchor 
                = new AimingThrowAnchor_PlayerState(blackboard);
            ThrowingAnchor_PlayerState throwingAnchor 
                = new ThrowingAnchor_PlayerState(blackboard);
            DashingDroppingAnchor_PlayerState dashingDroppingAnchor
                = new DashingDroppingAnchor_PlayerState(blackboard);

            MovingWithoutAnchor_PlayerState movingWithoutAnchor 
                = new MovingWithoutAnchor_PlayerState(blackboard);
            PickingUpAnchor_PlayerState pickingUpAnchor 
                = new PickingUpAnchor_PlayerState(blackboard);
            DashingTowardsAnchor_PlayerState dashingTowardsAnchor 
                = new DashingTowardsAnchor_PlayerState(blackboard);
            KickingAnchor_PlayerState kickingAnchor 
                = new KickingAnchor_PlayerState(blackboard);
            PullingAnchor_PlayerState pullingAnchor 
                = new PullingAnchor_PlayerState(blackboard);
            SpinningAnchor_PlayerState spinningAnchor 
                = new SpinningAnchor_PlayerState(blackboard);
            Tired_PlayerState tired
                = new Tired_PlayerState(blackboard);

            Healing_PlayerState healingWithAnchor
                = new Healing_PlayerState(blackboard, PlayerStates.MovingWithAnchor);
            Healing_PlayerState healingWithoutAnchor
                = new Healing_PlayerState(blackboard, PlayerStates.MovingWithoutAnchor);
            
            
            withAnchorState.Setup(movingWithAnchor, aimingThrowAnchor, throwingAnchor, healingWithAnchor, kickingAnchor,
                dashingDroppingAnchor);
            withoutAnchorState.Setup(movingWithoutAnchor, pickingUpAnchor, dashingTowardsAnchor, kickingAnchor, 
                pullingAnchor, spinningAnchor, tired, healingWithoutAnchor);

            _states = new Dictionary<PlayerStates, APlayerState>()
            {
                { PlayerStates.Spawning , spawningState },
                { PlayerStates.Dead , deadState },
                { PlayerStates.WithAnchor , withAnchorState },
                { PlayerStates.WithoutAnchor , withoutAnchorState }
            };
            
            
            
            _currentState = _states[PlayerStates.Spawning];
            _currentState.Enter();
        }

        public void Update(float deltaTime)
        {
            if (_currentState.Update(deltaTime))
            {
                TransitionToNextState(_currentState.NextState);
            }
        }

        private void TransitionToNextState(PlayerStates nextState)
        {
            _currentState.Exit();
            _currentState = _states[nextState];
            _currentState.Enter();
        }

        public void OverwriteState(PlayerStates newState)
        {
            Blackboard.queuedOverwriteState = newState;
            
            if (_states.ContainsKey(newState))
            {
                TransitionToNextState(newState);
            }
            else
            {
                foreach (var state in _states)
                {
                    if (state.Value.HasSubState(newState))
                    {
                        TransitionToNextState(state.Key);
                        break;
                    }
                }
            }
            
            Blackboard.queuedOverwriteState = PlayerStates.None;
        }
        
    }
}


