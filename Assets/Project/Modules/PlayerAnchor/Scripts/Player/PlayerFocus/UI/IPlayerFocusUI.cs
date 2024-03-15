namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus
{
    public interface IPlayerFocusUI
    {
        void OnFocusGained();
        void OnFocusSpent();
        void OnMaxFocusAmountChanged();
    }
}