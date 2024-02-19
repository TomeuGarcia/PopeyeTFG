using System.Collections.Generic;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStates
{
    public class TutorialPlayerStatesCreator : IPlayerStatesCreator
    {
        public PlayerStates StartState => PlayerStates.SpawningWithAnchorOnFloor;
        
        public Dictionary<PlayerStates, APlayerState> CreateStatesDictionary(PlayerStatesBlackboard blackboard)
        {
            DefaultPlayerStatesCreator defaultPlayerStatesCreator = new DefaultPlayerStatesCreator();


            Dictionary<PlayerStates, APlayerState> statesDictionary =
                defaultPlayerStatesCreator.CreateStatesDictionary(blackboard);

            SpawningWithAnchorOnFloor_PlayerState spawningWithAnchorOnFloorState 
                = new SpawningWithAnchorOnFloor_PlayerState(blackboard);

            statesDictionary[PlayerStates.SpawningWithAnchorOnFloor] = spawningWithAnchorOnFloorState;

            return statesDictionary;
        }
    }
}