
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPlacer
{
    public interface IPlacePopeyePlayerEventChannelListenEntry
    {
        delegate void ChannelEvent(PopeyePlayerPlacingData placingData);
        
        void Subscribe(ChannelEvent callback);
        void Unsubscribe(ChannelEvent callback);
    }
}