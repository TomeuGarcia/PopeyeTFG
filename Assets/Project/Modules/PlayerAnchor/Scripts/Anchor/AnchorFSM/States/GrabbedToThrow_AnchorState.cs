using DG.Tweening;

namespace Project.Modules.PlayerAnchor.Anchor.AnchorStates.States
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
            _blackboard.AnchorMotion.ParentAndReset(_blackboard.AnchorGrabToThrowHolder, 0.2f, Ease.InOutSine);
        }

        public void Exit()
        {
            
        }

    }
}