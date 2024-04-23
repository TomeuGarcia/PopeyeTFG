namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts
{
    public interface IPowerBooster
    {
        void Init(IPlayerMediator playerMediator);
        void Apply();
        void Remove();
    }
}