using Popeye.Modules.PlayerAnchor.Player.InstantTranslation;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPlacer
{
    public class PopeyePlayerPlacer
    {
        private readonly IPlacePopeyePlayerEventChannelListenEntry _placeEventChannelListenEntry;
        private readonly IPlayerInstantTranslation _playerInstantTranslation;


        public PopeyePlayerPlacer(
            IPlacePopeyePlayerEventChannelListenEntry placeEventChannelListenEntry,
            IPlayerInstantTranslation playerInstantTranslation
        )
        {
            _placeEventChannelListenEntry = placeEventChannelListenEntry;
            _playerInstantTranslation = playerInstantTranslation;
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
            }
            else
            {
                _playerInstantTranslation.TranslateAnchor(placingData.anchorPosition, placingData.anchorRotation);
            }
            
        }
    }
}