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
            _blackboard.AnchorPhysics.UseGravity(false);
            _blackboard.AnchorChain.EnableTension();
        }

        public void Exit()
        {
            _blackboard.AnchorPhysics.UseGravity(true);
            _blackboard.TransformMotion.Unparent();
        }
        
        
    }
}