using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.Enemies.Components;
using UnityEngine;
using UnityEngine.Serialization;
using System.Threading.Tasks;
using Popeye.Core.Services.ServiceLocator;
using Project.Modules.CombatSystem;
using Task = System.Threading.Tasks.Task;


namespace Popeye.Modules.Enemies
{
    public class SlimeMediator : AEnemyMediator
    {
        [SerializeField] private SlimeMovement _slimeMovement;
        [SerializeField] private SquashStretchAnimator _squashStretchAnimator;
        [SerializeField] private SlimeDivider _slimeDivider;
        [SerializeField] private EnemyPatrolling _enemyPatrolling;
        [SerializeField] private DamageTrigger _damageTrigger;
        
        [SerializeField] private DamageHitConfig _contactDamageHitConfig;

        [SerializeField] private BoxCollider _boxCollider;
        public SlimeMindEnemy slimeMindEnemy;
        public Transform playerTransform { get; private set; }
        [SerializeField] private Transform _slimeTransform;
        private Transform _particlePoolParent;
        private Core.Pool.ObjectPool _objectPool;

        public void Init()
        {
            _slimeMovement.Configure(this);
            _enemyHealth.Configure(this);
            _squashStretchAnimator.Configure(this,_slimeTransform,_objectPool);
            _slimeDivider.Configure(this);
            _enemyPatrolling.Configure(this);
            _damageTrigger.Configure(ServiceLocator.Instance.GetService<ICombatManager>(),new DamageHit(_contactDamageHitConfig));
        }

        public void SetSlimeMind(SlimeMindEnemy slimeMind)
        {
            slimeMindEnemy = slimeMind;
        }

        public void SetObjectPool(Core.Pool.ObjectPool objectPool)
        {
            _objectPool = objectPool;
        }

        public Core.Pool.ObjectPool GetObjectPool()
        {
            return _objectPool;
        }
        public void PlayMoveAnimation()
        {
            _squashStretchAnimator.PlayMove();
        }
        public void SetPlayerTransform(Transform _playerTransform)
        {
            playerTransform = _playerTransform;
            _slimeMovement.SetTarget(playerTransform);
            _enemyPatrolling.SetPlayerTransform(playerTransform);
        }

        public void SetWayPoints(Transform[] wayPoints)
        {
            _enemyPatrolling.SetWayPoints(wayPoints);
        }
       
        public void AddSlimesToSlimeMindList(SlimeMediator mediator)
        {
            slimeMindEnemy.AddSlimeToList();
        }

        public void SpawningFromDivision(Vector3 explosionForceDir,EnemyPatrolling.PatrolType type,Transform[] wayPoints)
        {
            ApplyDivisionExplosionForces(explosionForceDir,type,wayPoints);
        }

        private async void ApplyDivisionExplosionForces(Vector3 explosionForceDir,EnemyPatrolling.PatrolType type,Transform[] wayPoints)
        {
            _slimeMovement.DeactivateNavigation();
            _squashStretchAnimator.PlayDeath();
            _enemyHealth.SetIsInvulnerable(true);
            _boxCollider.isTrigger = false;
            _slimeMovement.ApplyExplosionForce(explosionForceDir);

            await Task.Delay(TimeSpan.FromSeconds(0.5f));

            _slimeMovement.StopExplosionForce();
            _boxCollider.isTrigger = true;
            _slimeMovement.ActivateNavigation();
            if(type == EnemyPatrolling.PatrolType.FixedWaypoints){SetWayPoints(wayPoints);}
            else if (type == EnemyPatrolling.PatrolType.None){StartChasing();}
            PlayMoveAnimation();
            _enemyHealth.SetIsInvulnerable(false);
        }

        public void Divide()
        {
            _squashStretchAnimator.PlayDeath();
            _slimeDivider.SpawnSlimes();
            slimeMindEnemy.RemoveSlimeFromList();
            _squashStretchAnimator.StopMove();
            Destroy(gameObject);
        }

        public override void OnDeath()
        {
            base.OnDeath();
            Divide();
        }

        public override void OnPlayerClose()
        {
            StartChasing();
        }

        public override void OnPlayerFar()
        {
            StartPatrolling();
        }

        public void StartChasing()
        {
            _enemyPatrolling.SetPatrolling(false);
            _slimeMovement.StartChasing();
        }

        public void StartPatrolling()
        {
            _slimeMovement.StopChasing();
            _enemyPatrolling.SetPatrolling(true);
        }

        public EnemyPatrolling.PatrolType GetPatrolType()
        {
            return _enemyPatrolling.GetPatrolType();
        }
        public Transform[] GetPatrolWaypoints()
        {
            return _enemyPatrolling.GetWaypoints();
        }
    }
}
