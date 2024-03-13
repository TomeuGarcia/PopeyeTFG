namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IPlayerHealingUI
    {
        void Setup(int maxNumberOfHeals, int currentNumberOfHeals);
        void OnHealUsed(int currentNumberOfHeals);
        void OnHealsExhausted();
        void OnHealsReset(int maxNumberOfHeals);
    }
}