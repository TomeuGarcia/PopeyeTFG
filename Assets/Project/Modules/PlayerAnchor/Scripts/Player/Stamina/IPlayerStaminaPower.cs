namespace Popeye.Modules.PlayerAnchor.Player.Stamina
{
    public interface IPlayerStaminaPower
    {
        void AddExtraStamina(int staminaAddAmount);
        void RemoveExtraBoosts(int staminaRemoveAmount);
    }
}