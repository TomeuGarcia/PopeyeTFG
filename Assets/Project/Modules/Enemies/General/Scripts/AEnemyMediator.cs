using Popeye.Core.Pool;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.Enemies.Components;
using Popeye.Modules.Enemies.Hazards;
using Popeye.Modules.Enemies.VFX;
using UnityEngine;

namespace Popeye.Modules.Enemies
{
    public abstract class AEnemyMediator : RecyclableObject
    {
        [SerializeField] protected EnemyHealth _enemyHealth;
        [SerializeField] protected EnemyVisuals _enemyVisuals;
        protected IHazardFactory _hazardsFactory;

        public abstract Vector3 Position { get; }
        
        public virtual void OnHit(DamageHit damageHit)
        {
            _enemyVisuals.PlayHitEffects(_enemyHealth.GetValuePer1Ratio(), damageHit);
        }
        
        public virtual void OnDeath(DamageHit damageHit)
        {
            _enemyVisuals.PlayDeathEffects(damageHit);
            Recycle();
        }

        public void SetHazardFactory(IHazardFactory hazardsFactory)
        {
            _hazardsFactory = hazardsFactory;
        }
        public abstract void OnPlayerClose();
        public abstract void OnPlayerFar();
    }
}