namespace Popeye.Modules.PlayerAnchor.Player.DeathDelegate
{
    public interface IPlayerDeathDelegate
    {
        void OnPlayerDied();
        void OnPlayerRespawnedFromDeath();
    }
}