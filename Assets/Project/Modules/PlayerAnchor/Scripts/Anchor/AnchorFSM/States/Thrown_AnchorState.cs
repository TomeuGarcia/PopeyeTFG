namespace Project.Modules.PlayerAnchor.Anchor.AnchorStates.States
{
    public class Thrown_AnchorState : IAnchorState
    {
        private readonly AnchorStatesBlackboard _blackboard;

        public Thrown_AnchorState(AnchorStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        public void Enter()
        {
            _blackboard.AnchorPhysics.EnablePhysics();
            _blackboard.AnchorMotion.Unparent();
            
            _blackboard.AnchorChain.Show();
        }

        public void Exit()
        {
            
        }
    }
}