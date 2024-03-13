using System;
using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.ValueStatSystem
{
    [CreateAssetMenu(fileName = "TimeStaminaConfig_SO", 
        menuName = ScriptableObjectsHelper.VALUESTATS_ASSETS_PATH + "TimeStaminaConfig")]
    public class TimeStaminaConfig_SO : ScriptableObject, ITimeStaminaConfig
    {
        [Header("VALUES")] 
        [SerializeField, Range(0, 500)] private int _maxStamina = 100;
        [SerializeField, Range(0, 500)] private int _spawnStamina = 100;

        public int SpawnMaxStamina => _maxStamina;
        public int SpawnStamina => _spawnStamina;
        public int CurrentMaxStamina { get; set; }


        [Header("DURATIONS")]
        [SerializeField, Range(0.01f, 20.0f)] private float _fullRecoverDuration = 1.0f;
        [SerializeField, Range(0.01f, 20.0f)] private float _recoverAfterUseDelayDuration = 0.5f;
        [SerializeField, Range(0.01f, 20.0f)] private float _recoverAfterExhaustDelayDuration = 2.0f;
        
        public float FullRecoverDuration => _fullRecoverDuration;
        public float RecoverAfterUseDelayDuration => _recoverAfterUseDelayDuration;
        public float RecoverAfterExhaustDelayDuration => _recoverAfterExhaustDelayDuration;


        private void OnValidate()
        {
            _spawnStamina = Mathf.Min(_spawnStamina, _maxStamina);
            CurrentMaxStamina = _spawnStamina;
        }

        private void Awake()
        {
            OnValidate();
        }
    }
}