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
            _blackboard.AnchorPhysics.EnableAllPhysics();
            _blackboard.TransformMotion.Unparent();
            
            _blackboard.AnchorChain.Show();
            //_blackboard.AnchorChain.DisableTension();
        }

        public void Exit()
        {
            //_blackboard.AnchorChain.EnableTension();
            
            _blackboard.AnchorChain.SetFailedThrow(false);
        }
    }
}