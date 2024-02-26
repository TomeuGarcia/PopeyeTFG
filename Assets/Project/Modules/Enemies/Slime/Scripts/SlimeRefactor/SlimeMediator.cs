using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.Enemies.Components;
using UnityEngine;
using UnityEngine.Serialization;
using System.Threading.Tasks;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.Enemies.EnemyFactories;
using Popeye.Modules.Enemies.Slime;
using Popeye.Modules.VFX.ParticleFactories;
using Task = System.Threading.Tasks.Task;


namespace Popeye.Modules.Enemies
{
    public class SlimeMediator : AEnemyMediator
    {
        [SerializeField] private SlimeMovement _slimeMovement;
        [FormerlySerializedAs("_squashStretchAnimator")] [SerializeField] private SlimeAnimatorController slimeAnimatorController;
        [SerializeField] private EnemyPatrolling _enemyPatrolling;
        [SerializeField] private DamageTrigger _damageTrigger;
        
        [SerializeField] private DamageHitConfig _contactDamageHitConfig;

        [SerializeField] private BoxCollider _boxCollider;
        [HideInInspector] public SlimeMindEnemy slimeMindEnemy;
        private SlimeFactory _slimeFactory;
        public Transform PlayerTransform { get; private set; }
        public SlimeSizeID SlimeSizeID { get; private set; }
        [SerializeField] private Transform _slimeTransform;
        private Transform _particlePoolParent;
        private Core.Pool.ObjectPool _objectPool;
    

        public override Vector3 Position => _slimeTransform.position;

        public void InitAfterSpawn()
        {
            _enemyVisuals.Configure(ServiceLocator.Instance.GetService<IParticleFactory>());
            _slimeMovement.Configure(this);
            _enemyHealth.Configure(this);
            slimeAnimatorController.Configure(this,ServiceLocator.Instance.GetService<IParticleFactory>());
            _enemyPatrolling.Configure(this);
            _damageTrigger.Configure(ServiceLocator.Instance.GetService<ICombatManager>(),new DamageHit(_contactDamageHitConfig));
            
            PlayMoveAnimation();
        }

        public void SetSlimeMind(SlimeMindEnemy slimeMind)
        {
            slimeMindEnemy = slimeMind;
        }

        public void SetSlimeFactory(SlimeFactory slimeFactory)
        {
            _slimeFactory = slimeFactory;
        }

        public void SetSlimeSize(SlimeSizeID slimeSizeID)
        {
            SlimeSizeID = slimeSizeID;
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
            slimeAnimatorController.PlayMove();
        }
        public void SetPlayerTransform(Transform _playerTransform)
        {
            PlayerTransform = _playerTransform;
            _slimeMovement.SetTarget(PlayerTransform);
            _enemyPatrolling.SetPlayerTransform(PlayerTransform);
        }

        public void SetWayPoints(Transform[] wayPoints)
        {
            _enemyPatrolling.SetWayPoints(wayPoints);
        }

        public void SetPatrolingType()
        {
            
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
            slimeAnimatorController.PlayDeath();
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
            slimeAnimatorController.PlayDeath();
            if (_slimeFactory.CanSpawnNextSize(SlimeSizeID))
            {
                _slimeFactory.CreateFromParent(slimeMindEnemy,this,Position,Quaternion.identity);
            }
            slimeMindEnemy.RemoveSlimeFromList();
            slimeAnimatorController.StopMove();
        }

        public override void OnDeath(DamageHit damageHit)
        {
            Divide();
            base.OnDeath(damageHit);
            
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

        internal override void Init()
        {
        }

        internal override void Release()
        {
            _enemyHealth.HealToMax();
            _enemyPatrolling.resetPatrolling();
            _slimeTransform.localPosition = Vector3.zero;
        }
    }
}
