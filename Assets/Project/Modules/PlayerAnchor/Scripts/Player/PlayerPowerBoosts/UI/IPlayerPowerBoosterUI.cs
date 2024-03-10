using Popeye.Modules.ValueStatSystem;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts
{
    public interface IPlayerPowerBoosterUI
    {
        void Init(AValueStat experienceValueStat);
        void OnLevelAdded(int nextLevelNumber);
        void OnLevelLost(int nextLevelNumber);
    }
}