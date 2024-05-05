using System;
using Popeye.Core.Pool;
using Popeye.Modules.AudioSystem;
using Popeye.Modules.Enemies.Hazards;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus.Spikes
{
    [CreateAssetMenu(fileName = "ChainSpikesAttackConfig", 
        menuName = ScriptableObjectsHelper.PLAYERSPECIALATTACKS_ASSETS_PATH + "ChainSpikesAttackConfig")]
    public class ChainSpikesAttackConfig : ScriptableObject
    {
        [Header("DURATIONS")]
        [SerializeField] private float _totalDuration = 0.5f;
        [SerializeField] private float _delay = 0.05f;
        
        [Header("PREFAB")]
        [SerializeField] private ObjectPoolData<ChainSpike>  _chainSpikePoolData;
        
        [Header("SPIKES POSITIONING")]
        [SerializeField, Range(0, 20)] private int _numberOfSpikePoints = 10;
        [SerializeField, Range(0f, 1f)] private float _firstSpikePositionRatio = 0.3f;
        [SerializeField, Range(0f, 1f)] private float _lastSpikePositionRatio = 0.95f;
        
        
        public float TotalDuration => _totalDuration;
        public float Delay => _delay;
        
        public ObjectPoolData<ChainSpike> ChainSpikePoolData => _chainSpikePoolData;
        
        public int NumberOfSpikePoints => _numberOfSpikePoints;
        public float FirstSpikePositionRatio => _firstSpikePositionRatio;
        public float LastSpikePositionRatio => _lastSpikePositionRatio;
        
        
        public Action OnValuesChanged;
        
        private void OnValidate()
        {
            OnValuesChanged?.Invoke();
        }
        
        
        [Header("AUDIO")] 
        [SerializeField] private AFMODAudioManagerReference _audioManager;
        [SerializeField] private OneShotFMODSound _performSound;

        public void PlayPerformSound()
        {
            _audioManager.PlayOneShot(_performSound);
        }
    }
}