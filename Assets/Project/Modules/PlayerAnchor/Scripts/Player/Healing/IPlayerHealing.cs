namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IPlayerHealing
    {
        bool CanHeal(out bool hasHealsLeft);
        void UseHeal();
        void ResetHeals();
    }
}