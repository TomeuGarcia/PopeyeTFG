using System.Collections.Generic;

namespace Popeye.Modules.PlayerAnchor.Anchor.AnchorStates
{
    public interface IAnchorStatesCreator
    {
        AnchorStates StartState { get; }

        Dictionary<AnchorStates, IAnchorState> CreateStatesDictionary(AnchorStatesBlackboard blackboard);
    }
}