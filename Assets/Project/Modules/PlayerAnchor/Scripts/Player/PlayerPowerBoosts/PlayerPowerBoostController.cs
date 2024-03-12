using NaughtyAttributes;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts
{
    
    [CreateAssetMenu(fileName = "PlayerPowerBoostController", 
        menuName = ScriptableObjectsHelper.PLAYERPOWERBOOSTERS_ASSETS_PATH + "PlayerPowerBoostController")]
    public class PlayerPowerBoostController : ScriptableObject, IPlayerPowerBoostController
    {
        [Header("LEVELS")]
        [Expandable] [SerializeField] private PlayerPowerBoostLevel[] _powerBoostLevels;
        [SerializeField, Range(1, 10)] private int _levelLoseAmount = 3;
        
        [Space(30)]
        [ShowNonSerializedField] private int _indexOfActiveLevel;

        private bool AllLevelsAreMaxed => _indexOfActiveLevel == _powerBoostLevels.Length - 1;
        private int NextLevelIndex => _indexOfActiveLevel + 1;

        private IPlayerPowerBoosterUI _playerPowerBoosterUI;
        
        
        // Drawers
        [Space(30)]
        [ProgressBar("Experience", "NextExperienceToUnlock", EColor.Green)] // Dynamic max value constructor
        [SerializeField] private int _accumulatedExperience;
        private int NextExperienceToUnlock => AllLevelsAreMaxed ? 1 : _powerBoostLevels[NextLevelIndex].ExperienceToUnlock;


        
        public void Init(IPlayerMediator playerMediator, int startingExperience, IPlayerPowerBoosterUI playerPowerBoosterUI)
        {
            foreach (PlayerPowerBoostLevel powerBoostLevel in _powerBoostLevels)
            {
                powerBoostLevel.Init(playerMediator);
            }
            
            _indexOfActiveLevel = -1;
            _accumulatedExperience = 0;
            
            _playerPowerBoosterUI = playerPowerBoosterUI;
            _playerPowerBoosterUI.Init();
            
            AddExperience(startingExperience);
        }

        
        public void AddExperience(int experienceToAdd)
        {
            bool leveledUp = false;
            
            while (!AllLevelsAreMaxed &&
                   experienceToAdd > 0 &&
                   AddExperienceToUnlock(experienceToAdd, out int reminderExperience))
            {
                AddLevels(1);
                _accumulatedExperience = 0;
                experienceToAdd = reminderExperience;
                leveledUp = true;
            }

            if (leveledUp && !AllLevelsAreMaxed)
            {
                _playerPowerBoosterUI.OnExperienceAdded(_accumulatedExperience, NextExperienceToUnlock);
            }
            
            _playerPowerBoosterUI.OnLevelAdded(NextLevelIndex);
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

            int experienceToUnlock = NextExperienceToUnlock;
            
            reminderExperience = Mathf.Max(0, _accumulatedExperience - experienceToUnlock);
            
            bool levelUp = _accumulatedExperience >= experienceToUnlock;
            
            
            _playerPowerBoosterUI.OnExperienceAdded(_accumulatedExperience, experienceToUnlock);

            
            return levelUp;
        }
        
        
        
        public void RemoveExperience()
        {
            RemoveLevels(_levelLoseAmount);

            _accumulatedExperience = 0;
            _playerPowerBoosterUI.OnLevelLost(NextLevelIndex);
            _playerPowerBoosterUI.OnExperienceLost(0, NextExperienceToUnlock);
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


#if UNITY_EDITOR
        
        [SerializeField, Range(0, 100)] private int _debugExperienceToAdd = 5;
        [Button()]
        private void DebugAddExperience()
        {
            AddExperience(_debugExperienceToAdd);
        }
        
        [Button()]
        private void DebugRemoveExperience()
        {
            RemoveExperience();
        }
#endif
        
    }
    
    
}