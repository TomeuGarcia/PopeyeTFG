using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Anchor;
using Popeye.Modules.PlayerAnchor.Chain;
using Popeye.Modules.PlayerAnchor.Player.AutoActionsQueue;
using Popeye.Modules.PlayerAnchor.Player.InstantTranslation;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
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


        public PopeyePlayerPlacer(
            IPlacePopeyePlayerEventChannelListenEntry placeEventChannelListenEntry,
            IPlayerInstantTranslation playerInstantTranslation,
            PlayerFSM playerStateMachine,
            EnvironmentFollower environmentFollower
        )
        {
            _placeEventChannelListenEntry = placeEventChannelListenEntry;
            _playerInstantTranslation = playerInstantTranslation;
            _playerStateMachine = playerStateMachine;
            _environmentFollower = environmentFollower;
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

            _environmentFollower.Configure(placingData.environmentFollowData);
        }
        

        
    }
}