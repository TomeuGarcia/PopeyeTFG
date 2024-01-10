using Popeye.Modules.CombatSystem;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerHealth : IHealthBehaviourListener
    {
        private IPlayerMediator _playerMediator;
        private HealthBehaviour _playerHealthBehaviour;
        private int _potionHealAmount;
        
        public void Configure(IPlayerMediator playerMediator, HealthBehaviour playerHealthBehaviour, int maxHealth,
            int potionHealAmount, Rigidbody knockbackRigidbody)
        {
            _playerMediator = playerMediator;
            _potionHealAmount = potionHealAmount;

            _playerHealthBehaviour = playerHealthBehaviour;
            _playerHealthBehaviour.Configure(this, maxHealth, DamageHitTargetType.Player, knockbackRigidbody);
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
        
    }
}