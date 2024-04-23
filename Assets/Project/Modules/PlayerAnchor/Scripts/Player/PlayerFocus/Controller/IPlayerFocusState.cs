namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus
{
    public interface IPlayerFocusState 
    {
        int CurrentFocusAmount { get; }
        int MaxFocusAmount { get; }
    }
}