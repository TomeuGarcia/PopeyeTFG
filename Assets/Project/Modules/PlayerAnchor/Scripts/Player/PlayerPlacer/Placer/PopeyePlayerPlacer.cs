using Popeye.Modules.PlayerAnchor.Player.AutoActionsQueue;
using Popeye.Modules.PlayerAnchor.Player.InstantTranslation;
using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPlacer
{
    public class PopeyePlayerPlacer
    {
        private readonly IPlacePopeyePlayerEventChannelListenEntry _placeEventChannelListenEntry;
        private readonly IPlayerInstantTranslation _playerInstantTranslation;
        private readonly PlayerFSM _playerStateMachine;
        private readonly IPlayerAutoActionsQueue _playerAutoActionsQueue;


        public PopeyePlayerPlacer(
            IPlacePopeyePlayerEventChannelListenEntry placeEventChannelListenEntry,
            IPlayerInstantTranslation playerInstantTranslation,
            PlayerFSM playerStateMachine,
            IPlayerAutoActionsQueue playerAutoActionsQueue
        )
        {
            _placeEventChannelListenEntry = placeEventChannelListenEntry;
            _playerInstantTranslation = playerInstantTranslation;
            _playerStateMachine = playerStateMachine;
            _playerAutoActionsQueue = playerAutoActionsQueue;
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
            _playerInstantTranslation.TranslatePlayer(placingData.playerPosition, placingData.playerRotation);

            if (placingData.startCarryingAnchor)
            {
                // Es puto lia
                //_playerStateMachine.OverwriteState(PlayerStates.PlayerStates.Spawning);
                _playerAutoActionsQueue.TryQueueAnchorPull();
            }
            else
            {
                _playerInstantTranslation.TranslateAnchor(placingData.anchorPosition, placingData.anchorRotation);
                _playerStateMachine.OverwriteState(PlayerStates.PlayerStates.SpawningWithAnchorOnFloor);
            }
            
        }
    }
}