using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.ValueStatSystem
{
    public class HealthSystem : AValueStat
    {
        private float _maxHealth;
        private float _currentHealth;
        public float MaxHealth => _maxHealth;    
        public float CurrentHealth => _currentHealth;    
    
        private bool _isInvulnerable;
        public bool IsInvulnerable
        { 
            get { return _isInvulnerable; } 
            set { _isInvulnerable = value;}
        }
    
    
        public HealthSystem(float maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
            _isInvulnerable = false;
        }
    
    
        public float TakeDamage(float damageAmount)
        {
            if (IsInvulnerable) { return 0.0f; }
    
            float receivedDamage = Mathf.Min(damageAmount, _currentHealth);
    
            _currentHealth -= damageAmount;
            _currentHealth = Mathf.Max(0f, _currentHealth);

            InvokeOnValueUpdate();
    
            return receivedDamage;
        }
        
        public void Kill()
        {
            _currentHealth = 0f;

            InvokeOnValueUpdate();
        }
    
    
        public void Heal(float healAmount)
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
            return _currentHealth == 0f;
        }
    
        public async void SetInvulnerableForDuration(float duration, bool setVulnerableEvenIfDead = false)
        {
            _isInvulnerable = true;
    
            await Task.Delay((int)(duration * 1000));
    
            if (!IsDead() || setVulnerableEvenIfDead)
            {
                _isInvulnerable = false;
            }        
        }
    
        public override float GetValuePer1Ratio()
        {
            return _currentHealth / _maxHealth;
        }
    }
}


