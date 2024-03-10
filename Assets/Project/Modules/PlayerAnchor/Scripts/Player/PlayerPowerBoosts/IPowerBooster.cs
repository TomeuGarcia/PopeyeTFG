namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts
{
    public interface IPowerBooster
    {
        void AddBoost();
        void RemoveBoosts(int numberOfBoostsToRemove);
    }
}