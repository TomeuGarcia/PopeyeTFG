using Popeye.Modules.CombatSystem;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerHealth : IHealthBehaviourListener
    {
        private IPlayerMediator _playerMediator;
        private HealthBehaviour _playerHealthBehaviour;
        private DamageHit _voidDamageHit;
        
        
        
        public void Configure(IPlayerMediator playerMediator, HealthBehaviour playerHealthBehaviour, int maxHealth,
            Rigidbody knockbackRigidbody, DamageHitConfig voidDamageHitConfig)
        {
            _playerMediator = playerMediator;

            _playerHealthBehaviour = playerHealthBehaviour;
            _playerHealthBehaviour.Configure(this, maxHealth, DamageHitTargetType.Player, knockbackRigidbody);

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
            _playerMediator.OnHealUsed();
            _playerHealthBehaviour.Heal(healAmount);
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
    }
}