using System;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.ValueStatSystem
{
    [CreateAssetMenu(fileName = "TimeStepsStaminaSystemConfig", 
        menuName = ScriptableObjectsHelper.VALUESTATS_ASSETS_PATH + "TimeStepsStaminaSystemConfig")]
    public class TimeStepsStaminaSystemConfig : ScriptableObject, IStaminaConfig
    {
        [Header("STAMINA AMOUNT")]
        [SerializeField, Range(1, 100)] private int _maxStamina = 80;
        [SerializeField, Range(1, 100)] private int _spawnStamina = 80;

        [Header("STEP")]
        [SerializeField, Range(1, 100)] private int _staminaAmountRecoveringStep = 20;
        
        [Header("DELAYS")] 
        [SerializeField, Range(0.01f, 10.0f)] private float _delayStartRecovering = 1.0f;
        [SerializeField, Range(0.01f, 10.0f)] private float _delayStartRecoveringAfterExhausted = 1.5f;
        [SerializeField, Range(0.01f, 10.0f)] private float _delayRecoveringStep = 0.5f;

        
        public int MaxStamina => _maxStamina;
        public int SpawnStamina => _spawnStamina;

        public int StaminaAmountRecoveringStep => _staminaAmountRecoveringStep;
        
        public float DelayStartRecovering => _delayStartRecovering;
        public float DelayStartRecoveringAfterExhausted => _delayStartRecoveringAfterExhausted;
        public float DelayRecoveringStep => _delayRecoveringStep;



        private void OnValidate()
        {
            _spawnStamina = Mathf.Min(_spawnStamina, _maxStamina);
        }
        
        
    }
}