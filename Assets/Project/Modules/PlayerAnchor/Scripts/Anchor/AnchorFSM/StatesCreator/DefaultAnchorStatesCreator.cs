using System.Collections.Generic;
using Popeye.Modules.PlayerAnchor.Anchor.AnchorStates.States;

namespace Popeye.Modules.PlayerAnchor.Anchor.AnchorStates
{
    public class DefaultAnchorStatesCreator : IAnchorStatesCreator
    {
        public AnchorStates StartState => AnchorStates.Carried;
        
        public Dictionary<AnchorStates, IAnchorState> CreateStatesDictionary(AnchorStatesBlackboard blackboard)
        {
            Carried_AnchorState carried = 
                new Carried_AnchorState(blackboard);
            
            GrabbedToThrow_AnchorState grabbedToThrow = 
                new GrabbedToThrow_AnchorState(blackboard);
            Thrown_AnchorState thrown = 
                new Thrown_AnchorState(blackboard);
            
            Pulled_AnchorState pulled = 
                new Pulled_AnchorState(blackboard);
            
            RestingOnFloor_AnchorState restingOnFloor = 
                new RestingOnFloor_AnchorState(blackboard);
            GrabbedBySnapper_AnchorState grabbedBySnapper = 
                new GrabbedBySnapper_AnchorState(blackboard);
            
            Spinning_AnchorState spinning = 
                new Spinning_AnchorState(blackboard);
            
            
            Dictionary<AnchorStates, IAnchorState> statesDictionary = new Dictionary<AnchorStates, IAnchorState>()
            {
                { AnchorStates.Carried , carried },
                { AnchorStates.GrabbedToThrow , grabbedToThrow },
                { AnchorStates.Thrown , thrown },
                { AnchorStates.Pulled , pulled },
                { AnchorStates.RestingOnFloor , restingOnFloor },
                { AnchorStates.GrabbedBySnapper , grabbedBySnapper },
                { AnchorStates.Spinning , spinning }
            };

            return statesDictionary;
        }
    }
}