using Popeye.Modules.Enemies.Components;
using Popeye.Modules.Enemies.VFX;
using UnityEngine;

namespace Popeye.Modules.Enemies
{
    public abstract class AEnemyMediator : MonoBehaviour
    {
        [SerializeField] protected EnemyHealth _enemyHealth;
        [SerializeField] protected EnemyVisuals _enemyVisuals;

        public virtual void OnHit()
        {
            _enemyVisuals.PlayHitEffects(_enemyHealth.GetValuePer1Ratio());
        }
        
        public virtual void OnDeath()
        {
            _enemyVisuals.PlayDeathEffects();
        }

        public abstract void OnPlayerClose();
        public abstract void OnPlayerFar();
    }
}