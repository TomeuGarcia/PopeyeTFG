using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Popeye.Modules.ValueStatSystem;
using TMPro;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts
{
    public class TankBarPlayerPowerBoosterUI : MonoBehaviour, IPlayerPowerBoosterUI
    {
        [SerializeField] private GameObject _nextLevelNumberHolder;
        [SerializeField] private TextMeshProUGUI _nextLevelNumberText;
        [SerializeField] private ValueStatBar _tankBar;

        private int _activeCurrentLevelNumber;

        private const int LEVEL_NUMBER_START_SHOWING = 1;
        private const float FULL_FILL_DURATION = 0.5f;


        private SimpleValueStat _experienceValueStat;

        private struct ExperienceGroup
        {
            private int _currentValue;
            private int _maxValue;
            private float _valueRatio;

            public ExperienceGroup(int currentValue, int maxValue)
            {
                _currentValue = currentValue;
                _maxValue = maxValue;
                _valueRatio = (float)_currentValue / _maxValue;
            }
            
            public int CurrentValue => _currentValue;
            public int MaxValue => _maxValue;
            public float ValueRatio => _valueRatio;
            
        }

        private ExperienceGroup _lastExperienceGroup;
        private Queue<ExperienceGroup> _addedExperienceQueue;
        private bool _processingAddedExperience;

            

        public void Init()
        {
            _experienceValueStat = new SimpleValueStat(1, 0);
            _tankBar.Init(_experienceValueStat);

            _addedExperienceQueue = new Queue<ExperienceGroup>(10);
            _lastExperienceGroup = new ExperienceGroup(0, 1);
            _processingAddedExperience = false;
            
            _activeCurrentLevelNumber = 0;
            HideText();
        }

        public void OnExperienceAdded(int currentExperience, int maxExperience)
        {
            QueueAddedExperience(currentExperience, maxExperience);
        }

        public void OnExperienceLost(int currentExperience, int maxExperience)
        {
            _addedExperienceQueue.Clear();
            QueueAddedExperience(currentExperience, maxExperience);
        }


        public void OnLevelAdded(int currentLevelNumber)
        {
            if (_activeCurrentLevelNumber < LEVEL_NUMBER_START_SHOWING &&
                currentLevelNumber >= LEVEL_NUMBER_START_SHOWING)
            {
                ShowText();
            }
            UpdateText(currentLevelNumber);
        }

        public void OnLevelLost(int currentLevelNumber)
        {
            if (_activeCurrentLevelNumber >= LEVEL_NUMBER_START_SHOWING &&
                currentLevelNumber < LEVEL_NUMBER_START_SHOWING)
            {
                HideText();
            }            
            UpdateText(currentLevelNumber);
        }


        private void UpdateText(int nextLevelNumber)
        {
            _nextLevelNumberText.text = nextLevelNumber.ToString();
            _activeCurrentLevelNumber = nextLevelNumber;
        }
        private void ShowText()
        {
            _nextLevelNumberHolder.gameObject.SetActive(true);
        }
        private void HideText()
        {
            _nextLevelNumberHolder.gameObject.SetActive(false);
        }


        private void QueueAddedExperience(int currentExperience, int maxExperience)
        {
            _addedExperienceQueue.Enqueue( new ExperienceGroup(currentExperience, maxExperience));
            
            if (_processingAddedExperience) return;
            
            ProcessQueuedExperience().Forget();
        }


        private async UniTaskVoid ProcessQueuedExperience()
        {
            _processingAddedExperience = true;
            
            while (_addedExperienceQueue.Count > 0)
            {
                ExperienceGroup experienceGroup = _addedExperienceQueue.Dequeue(); 
                UpdateExperienceTank(experienceGroup);

                float addedRatio = Mathf.Abs(experienceGroup.ValueRatio - _lastExperienceGroup.ValueRatio);
                await UniTask.Delay(TimeSpan.FromSeconds(FULL_FILL_DURATION * addedRatio));
            }

            _processingAddedExperience = false;
        }


        private void UpdateExperienceTank(ExperienceGroup experienceGroup)
        {
            _experienceValueStat.ResetMaxValue(experienceGroup.MaxValue, false);
            _experienceValueStat.SetCurrentValue(experienceGroup.CurrentValue);

            _lastExperienceGroup = experienceGroup;
        }
        
    }
}