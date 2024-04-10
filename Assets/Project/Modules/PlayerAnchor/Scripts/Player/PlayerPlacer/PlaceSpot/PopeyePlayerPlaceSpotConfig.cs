using Popeye.ProjectHelpers;
using Popeye.Scripts.EntityPlacing;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPlacer
{
 
    [CreateAssetMenu(fileName = "WorldPopeyePlayerPlaceSpotConfig", 
        menuName = ScriptableObjectsHelper.PLAYERPLACING_ASSETS_PATH + "PlaceSpotConfig")]
    public class PopeyePlayerPlaceSpotConfig : ScriptableObject
    {
        [SerializeField] private WorldEntityPlacerViewData _playerViewData;
        [SerializeField] private WorldEntityPlacerViewData _anchorViewData;
        
        public WorldEntityPlacerViewData PlayerViewData => _playerViewData;
        public WorldEntityPlacerViewData AnchorViewData => _anchorViewData;
    }
}