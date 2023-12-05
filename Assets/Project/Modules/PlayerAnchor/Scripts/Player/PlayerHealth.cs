using Project.Modules.CombatSystem;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerHealth : IHealthBehaviourListener
    {
        private IPlayerMediator _playerMediator;
        private HealthBehaviour _playerHealthBehaviour;
        
        public void Configure(IPlayerMediator playerMediator, HealthBehaviour playerHealthBehaviour, int maxHealth)
        {
            _playerMediator = playerMediator;

            _playerHealthBehaviour = playerHealthBehaviour;
            _playerHealthBehaviour.Configure(this, maxHealth, DamageHitTargetType.Player);
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
        
        public void Heal(int healAmount)
        {
            _playerHealthBehaviour.Heal(healAmount);
        }
        public void HealToMax()
        {
            _playerHealthBehaviour.HealToMax();
        }
        
    }
}