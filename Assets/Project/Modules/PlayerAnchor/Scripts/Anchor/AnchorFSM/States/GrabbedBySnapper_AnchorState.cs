
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor.AnchorStates.States
{
    public class GrabbedBySnapper_AnchorState : IAnchorState
    {
        private readonly AnchorStatesBlackboard _blackboard;

        public GrabbedBySnapper_AnchorState(AnchorStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        
        public void Enter()
        {
            _blackboard.AnchorPhysics.EnableCollision();
            _blackboard.AnchorChain.EnableTension();
            
            _blackboard.AnchorMediator.CurrentTrajectorySnapTarget.OnStartBeingUsed(_blackboard.PlayerPositionTransform);
        }

        public void Exit()
        {
            _blackboard.TransformMotion.Unparent();
            _blackboard.AnchorMediator.CurrentTrajectorySnapTarget.OnFinishBeingUsed();
            _blackboard.AnchorMediator.ResetCurrentTrajectorySnapTarget();
        }
        
        
    }
}