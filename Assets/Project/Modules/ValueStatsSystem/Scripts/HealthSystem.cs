using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.ValueStatSystem
{
    public class HealthSystem : AValueStat
    {
        private int _maxHealth;
        private int _currentHealth;
        public int MaxHealth => _maxHealth;    
        public int CurrentHealth => _currentHealth;    
    
        private bool _isInvulnerable;
        public bool IsInvulnerable
        { 
            get { return _isInvulnerable; } 
            set { _isInvulnerable = value;}
        }
        private Queue<float> _queuedInvulnerableDurations;


        public HealthSystem(int maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
            _isInvulnerable = false;

            _queuedInvulnerableDurations = new Queue<float>(3);
        }
    
    
        public int TakeDamage(int damageAmount)
        {
            if (IsInvulnerable) { return 0; }
    
            int receivedDamage = Mathf.Min(damageAmount, _currentHealth);
    
            _currentHealth -= damageAmount;
            _currentHealth = Mathf.Max(0, _currentHealth);

            InvokeOnValueUpdate();
    
            return receivedDamage;
        }
        
        public void Kill()
        {
            _currentHealth = 0;

            InvokeOnValueUpdate();
        }
    
    
        public void Heal(int healAmount)
        {
            _currentHealth += healAmount;
            _currentHealth = Mathf.Min(MaxHealth, _currentHealth);

            InvokeOnValueUpdate();
        }
        
        public void HealToMax()
        {
            _currentHealth = MaxHealth;

            InvokeOnValueUpdate();
        }
    
        public bool IsDead()
        {
            return _currentHealth == 0;
        }
        public bool IsMaxHealth()
        {
            return _currentHealth == MaxHealth;
        }

        public void SetInvulnerable(bool isInvulnerable)
        {
            _isInvulnerable = isInvulnerable;
        }
        public void SetInvulnerableForDuration(float duration)
        {
            bool alreadyProcessingInvulnerableForDuration = _queuedInvulnerableDurations.Count > 0;
            _queuedInvulnerableDurations.Enqueue(duration);
            
            if (alreadyProcessingInvulnerableForDuration)
            {
                return;
            }
            
            ProcessInvulnerableForDuration().Forget();
        }

        private async UniTaskVoid ProcessInvulnerableForDuration()
        {
            SetInvulnerable(true);
            
            while (_queuedInvulnerableDurations.Count > 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(_queuedInvulnerableDurations.Peek()));

                _queuedInvulnerableDurations.Dequeue();
            }
            
            SetInvulnerable(false); 
        }
    
        public override float GetValuePer1Ratio()
        {
            return (float)_currentHealth / _maxHealth;
        }
    }
}


