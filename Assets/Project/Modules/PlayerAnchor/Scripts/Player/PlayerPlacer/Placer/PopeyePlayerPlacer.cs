using Popeye.Modules.PlayerAnchor.AbilityUnlock;
using Popeye.Modules.PlayerAnchor.Player.InstantTranslation;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using Popeye.Modules.PlayerAnchor.SafeGroundChecking.Checkpoint;
using Popeye.Modules.PlayerAnchor.SafeGroundChecking.Dynamic;
using Popeye.Modules.VFX.Generic;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPlacer
{
    public class PopeyePlayerPlacer
    {
        private readonly IPlacePopeyePlayerEventChannelListenEntry _placeEventChannelListenEntry;
        private readonly IPlayerInstantTranslation _playerInstantTranslation;
        private readonly PlayerFSM _playerStateMachine;
        private readonly EnvironmentFollower _environmentFollower;
        private readonly IDynamicCheckpointCreator _dynamicCheckpointCreator;
        private readonly ICheckpointStorerWrite _playerCheckpointStorerWrite;
        private readonly PlayerAbilitiesToUnlockHolder _abilitiesToUnlockHolder;


        public PopeyePlayerPlacer(
            IPlacePopeyePlayerEventChannelListenEntry placeEventChannelListenEntry,
            IPlayerInstantTranslation playerInstantTranslation,
            PlayerFSM playerStateMachine,
            EnvironmentFollower environmentFollower,
            IDynamicCheckpointCreator dynamicCheckpointCreator,
            PlayerAbilitiesToUnlockHolder abilitiesToUnlockHolder
        )
        {
            _placeEventChannelListenEntry = placeEventChannelListenEntry;
            _playerInstantTranslation = playerInstantTranslation;
            _playerStateMachine = playerStateMachine;
            _environmentFollower = environmentFollower;
            _dynamicCheckpointCreator = dynamicCheckpointCreator;
            _abilitiesToUnlockHolder = abilitiesToUnlockHolder;
        }

        public void StartListening()
        {
            _placeEventChannelListenEntry.Subscribe(PlacePlayer);
        }
        
        public void StopListening()
        {
            _placeEventChannelListenEntry.Unsubscribe(PlacePlayer);
        }

        private void PlacePlayer(PopeyePlayerPlacingData placingData)
        {
            _playerInstantTranslation.TranslatePlayerAndPauseForAFrame(placingData.playerPosition, placingData.playerRotation);
            _playerInstantTranslation.TranslateAnchorAndReset(placingData.anchorPosition, placingData.anchorRotation);

            if (placingData.startCarryingAnchor)
            {
                _playerStateMachine.OverwriteState(PlayerStates.PlayerStates.Spawning);
            }
            else
            {
                _playerStateMachine.OverwriteState(PlayerStates.PlayerStates.SpawningWithAnchorOnFloor);
            }

            if (placingData.isNewPlayerRespawn)
            {
                _dynamicCheckpointCreator.SetCurrentStateAsCheckpoint();
            }

            if (placingData.debugUnlockAllAbilities)
            {
                _abilitiesToUnlockHolder.DebugUnlockAll();
            }

            _environmentFollower.Configure(placingData.environmentFollowData);
        }
        

        
    }
}