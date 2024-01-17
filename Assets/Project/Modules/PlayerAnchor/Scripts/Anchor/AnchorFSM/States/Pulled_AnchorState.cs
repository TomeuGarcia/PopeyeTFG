using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor.AnchorStates.States
{
    public class Pulled_AnchorState : IAnchorState
    {
        private readonly AnchorStatesBlackboard _blackboard;

        public Pulled_AnchorState(AnchorStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        public void Enter()
        {
            _blackboard.AnchorPhysics.EnableTension();
        }

        public void Exit()
        {
            
        }
    }
}