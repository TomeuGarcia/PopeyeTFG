using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Enemies.StateMachine;
using Popeye.Modules.PlayerController;
using Popeye.Modules.PlayerController.Inputs;
using Popeye.Modules.ValueStatSystem;
using Popeye.Modules.CombatSystem;
using Project.Modules.CombatSystem.KnockbackSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace Popeye.Modules.Enemies
{
    public class SpiderEnemy : AEnemy, IDamageHitTarget, IKnockbackHitTarget, IMovementInputHandler
    {
        [Header("SLIME")]
        [Header("COMPONENTS")] 
        [SerializeField] private ISpiderEnemyStateMachine _stateMachine;
        [SerializeField] private PlayerController.PlayerController _enemyController;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private MeshRenderer _meshRenderer;
        private Material _meshMaterial;
        
        
        [Header("HEALTH")]
        [SerializeField, Range(0.0f, 100.0f)] private int _maxHealth = 50;
        private HealthSystem _healthSystem;

        [Header("MOVE SPEEDS")] 
        [SerializeField, Range(0.0f, 100.0f)] private float _maxMoveSpeed = 16.0f;
        public float MaxMoveSpeed => _maxMoveSpeed;

        [Header("CONTACT DAMAGE")]
        [SerializeField] private DamageHitConfig _contactDamageConfig;
        private DamageHit _contactDamageHit;
        
        private bool _canDealContactDamage;

        [Header("KNOCKBACK")] 
        [SerializeField, Range(0.0f, 1.0f)] private float _knockbackEffectiveness = 1.0f;

        [HideInInspector] public float stunTime;
        

        public Vector3 Position => transform.position;
        public Vector3 LookDirection => _enemyController.LookDirection;
        public Vector3 TargetPosition => _attackTarget.position;


        private bool _canMove;
        private int _disabledMovementCount;

        private bool _respawnsAfterDeath;
        private Vector3 _respawnPosition;

        private ICombatManager _combatManager;




        private bool _alreadyInitialized = false;

        private void Start()
        {
            _combatManager = ServiceLocator.Instance.GetService<ICombatManager>();
            
            if (_alreadyInitialized) return;
        }

        public override void SetPatrollingWaypoints(Transform[] waypoints)
        {
            throw new System.NotImplementedException();
        }

        public override void AwakeInit(Transform attackTarget)
        {
            base.AwakeInit(attackTarget);
            AwakeInit_old(attackTarget, false);
        }

        public void AwakeInit_old(Transform attackTarget, bool respawnsAfterDeath)
        {
            _attackTarget = attackTarget;

            _enemyController.MovementInputHandler = this;
            _enemyController.MaxSpeed = _maxMoveSpeed;

            _canMove = true;
            _disabledMovementCount = 0;

            _respawnsAfterDeath = respawnsAfterDeath;
            _respawnPosition = new Vector3(-10, 10, 10);

            _healthSystem = new HealthSystem(_maxHealth);

            _stateMachine.AwakeInit(this);

            

            _contactDamageHit = new DamageHit(_contactDamageConfig);

            DisableDealingContactDamage();

            _alreadyInitialized = true;

            _meshMaterial = _meshRenderer.material;
        }

        private void Update()
        {
            if (_respawnsAfterDeath && transform.position.y < -1)
            {
                Respawn();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_canDealContactDamage)
            {
                _contactDamageHit.DamageSourcePosition = Position;
                _contactDamageHit.UpdateKnockbackPushDirection(
                    PositioningHelper.Instance.GetDirectionAlignedWithFloor(Position, other.transform.position)
                );

                _combatManager.TryDealDamage(other.gameObject, _contactDamageHit,
                    out DamageHitResult damageHitResult);
            }
        }


        public void SetRespawnPosition(Vector3 respawnPosition)
        {
            _respawnPosition = respawnPosition;
        }


        public void EnableDealingContactDamage()
        {
            _canDealContactDamage = true;
        }

        public void DisableDealingContactDamage()
        {
            _canDealContactDamage = false;
        }



        public DamageHitTargetType GetDamageHitTargetType()
        {
            return DamageHitTargetType.Enemy;
        }

        public DamageHitResult TakeHitDamage(DamageHit damageHit)
        {
            int receivedDamage = _healthSystem.TakeDamage(damageHit.Damage);
            if (_healthSystem.IsDead())
            {
                _stateMachine.OverwriteCurrentState(ISpiderEnemyState.States.Dead);
            }
            else
            {
                stunTime = damageHit.StunDuration;
                _stateMachine.OverwriteCurrentState(ISpiderEnemyState.States.Stunned);
            }
            
            StartTakeDamageAnimation().Forget();

            return new DamageHitResult(this, gameObject, receivedDamage, Position);
        }

        public bool CanBeDamaged(DamageHit damageHit)
        {
            return !IsDead() && !_healthSystem.IsInvulnerable;
        }

        public bool IsDead()
        {
            return _healthSystem.IsDead();
        }

        public async UniTaskVoid StartDeathAnimation()
        {
            float deathDuration = 1.0f;
            _enemyController.enabled = false;

            SetCanRotate(false);
            GetStunned(deathDuration);

            await transform.DOBlendableRotateBy(Vector3.right * 180f, deathDuration).AsyncWaitForCompletion();

            InvokeOnDeathComplete();

            if (_respawnsAfterDeath)
            {
                Respawn();
            }
            else
            {
                Recycle();
            }
        }

        private void Respawn()
        {
            _enemyController.enabled = true;
            transform.position = _respawnPosition;
            transform.rotation = Quaternion.identity;
            _healthSystem.HealToMax();
            SetCanRotate(true);

            _stateMachine.ResetStateMachine();
        }

        private void TakeKnockback(Vector3 knockbackForce)
        {
            transform.DOKill();

            Vector3 pushForce = knockbackForce * _knockbackEffectiveness;

            _rigidbody.AddForce(pushForce, ForceMode.Impulse);
        }

        public async void GetStunned(float duration)
        {
            DisableMovement();
            await Task.Delay((int)(duration * 1000));

            if (!_healthSystem.IsDead())
            {
                EnableMovement();
            }
        }

        public void SetMaxMoveSpeed(float maxMoveSpeed)
        {
            _enemyController.MaxSpeed = maxMoveSpeed;
        }

        public void SetCanRotate(bool canRotate)
        {
            _enemyController.CanRotate = canRotate;
        }

        private void EnableMovement()
        {
            --_disabledMovementCount;

            if (_disabledMovementCount == 0)
            {
                _enemyController.enabled = true;
            }
        }

        private void DisableMovement()
        {
            if (_disabledMovementCount == 0)
            {
                _enemyController.enabled = false;
            }

            ++_disabledMovementCount;
        }

        public bool IsTargetOnReachableHeight()
        {
            return Mathf.Abs(Position.y - TargetPosition.y) < 0.5f;
        }


        private Vector3 GetHeightIgnoredDirection(Vector3 start, Vector3 end)
        {
            Vector3 direction = end - start;
            direction.y = 0;
            return direction.normalized;
        }


        public Vector3 ForwardAxis => Vector3.forward;
        public Vector3 RightAxis => Vector3.right;

        public Vector3 GetMovementInput()
        {
            if (!_canMove)
            {
                return Vector3.zero;
            }

            Vector3 movementInput = GetHeightIgnoredDirection(Position, TargetPosition);

            return movementInput;
        }

        public Vector3 GetLookInput()
        {
            return GetHeightIgnoredDirection(Position, TargetPosition);
        }


        private async UniTaskVoid StartTakeDamageAnimation()
        {
            for (int i = 0; i < 2; ++i)
            {
                _meshMaterial.color = Color.red;
                await UniTask.Delay(MathUtilities.SecondsToMilliseconds(0.1f));
                _meshMaterial.color = Color.white;
                await UniTask.Delay(MathUtilities.SecondsToMilliseconds(0.1f));
            }
        }

        public Rigidbody GetRigidbodyToKnockback()
        {
            return _rigidbody;
        }

        public bool CanBeKnockbacked()
        {
            return true;
        }

        public float GetKnockbackEffectivenessMultiplier()
        {
            return 1;
        }

        internal override void Init()
        {
            AwakeInit(_attackTarget);
            SetRespawnPosition(Position);
        }

        internal override void Release()
        {
            
        }
    }
}
