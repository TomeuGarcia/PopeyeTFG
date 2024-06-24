using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class PlayerFSM
    {
        private APlayerState _currentState;
        public PlayerStates CurrentStateType { get; private set; }
        private Dictionary<PlayerStates, APlayerState> _states;
        private HashSet<PlayerStates> _nonOverwritableStates;

        public PlayerStatesBlackboard Blackboard { get; private set; }

        public void Configure(PlayerStatesBlackboard blackboard, IPlayerStatesCreator playerStatesCreator)
        {
            Blackboard = blackboard;
            
            _states = playerStatesCreator.CreateStatesDictionary(blackboard);
            _nonOverwritableStates = playerStatesCreator.CreateNonOverwritableStates();
            CurrentStateType = playerStatesCreator.StartState;
            
            _currentState = _states[CurrentStateType];
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
            Blackboard.CameFromState = CurrentStateType;
            CurrentStateType = nextState;
            
            _currentState.Exit();
            _currentState = _states[nextState];
            _currentState.Enter();
        }

        public void TryOverwriteState(PlayerStates newState)
        {
            if (_nonOverwritableStates.Contains(CurrentStateType))
            {
                return;
            }
        
            TransitionToNextState(newState);
        }
        
    }
}


