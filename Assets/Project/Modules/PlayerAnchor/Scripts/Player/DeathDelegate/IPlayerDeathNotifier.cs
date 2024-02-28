namespace Popeye.Modules.PlayerAnchor.Player.DeathDelegate
{
    public interface IPlayerDeathNotifier
    {
        void AddDelegate(IPlayerDeathDelegate deathDelegate);
        void RemoveDelegate(IPlayerDeathDelegate deathDelegate);
    }
}