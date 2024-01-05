namespace Popeye.Modules.PlayerAnchor.Anchor.AnchorStates.States
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
            _blackboard.AnchorChain.EnableTension();
        }

        public void Exit()
        {
        }
    }
}