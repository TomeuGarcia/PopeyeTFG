using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    
    
        public HealthSystem(int maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
            _isInvulnerable = false;
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
        public async void SetInvulnerableForDuration(float duration, bool setVulnerableEvenIfDead = false)
        {
            SetInvulnerable(true);
    
            await Task.Delay(TimeSpan.FromSeconds(duration));
    
            if (!IsDead() || setVulnerableEvenIfDead)
            {
                SetInvulnerable(false);
            }        
        }
    
        public override float GetValuePer1Ratio()
        {
            return (float)_currentHealth / _maxHealth;
        }
    }
}


