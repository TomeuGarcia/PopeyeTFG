namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops
{
    public interface IPowerBoostDrop
    {
        bool CanBeUsed();
        int GetExperienceAndSetUsed();
    }
}