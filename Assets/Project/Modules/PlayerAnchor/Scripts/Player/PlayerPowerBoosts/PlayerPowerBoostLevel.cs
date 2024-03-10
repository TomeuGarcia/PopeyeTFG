using AYellowpaper;
using NaughtyAttributes;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts
{
    [CreateAssetMenu(fileName = "PlayerPowerBoostLevel_NUMBER", 
        menuName = ScriptableObjectsHelper.PLAYERPOWERBOOSTERS_ASSETS_PATH + "PlayerPowerBoostLevel")]
    public class PlayerPowerBoostLevel : ScriptableObject
    {
        [Header("EXPERIENCE")] 
        [SerializeField, Min(1)] private int _experienceToUnlock = 10;
        [ShowNonSerializedField] private int _accumulatedExperience;
        
        [Header("POWER BOOSTERS")]
        [SerializeField] private InterfaceReference<IPowerBooster, ScriptableObject>[] _powerBoosters;

        private delegate void PowerBoosterCallback(IPowerBooster powerBooster);

        public int ExperienceToUnlock => _experienceToUnlock;
        public int AccumulatedExperience => _accumulatedExperience;
        
            
        private void IteratePowerBoosters(PowerBoosterCallback powerBoosterCallback)
        {
            foreach (var powerBoosterReference in _powerBoosters)
            {
                powerBoosterCallback(powerBoosterReference.Value);
            }
        }
            
            
        public void Init(IPlayerMediator playerMediator)
        {
            IteratePowerBoosters((powerBooster) => powerBooster.Init(playerMediator));
            _accumulatedExperience = 0;
        }

        public bool AddExperienceToUnlock(int experience, out int reminderExperience)
        {
            _accumulatedExperience += experience;
            reminderExperience = _accumulatedExperience - _experienceToUnlock;

            _accumulatedExperience = Mathf.Min(_accumulatedExperience, _experienceToUnlock);
            
            return _accumulatedExperience == _experienceToUnlock;
        }
        
        public void Apply()
        {
            IteratePowerBoosters((powerBooster) => powerBooster.Apply());
        }
        public void Remove()
        {
            IteratePowerBoosters((powerBooster) => powerBooster.Remove());

            _accumulatedExperience = 0;
        }
    }
}