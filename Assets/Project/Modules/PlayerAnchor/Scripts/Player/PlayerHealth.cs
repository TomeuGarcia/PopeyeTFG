using Popeye.Modules.CombatSystem;
using Popeye.Modules.PlayerAnchor.Player.PlayerConfigurations;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerHealth : IHealthBehaviourListener, IPlayerHealthUpgrader
    {
        private IPlayerMediator _playerMediator;
        private HealthBehaviour _playerHealthBehaviour;
        private DamageHit _voidDamageHit;

        private PlayerHealthConfig.HealthConfigData _healthConfigData;
        
        
        public void Configure(
            IPlayerMediator playerMediator, 
            HealthBehaviour playerHealthBehaviour, 
            PlayerHealthConfig.HealthConfigData healthConfigData,
            Rigidbody knockbackRigidbody, 
            DamageHitConfig voidDamageHitConfig)
        {
            _playerMediator = playerMediator;

            _healthConfigData = healthConfigData;
            
            _playerHealthBehaviour = playerHealthBehaviour;
            _playerHealthBehaviour.Configure(
                this, _healthConfigData.StartingMaxHealth, _healthConfigData.StartingHealth,
                DamageHitTargetType.Player, knockbackRigidbody);

            _voidDamageHit = new DamageHit(voidDamageHitConfig);
        }

        public void OnDamageTaken(DamageHitResult damageHitResult)
        {
            _playerMediator.OnDamageTaken(damageHitResult);
        }

        public void OnKilledByDamageTaken(DamageHitResult damageHitResult)
        {
            _playerMediator.OnKilledByDamageTaken(damageHitResult);
        }

        public void OnHealed()
        {
            _playerMediator.OnHealed();
        }


        public void SetInvulnerable(bool isInvulnerable)
        {
            _playerHealthBehaviour.HealthSystem.SetInvulnerable(isInvulnerable);
        }
        public void SetInvulnerableForDuration(float duration)
        {
            _playerHealthBehaviour.HealthSystem.SetInvulnerableForDuration(duration);
        }
        
        public void HealToMax()
        {
            _playerHealthBehaviour.HealToMax();
        }

        public void Heal(int healAmount)
        {
            int healthBeforeHealing = GetCurrentHealth();
            _playerHealthBehaviour.Heal(healAmount);
            
            int currentHealth = GetCurrentHealth();
            
            _playerMediator.OnHealUsed(healthBeforeHealing, currentHealth);
        }

        public bool IsMaxHealth()
        {
            return _playerHealthBehaviour.IsMaxHealth();
        }

        public bool IsDead()
        {
            return _playerHealthBehaviour.IsDead();
        }
        
        public int GetCurrentHealth()
        {
            return _playerHealthBehaviour.HealthSystem.CurrentHealth;
        }


        public void TakeVoidFallDamage()
        {
            _playerHealthBehaviour.TakeHitDamage(_voidDamageHit);
        }

        public void IncreaseMaxHealth()
        {
            int newMaxHealth = _playerHealthBehaviour.HealthSystem.MaxHealth + _healthConfigData.HealthIncreaseAmount;
            _playerHealthBehaviour.ResetMaxHealth(newMaxHealth, true);
        }
    }
}