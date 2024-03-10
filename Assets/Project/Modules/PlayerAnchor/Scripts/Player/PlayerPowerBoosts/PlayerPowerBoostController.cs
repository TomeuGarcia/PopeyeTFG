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

        private IPlayerPowerBoosterUI _playerPowerBoosterUI;
        
        
        // Drawers
        [Space(30)]
        [ProgressBar("Experience", "DrawerExperienceToUnlock", EColor.Green)] // Dynamic max value constructor
        [SerializeField] private int _accumulatedExperience;
        private int DrawerExperienceToUnlock => AllLevelsAreMaxed ? 0 : _powerBoostLevels[NextLevelIndex].ExperienceToUnlock;


        
        public void Init(IPlayerMediator playerMediator, int startingExperience, IPlayerPowerBoosterUI playerPowerBoosterUI)
        {
            foreach (PlayerPowerBoostLevel powerBoostLevel in _powerBoostLevels)
            {
                powerBoostLevel.Init(playerMediator);
            }
            
            _indexOfActiveLevel = -1;
            _accumulatedExperience = 0;

            _playerPowerBoosterUI = playerPowerBoosterUI;
            
            AddExperience(startingExperience);
        }

        
        public void AddExperience(int experienceToAdd)
        {
            while (!AllLevelsAreMaxed &&
                   experienceToAdd > 0 &&
                   AddExperienceToUnlock(experienceToAdd, out int reminderExperience))
            {
                AddLevels(1);
                experienceToAdd = reminderExperience;
            }
            
            _playerPowerBoosterUI.OnLevelAdded(NextLevelIndex + 1);
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
        
        
        private bool AddExperienceToUnlock(int experienceToAdd, out int reminderExperience)
        {
            _accumulatedExperience += experienceToAdd;

            int experienceToUnlock = DrawerExperienceToUnlock;
            
            reminderExperience = Mathf.Max(0, _accumulatedExperience - experienceToUnlock);
            
            bool levelUp = _accumulatedExperience >= experienceToUnlock;
            
            if(levelUp)
            {
                _accumulatedExperience = 0;
            }
            
            return levelUp;
        }
        
        
        
        public void RemoveExperience()
        {
            RemoveLevels(_levelLoseAmount);

            _accumulatedExperience = 0;
            _playerPowerBoosterUI.OnLevelAdded(NextLevelIndex + 1);
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