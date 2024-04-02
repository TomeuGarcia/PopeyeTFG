using System;
using Popeye.Modules.Enemies.Components;
using UnityEngine;
using UnityEngine.Serialization;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.Enemies.EnemyFactories;
using Popeye.Modules.Enemies.Slime;
using Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops;
using Popeye.Modules.VFX.ParticleFactories;
using Popeye.Scripts.Collisions;
using Project.Modules.Enemies.Slime.Scripts.SlimeRefactor;
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


        [SerializeField] private CollisionProbingConfig _floorCollisionProbingConfig;

        [SerializeField] private SlimeSoundsConfig _slimeSounds;
        private IFMODAudioManager _audioManager;
        
        [SerializeField] private PowerBoostDropConfig _powerBoostDrop;
        private IPowerBoostDropFactory _powerBoostDropFactory;

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
        public void SetAudioManager(IFMODAudioManager audioManager)
        {
            _audioManager = audioManager;
        }
        public void SetBoostDropFactory(IPowerBoostDropFactory powerBoostDropFactory)
        {
            _powerBoostDropFactory = powerBoostDropFactory;
        }

        public void SetSlimeSize(SlimeSizeID slimeSizeID)
        {
            SlimeSizeID = slimeSizeID;
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
            StartPatrolling();
        }


       
        public void AddSlimesToSlimeMindList(SlimeMediator childSlimeMediator)
        {
            slimeMindEnemy.AddSlimeToList(childSlimeMediator);
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
            //_boxCollider.isTrigger = false;
            _slimeMovement.ApplyExplosionForce(explosionForceDir);

            await Task.Delay(TimeSpan.FromSeconds(0.5f));

            _slimeMovement.StopExplosionForce();
            //_boxCollider.isTrigger = true;
            _slimeMovement.ActivateNavigation();
            if(type == EnemyPatrolling.PatrolType.FixedWaypoints){SetWayPoints(wayPoints);}
            else if (type == EnemyPatrolling.PatrolType.None){StartChasing();}
            PlayMoveAnimation();
            _enemyHealth.SetIsInvulnerable(false);
        }

        public void Divide()
        {
            slimeAnimatorController.PlayDeath();
            _powerBoostDropFactory.Create(Position, Quaternion.identity, _powerBoostDrop);
            
            if (_slimeFactory.CanSpawnNextSize(SlimeSizeID))
            {
                _slimeFactory.CreateFromParent(slimeMindEnemy,this, ComputeChildSlimesSpawnPosition(), Quaternion.identity);
                _slimeSounds.PlayDivideSound(_audioManager, _slimeTransform.gameObject, SlimeSizeID);
            }
            else
            {
                _slimeSounds.PlayDeathSound(_audioManager, _slimeTransform.gameObject, SlimeSizeID);
            }
            
            slimeMindEnemy.RemoveSlimeFromList(this);
            slimeAnimatorController.StopMove();
        }

        private Vector3 ComputeChildSlimesSpawnPosition()
        {
            if (Physics.Raycast(Position, Vector3.down, out RaycastHit hit, 
                    _floorCollisionProbingConfig.ProbeDistance, _floorCollisionProbingConfig.CollisionLayerMask,
                    _floorCollisionProbingConfig.QueryTriggerInteraction))
            {
                return hit.point + (hit.normal * 1.0f);
            }

            return Position;
        }
        
        public override void OnDeath(DamageHitResult damageHitResult)
        {
            Divide();
            base.OnDeath(damageHitResult);
        }

        public override void OnPlayerClose()
        {
            base.OnPlayerClose();
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
            _slimeMovement.StopExplosionForce();
        }

        internal override void Release()
        {
            _enemyHealth.HealToMax();
            _enemyPatrolling.ResetPatrolling();
            _slimeTransform.localPosition = Vector3.zero;
            _slimeMovement.StopExplosionForce();
        }
        
        public override void DieFromOrder()
        {
            Recycle();
        }
    }
}
