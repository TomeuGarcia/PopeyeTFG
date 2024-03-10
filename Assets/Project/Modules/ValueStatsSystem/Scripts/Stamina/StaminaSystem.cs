using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.ValueStatSystem
{
    public class StaminaSystem : AValueStat, IStaminaSystem
    {
        public override int MaxValue => MaxStamina;
        
        private int _currentStamina;
        public int MaxStamina => _config.CurrentMaxStamina;
        public int CurrentStamina => _currentStamina;

        private readonly IStaminaConfig _config;
        
        public StaminaSystem(IStaminaConfig config)
        {
            _config = config;
            _currentStamina = _config.SpawnStamina;
        }
    
    
        public void Spend(int spendAmount)
        {
            _currentStamina -= spendAmount;
            _currentStamina = Mathf.Max(0, _currentStamina);
    
            InvokeOnValueUpdate();
        }
    
        public void SpendAll()
        {
            _currentStamina = 0;
    
            InvokeOnValueUpdate();
        }
    
    
        public void Restore(int gainAmount)
        {
            _currentStamina += gainAmount;
            _currentStamina = Mathf.Min(MaxStamina, _currentStamina);
    
            InvokeOnValueUpdate();
        }
    
        public void RestoreAll()
        {
            _currentStamina = MaxStamina;
    
            InvokeOnValueUpdate();
        }
    
        public bool HasStaminaLeft()
        {
            return _currentStamina > 0;
        }
        public bool HasMaxStamina()
        {
            return _currentStamina == MaxStamina;
        }
        public bool HasEnoughStamina(float staminaAmount)
        {
            return _currentStamina >= staminaAmount;
        }
    
    
        public override float GetValuePer1Ratio()
        {
            return (float)_currentStamina / MaxStamina;
        }
        public override int GetValue()
        {
            return _currentStamina;
        }

        protected override void DoResetMaxValue(int maxValue, bool setValueToMax)
        {
            _config.CurrentMaxStamina = maxValue;

            if (setValueToMax)
            {
                _currentStamina = MaxStamina;
            }
            else
            {
                _currentStamina = Mathf.Min(_currentStamina, MaxStamina);
            }
        }
    }
}

