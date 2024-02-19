using System.Collections.Generic;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public interface IPlayerStatesCreator
    {
        PlayerStates StartState { get; }
        Dictionary<PlayerStates, APlayerState> CreateStatesDictionary(PlayerStatesBlackboard blackboard);
    }
}