using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPlacer
{
    [CreateAssetMenu(fileName = "PlacePopeyePlayerEventChannel", 
        menuName = ScriptableObjectsHelper.PLAYERPLACING_ASSETS_PATH + "PlaceEventChannel")]
    public class PlacePopeyePlayerEventChannelAsset : ScriptableObject, 
        IPlacePopeyePlayerEventChannelListenEntry, IPlacePopeyePlayerEventChannelDispatcher
    {
        private IPlacePopeyePlayerEventChannelListenEntry.ChannelEvent OnRequest;
        
        [SerializeField] private bool _logWarining = true;
        
        public void Subscribe(IPlacePopeyePlayerEventChannelListenEntry.ChannelEvent callback)
        {
            OnRequest += callback;
        }

        public void Unsubscribe(IPlacePopeyePlayerEventChannelListenEntry.ChannelEvent callback)
        {
            OnRequest -= callback;
        }
        
        
        public void RaiseEvent(PopeyePlayerPlacingData placingData)
        {
            if (OnRequest != null)
            {
                OnRequest.Invoke(placingData);
            }
            else if (_logWarining)
            {
                Debug.LogWarning( $"An Event was requested but nobody picked it up [{name}]");
            }
        }
        
        
    }
}