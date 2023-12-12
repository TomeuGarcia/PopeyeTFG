using System.Collections.Generic;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using Project.Modules.PlayerAnchor.Anchor.AnchorStates.States;

namespace Project.Modules.PlayerAnchor.Anchor.AnchorStates
{
    public class AnchorFSM
    {
        private IAnchorState _currentState;
        private Dictionary<AnchorStates, IAnchorState> _states;

        public AnchorStates CurrentStateType { get; private set; }

        public void Setup(AnchorStatesBlackboard blackboard)
        {
            Carried_AnchorState carried = new Carried_AnchorState(blackboard);
            GrabbedToThrow_AnchorState grabbedToThrow = new GrabbedToThrow_AnchorState(blackboard);
            Thrown_AnchorState thrown = new Thrown_AnchorState(blackboard);
            Pulled_AnchorState pulled = new Pulled_AnchorState(blackboard);
            RestingOnFloor_AnchorState restingOnFloor = new RestingOnFloor_AnchorState(blackboard);
            GrabbedBySnapper_AnchorState grabbedBySnapper = new GrabbedBySnapper_AnchorState(blackboard);
            Spinning_AnchorState spinning = new Spinning_AnchorState(blackboard);
            
            
            _states = new Dictionary<AnchorStates, IAnchorState>()
            {
                { AnchorStates.Carried , carried },
                { AnchorStates.GrabbedToThrow , grabbedToThrow },
                { AnchorStates.Thrown , thrown },
                { AnchorStates.Pulled , pulled },
                { AnchorStates.RestingOnFloor , restingOnFloor },
                { AnchorStates.GrabbedBySnapper , grabbedBySnapper },
                { AnchorStates.Spinning , spinning }
            };

            CurrentStateType = AnchorStates.Carried;
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