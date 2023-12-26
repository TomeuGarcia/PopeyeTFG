namespace Project.Modules.PlayerAnchor.Anchor.AnchorStates.States
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
            _blackboard.TransformMotion.Unparent();
        }

        public void Exit()
        {
            _blackboard.AnchorPhysics.EnableTension();
        }
    }
}