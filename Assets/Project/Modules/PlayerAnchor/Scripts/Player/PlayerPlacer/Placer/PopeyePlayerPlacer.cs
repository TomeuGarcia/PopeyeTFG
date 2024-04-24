using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.AbilityUnlock;
using Popeye.Modules.PlayerAnchor.Anchor;
using Popeye.Modules.PlayerAnchor.Chain;
using Popeye.Modules.PlayerAnchor.Player.AutoActionsQueue;
using Popeye.Modules.PlayerAnchor.Player.InstantTranslation;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using Popeye.Modules.PlayerAnchor.SafeGroundChecking;
using Popeye.Modules.VFX.Generic;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPlacer
{
    public class PopeyePlayerPlacer
    {
        private readonly IPlacePopeyePlayerEventChannelListenEntry _placeEventChannelListenEntry;
        private readonly IPlayerInstantTranslation _playerInstantTranslation;
        private readonly PlayerFSM _playerStateMachine;
        private readonly EnvironmentFollower _environmentFollower;
        private readonly ISafeGroundOnDemand _playerRespawnSafeGround;
        private readonly PlayerAbilitiesToUnlockHolder _abilitiesToUnlockHolder;


        public PopeyePlayerPlacer(
            IPlacePopeyePlayerEventChannelListenEntry placeEventChannelListenEntry,
            IPlayerInstantTranslation playerInstantTranslation,
            PlayerFSM playerStateMachine,
            EnvironmentFollower environmentFollower,
            ISafeGroundOnDemand playerRespawnSafeGround,
            PlayerAbilitiesToUnlockHolder abilitiesToUnlockHolder
        )
        {
            _placeEventChannelListenEntry = placeEventChannelListenEntry;
            _playerInstantTranslation = playerInstantTranslation;
            _playerStateMachine = playerStateMachine;
            _environmentFollower = environmentFollower;
            _playerRespawnSafeGround = playerRespawnSafeGround;
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
                _playerRespawnSafeGround.SetCurrentStateAsSafeGround();
            }

            if (placingData.debugUnlockAllAbilities)
            {
                _abilitiesToUnlockHolder.DebugUnlockAll();
            }

            _environmentFollower.Configure(placingData.environmentFollowData);
        }
        

        
    }
}