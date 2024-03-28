using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.Enemies.Components;
using Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops;
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
        [SerializeField] private ShieldedStun _shieldedStun;
        
        [SerializeField] private Material _stunnedMaterial;
        [SerializeField] private Material _invulnerableMaterial;
        [SerializeField] private MeshRenderer _body;
        [SerializeField] private EnemyHealthPassInvulnerableAttacks _shieldedHealth;
        
        
        [SerializeField] private PowerBoostDropConfig _powerBoostDrop;
        private IPowerBoostDropFactory _powerBoostDropFactory;
        private bool _chasing = false;

        [SerializeField] private Rigidbody _rigidbody;
        internal override void Init()
        {
            _damageTrigger.Configure(ServiceLocator.Instance.GetService<ICombatManager>(),new DamageHit(_contactDamageHitConfig));
            _shieldedMovement.Configure(this);
            _enemyPatrolling.Configure(this);
            _shieldedDashing.Configure(this);
            _shieldedStun.Configure(this);
            _enemyHealth.Configure(this);
            _enemyVisuals.Configure(ServiceLocator.Instance.GetService<IParticleFactory>());
            _enemyHealth.SetIsInvulnerable(true);
            _shieldedHealth.OnTakePassInvulnerableHit += Stun;
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
        
        public void SetBoostDropFactory(IPowerBoostDropFactory powerBoostDropFactory)
        {
            _powerBoostDropFactory = powerBoostDropFactory;
        }
        public void StartChasing()
        {
            _chasing = true;
            _enemyPatrolling.SetPatrolling(false);
            _shieldedMovement.StartChasing();
        }

        public void StopMoving()
        {
            _chasing = false;
            _enemyPatrolling.SetPatrolling(false);
            _shieldedMovement.StopChasing();
        }

        public void StartPatrolling()
        {
            _chasing = false;
            _shieldedMovement.StopChasing();
            _enemyPatrolling.SetPatrolling(true);
        }
        public void SetWayPoints(Transform[] wayPoints)
        {
            _enemyPatrolling.SetWayPoints(wayPoints);
            StartPatrolling();
        }
        public void StartDashing()
        {
            
            _chasing = false;
            _shieldedMovement.StopChasing();
        }
        public void StopDashing()
        {
            _shieldedDashing.StopDashing();
        }

        
        public bool IsChasing()
        {
            return _chasing;
        }
        internal override void Release()
        {
            _enemyPatrolling.ResetPatrolling();
            _shieldedHealth.OnTakePassInvulnerableHit -= Stun;
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
            _powerBoostDropFactory.Create(transform.position, Quaternion.identity, _powerBoostDrop);
            _enemyVisuals.PlayDeathEffects(damageHit);
            _shieldedMind.Die();
        }

        public override void OnHit(DamageHit damageHit)
        {
            base.OnHit(damageHit);
            _shieldedStun.CancellStun();
        }

        public void Stun()
        {
            _rigidbody.velocity = Vector3.zero;
            _shieldedStun.Stun();
        }

        public void SetIsInvulnerable(bool isInvulnerable)
        {
            if (isInvulnerable)
            {
                _body.material = _invulnerableMaterial;
            }
            else
            {
                _body.material = _stunnedMaterial;
            }
            _enemyHealth.SetIsInvulnerable(isInvulnerable);
        }
        public void ResetDashingCooldown()
        {
            _shieldedDashing.ResetDashingCooldown();
        }
    }
}
