using DG.Tweening;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor.AnchorStates.States
{
    public class GrabbedToThrow_AnchorState : IAnchorState
    {
        private readonly AnchorStatesBlackboard _blackboard;

        public GrabbedToThrow_AnchorState(AnchorStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        public void Enter()
        {
            _blackboard.TransformMotion.ParentAndUpdate(_blackboard.AnchorGrabToThrowHolder,
                Vector3.zero, _blackboard.AnchorMotionConfig.GrabbedToThrowAnchorRotation,
                0.2f, Ease.InOutSine);
            
            _blackboard.AnchorPhysics.DisableCollision();
        }
        

        public void Exit()
        {
            
        }

    }
}