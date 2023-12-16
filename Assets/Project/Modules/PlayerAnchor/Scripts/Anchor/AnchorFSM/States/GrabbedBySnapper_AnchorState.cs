namespace Project.Modules.PlayerAnchor.Anchor.AnchorStates.States
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
            _blackboard.AnchorPhysics.EnableTension();
            _blackboard.AnchorChain.EnableTension();
        }

        public void Exit()
        {
            _blackboard.TransformMotion.Unparent();
        }
        
        
    }
}