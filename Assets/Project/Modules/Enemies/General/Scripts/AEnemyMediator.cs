using Popeye.Modules.CombatSystem;
using Popeye.Modules.Enemies.Components;
using Popeye.Modules.Enemies.VFX;
using UnityEngine;

namespace Popeye.Modules.Enemies
{
    public abstract class AEnemyMediator : MonoBehaviour
    {
        [SerializeField] protected EnemyHealth _enemyHealth;
        [SerializeField] protected EnemyVisuals _enemyVisuals;

        public virtual void OnHit(DamageHit damageHit)
        {
            _enemyVisuals.PlayHitEffects(_enemyHealth.GetValuePer1Ratio(), damageHit);
        }
        
        public virtual void OnDeath(DamageHit damageHit)
        {
            _enemyVisuals.PlayDeathEffects(damageHit);
        }

        public abstract void OnPlayerClose();
        public abstract void OnPlayerFar();
    }
}