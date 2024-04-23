using System.Collections.Generic;

namespace Popeye.Modules.PlayerAnchor.Anchor.AnchorStates
{
    public class TutorialAnchorStatesCreator : IAnchorStatesCreator
    {
        public AnchorStates StartState => AnchorStates.RestingOnFloor;
        
        public Dictionary<AnchorStates, IAnchorState> CreateStatesDictionary(AnchorStatesBlackboard blackboard)
        {
            DefaultAnchorStatesCreator defaultAnchorStatesCreator = new DefaultAnchorStatesCreator();

            return defaultAnchorStatesCreator.CreateStatesDictionary(blackboard);
        }
    }
}