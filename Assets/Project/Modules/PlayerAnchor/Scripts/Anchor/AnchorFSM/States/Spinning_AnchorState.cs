namespace Popeye.Modules.PlayerAnchor.Anchor.AnchorStates.States
{
    public class Spinning_AnchorState : IAnchorState
    {
        private readonly AnchorStatesBlackboard _blackboard;

        public Spinning_AnchorState(AnchorStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }

        public void Enter()
        {
            // old
            //_blackboard.TransformMotion.Unparent();
            
            _blackboard.AnchorChain.SetSpinningView();
        }

        public void Exit()
        {
            // old
            //_blackboard.AnchorPhysics.EnableCollision();
        }
    }
}