using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.ValueStatSystem
{
    [CreateAssetMenu(fileName = "TimeStaminaConfig", 
        menuName = "Popeye/ValueStats/TimeStaminaConfig")]
    public class TimeStaminaConfig_SO : ScriptableObject, ITimeStaminaConfig
    {
        [Header("VALUES")] 
        [SerializeField, Range(0, 500)] private int _maxStamina = 100;
        [SerializeField, Range(0, 500)] private int _spawnStamina = 100;

        public int MaxStamina => _maxStamina;
        public int SpawnStamina => _spawnStamina;


        [Header("DURATIONS")]
        [SerializeField, Range(0.01f, 20.0f)] private float _fullRecoverDuration = 1.0f;
        [SerializeField, Range(0.01f, 20.0f)] private float _recoverDelayAfterUseDuration = 0.5f;
        [SerializeField, Range(0.01f, 20.0f)] private float _recoverDelayAfterExhaustDuration = 2.0f;
        
        public float FullRecoverDuration => _fullRecoverDuration;
        public float RecoverDelayAfterUseDuration => _recoverDelayAfterUseDuration;
        public float RecoverDelayAfterExhaustDuration => _recoverDelayAfterExhaustDuration;


        private void OnValidate()
        {
            _spawnStamina = Mathf.Min(_spawnStamina, _maxStamina);
        }

        private void Awake()
        {
            OnValidate();
        }
    }
}