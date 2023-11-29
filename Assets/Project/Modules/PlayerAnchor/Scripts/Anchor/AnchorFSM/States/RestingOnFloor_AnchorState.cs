namespace Project.Modules.PlayerAnchor.Anchor.AnchorStates.States
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
            _blackboard.AnchorPhysics.SetImmovable();
        }

        public void Exit()
        {
            _blackboard.AnchorPhysics.SetMovable();
        }
    }
}