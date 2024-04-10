namespace Popeye.Modules.PlayerAnchor.Player.PlayerPlacer
{
    public interface IPlacePopeyePlayerEventChannelDispatcher
    {
        void RaiseEvent(PopeyePlayerPlacingData placingData);
    }
}