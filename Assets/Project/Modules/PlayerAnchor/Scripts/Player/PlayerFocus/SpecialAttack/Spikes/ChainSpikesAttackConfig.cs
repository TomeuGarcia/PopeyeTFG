using System;
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
        [SerializeField] private ChainSpike _chainSpikePrefab;
        
        [Header("SPIKES POSITIONING")]
        [SerializeField, Range(0, 20)] private int _numberOfSpikePoints = 10;
        [SerializeField, Range(0f, 1f)] private float _firstSpikePositionRatio = 0.3f;
        [SerializeField, Range(0f, 1f)] private float _lastSpikePositionRatio = 0.95f;
        
        
        public float TotalDuration => _totalDuration;
        public float Delay => _delay;
        
        public ChainSpike ChainSpikePrefab => _chainSpikePrefab;
        
        public int NumberOfSpikePoints => _numberOfSpikePoints;
        public float FirstSpikePositionRatio => _firstSpikePositionRatio;
        public float LastSpikePositionRatio => _lastSpikePositionRatio;
        
        
        public Action OnValuesChanged;
        
        private void OnValidate()
        {
            OnValuesChanged?.Invoke();
        }
    }
}