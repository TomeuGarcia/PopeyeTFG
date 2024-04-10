using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPlacer
{
    
    public class PopeyePlayerPlaceSpotInitializer : MonoBehaviour
    {
        [Header("CHANNEL")]
        [SerializeField] private PlacePopeyePlayerEventChannelAsset _placePopeyePlayerEventChannel;
        
        [Header("COMPONENT")]
        [SerializeField] private PopeyePlayerPlaceSpot _popeyePlayerPlaceSpot;


        private void Awake()
        {
            _popeyePlayerPlaceSpot.Configure(_placePopeyePlayerEventChannel);
        }
        
    }
}