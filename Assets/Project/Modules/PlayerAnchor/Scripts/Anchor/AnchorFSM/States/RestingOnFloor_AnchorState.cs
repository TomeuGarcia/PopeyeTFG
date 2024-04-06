using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor.AnchorStates.States
{
    public class RestingOnFloor_AnchorState : IAnchorState
    {
        private readonly AnchorStatesBlackboard _blackboard;

        public RestingOnFloor_AnchorState(AnchorStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        
        public void Enter()
        {
            _blackboard.AnchorChain.EnableTension();
            
            _blackboard.AnchorMediator.DestructiblePlatformBreaker.SetBreakOverTimeMode();
            _blackboard.AnchorMediator.DestructiblePlatformBreaker.SetEnabled(true);
        }

        public void Exit()
        {
            _blackboard.AnchorMediator.DestructiblePlatformBreaker.SetEnabled(false);
        }
    }
}