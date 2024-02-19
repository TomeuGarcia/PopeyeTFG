using Popeye.Modules.CombatSystem;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerHealth : IHealthBehaviourListener
    {
        private IPlayerMediator _playerMediator;
        private HealthBehaviour _playerHealthBehaviour;
        private int _potionHealAmount;
        private DamageHit _voidDamageHit;
        
        public void Configure(IPlayerMediator playerMediator, HealthBehaviour playerHealthBehaviour, int maxHealth,
            int potionHealAmount, Rigidbody knockbackRigidbody, DamageHitConfig voidDamageHitConfig)
        {
            _playerMediator = playerMediator;
            _potionHealAmount = potionHealAmount;

            _playerHealthBehaviour = playerHealthBehaviour;
            _playerHealthBehaviour.Configure(this, maxHealth, DamageHitTargetType.Player, knockbackRigidbody);

            _voidDamageHit = new DamageHit(voidDamageHitConfig);
        }

        public void OnDamageTaken(DamageHitResult damageHitResult)
        {
            _playerMediator.OnDamageTaken();
        }

        public void OnKilledByDamageTaken(DamageHitResult damageHitResult)
        {
            _playerMediator.OnKilledByDamageTaken();
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
        
        public void UseHeal()
        {
            _playerHealthBehaviour.Heal(_potionHealAmount);
        }
        public void HealToMax()
        {
            _playerHealthBehaviour.HealToMax();
        }

        public bool IsMaxHealth()
        {
            return _playerHealthBehaviour.IsMaxHealth();
        }

        public bool IsDead()
        {
            return _playerHealthBehaviour.IsDead();
        }


        public void TakeVoidFallDamage()
        {
            _playerHealthBehaviour.TakeHitDamage(_voidDamageHit);
        }
    }
}