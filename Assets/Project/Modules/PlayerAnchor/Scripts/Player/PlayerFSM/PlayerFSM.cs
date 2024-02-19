using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class PlayerFSM
    {
        private APlayerState _currentState;
        private PlayerStates _currentStateType;
        private Dictionary<PlayerStates, APlayerState> _states;

        public PlayerStatesBlackboard Blackboard { get; private set; }

        public void Configure(PlayerStatesBlackboard blackboard, IPlayerStatesCreator playerStatesCreator)
        {
            Blackboard = blackboard;
            
            _states = playerStatesCreator.CreateStatesDictionary(blackboard);
            _currentStateType = playerStatesCreator.StartState;
            
            _currentState = _states[_currentStateType];
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
            Blackboard.cameFromState = _currentStateType;
            
            _currentState.Exit();
            _currentState = _states[nextState];
            _currentState.Enter();

            _currentStateType = nextState;
        }

        public void OverwriteState(PlayerStates newState)
        {
            if (_states.ContainsKey(newState))
            {
                TransitionToNextState(newState);
            }
        }
        
    }
}


