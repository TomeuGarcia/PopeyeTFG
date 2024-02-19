using System.Collections.Generic;

namespace Popeye.Modules.PlayerAnchor.Anchor.AnchorStates
{
    public class AnchorFSM
    {
        private IAnchorState _currentState;
        private Dictionary<AnchorStates, IAnchorState> _states;

        public AnchorStates CurrentStateType { get; private set; }

        public void Configure(AnchorStatesBlackboard blackboard, IAnchorStatesCreator anchorStatesCreator)
        {
            CurrentStateType = anchorStatesCreator.StartState;
            _states = anchorStatesCreator.CreateStatesDictionary(blackboard);

            _currentState = _states[CurrentStateType];
            _currentState.Enter();
        }

        public void OverwriteState(AnchorStates newState)
        {
            _currentState.Exit();
            _currentState = _states[newState];
            _currentState.Enter();

            CurrentStateType = newState;
        }

        public void Reset()
        {
            _currentState.Exit();
            CurrentStateType = AnchorStates.Carried;
            _currentState = _states[CurrentStateType];
            _currentState.Enter();
        }
    }
}