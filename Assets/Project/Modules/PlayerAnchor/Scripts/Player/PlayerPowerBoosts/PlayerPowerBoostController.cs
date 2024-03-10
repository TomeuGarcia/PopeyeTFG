using NaughtyAttributes;
using Popeye.Modules.ValueStatSystem;
using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts
{
    
    [CreateAssetMenu(fileName = "PlayerPowerBoostController", 
        menuName = ScriptableObjectsHelper.PLAYERPOWERBOOSTERS_ASSETS_PATH + "PlayerPowerBoostController")]
    public class PlayerPowerBoostController : ScriptableObject
    {
        [Header("LEVELS")]
        [SerializeField] private PlayerPowerBoostLevel[] _powerBoostLevels;
        [SerializeField, Range(1, 10)] private int _levelLoseAmount = 3;
        
        [Space(30)]
        [ShowNonSerializedField] private int _indexOfActiveLevel;

        private bool AllLevelsAreMaxed => _indexOfActiveLevel == _powerBoostLevels.Length - 1;
        private int NextLevelIndex => _indexOfActiveLevel + 1;
        
        
        // Drawers
        [Space(30)]
        [ProgressBar("Experience", "DrawerExperienceToUnlock", EColor.Green)] // Dynamic max value constructor
        [SerializeField] private int _drawerAccumulatedExperience = 0;
        private int DrawerAccumulatedExperience => AllLevelsAreMaxed ? 0 : _powerBoostLevels[NextLevelIndex].AccumulatedExperience;
        private int DrawerExperienceToUnlock => AllLevelsAreMaxed ? 0 : _powerBoostLevels[NextLevelIndex].ExperienceToUnlock;

        

        
        public void Init(IPlayerMediator playerMediator, int numberOfActiveLevels)
        {
            foreach (PlayerPowerBoostLevel powerBoostLevel in _powerBoostLevels)
            {
                powerBoostLevel.Init(playerMediator);
            }
            
            _indexOfActiveLevel = -1;

            
            _drawerAccumulatedExperience = 0;
            AddLevels(numberOfActiveLevels);
        }

        
        public void AddExperience(int experience)
        {
            // For now do nothing with experience, and always add 1
            
            while (!AllLevelsAreMaxed &&
                _powerBoostLevels[NextLevelIndex].AddExperienceToUnlock(experience, out int reminderExperience))
            {
                AddLevels(1);
                experience = reminderExperience;
            }

            _drawerAccumulatedExperience = DrawerAccumulatedExperience;
        }

        private void AddLevels(int numberOfLevelsToAdd)
        {
            int indexOfNewActiveLevel = _indexOfActiveLevel + numberOfLevelsToAdd;
            indexOfNewActiveLevel = Mathf.Min(_powerBoostLevels.Length-1, indexOfNewActiveLevel);
            
            for (int i = _indexOfActiveLevel + 1; i <= indexOfNewActiveLevel; ++i)
            {
                PlayerPowerBoostLevel powerBoostLevel = _powerBoostLevels[i];
                powerBoostLevel.Apply();
            }

            _indexOfActiveLevel = indexOfNewActiveLevel;
        }
        
        
        
        
        public void RemoveExperience()
        {
            RemoveLevels(_levelLoseAmount);

            _drawerAccumulatedExperience = DrawerAccumulatedExperience;
        }

        private void RemoveLevels(int levelLoseAmount)
        {
            int indexOfLastActiveLevel = _indexOfActiveLevel - levelLoseAmount;
            indexOfLastActiveLevel = Mathf.Max(-1, indexOfLastActiveLevel);
            
            for (int i = _indexOfActiveLevel; i > indexOfLastActiveLevel; --i)
            {
                PlayerPowerBoostLevel powerBoostLevel = _powerBoostLevels[i];
                powerBoostLevel.Remove();
            }

            _indexOfActiveLevel = indexOfLastActiveLevel;
        }

        

        [Button()]
        private void DebugAddExperience()
        {
            AddExperience(5);
        }
        
        [Button()]
        private void DebugRemoveExperience()
        {
            RemoveExperience();
        }
    }
    
    
}