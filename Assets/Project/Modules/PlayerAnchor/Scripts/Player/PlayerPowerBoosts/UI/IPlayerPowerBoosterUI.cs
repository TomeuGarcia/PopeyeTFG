
namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts
{
    public interface IPlayerPowerBoosterUI
    {
        void Init();
        void OnExperienceAdded(int currentExperience, int maxExperience);
        void OnExperienceLost(int currentExperience, int maxExperience);
        void OnLevelAdded(int currentLevelNumber);
        void OnLevelLost(int currentLevelNumber);
    }
}