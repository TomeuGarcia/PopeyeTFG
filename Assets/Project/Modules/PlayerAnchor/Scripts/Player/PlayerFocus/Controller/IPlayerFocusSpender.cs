namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus
{
    public interface IPlayerFocusSpender
    {
        bool HasEnoughFocus(int focusAmount);
        void SpendFocus(int focusAmount);
    }
}