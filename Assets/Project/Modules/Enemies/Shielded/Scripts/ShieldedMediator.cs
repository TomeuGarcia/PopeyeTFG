using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.Enemies.Components;
using Popeye.Modules.VFX.ParticleFactories;
using UnityEngine;
using UnityEngine.AI;

namespace Popeye.Modules.Enemies
{
    public class ShieldedMediator : AEnemyMediator
    {
        [SerializeField] private ShieldedMovement _shieldedMovement;
        [SerializeField] private EnemyPatrolling _enemyPatrolling;
        [SerializeField] private ShieldedDashing _shieldedDashing;
        private ShieldedMind _shieldedMind;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private DamageTrigger _damageTrigger;
        [SerializeField] private DamageHitConfig _contactDamageHitConfig;
        private bool _chasing = false;
        internal override void Init()
        {
            _damageTrigger.Configure(ServiceLocator.Instance.GetService<ICombatManager>(),new DamageHit(_contactDamageHitConfig));
            _shieldedMovement.Configure(this);
            _enemyPatrolling.Configure(this);
            _shieldedDashing.Configure(this);
            _enemyHealth.Configure(this);
            _enemyVisuals.Configure(ServiceLocator.Instance.GetService<IParticleFactory>());
        }

        public void SetShieldedMind(ShieldedMind shieldedMind)
        {
            _shieldedMind = shieldedMind ;
        }
        
        public void SetPlayerTransform(Transform _playerTransform)
        {
            _shieldedMovement.SetTarget(_playerTransform);
            _enemyPatrolling.SetPlayerTransform(_playerTransform);
            _shieldedDashing.SetPlayerTransform(_playerTransform);
        }
        public void StartChasing()
        {
            _chasing = true;
            _enemyPatrolling.SetPatrolling(false);
            _shieldedMovement.StartChasing();
        }

        public void StartPatrolling()
        {
            _chasing = false;
            _shieldedMovement.StopChasing();
            _enemyPatrolling.SetPatrolling(true);
        }
        public void StartDashing()
        {
            _chasing = false;
            _shieldedMovement.StopChasing();
        }
        public void StopDashing()
        {
            _chasing = true;
            _shieldedMovement.StartChasing();
        }
        public bool IsChasing()
        {
            return _chasing;
        }
        internal override void Release()
        {
            _enemyPatrolling.ResetPatrolling();
        }

        public override Vector3 Position { get; }
        public override void OnPlayerClose()
        {
            StartChasing();
        }
        public virtual void DeactivateNavigation()
        {
            _navMeshAgent.enabled = false;
        }
        public virtual void ActivateNavigation()
        {
            _navMeshAgent.enabled = true;
        }
        public override void OnPlayerFar()
        {
            StartPatrolling();
        }
        
        public override void DieFromOrder()
        {
            _shieldedMind.Die();
        }
        public override void OnDeath(DamageHit damageHit)
        {
            _enemyVisuals.PlayDeathEffects(damageHit);
            _shieldedMind.Die();
        }
    }
}
