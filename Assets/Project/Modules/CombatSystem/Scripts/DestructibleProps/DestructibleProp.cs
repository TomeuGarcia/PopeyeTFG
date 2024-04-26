using System;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.ValueStatSystem;
using Popeye.Modules.PlayerAnchor;
using Popeye.Modules.PlayerController.AutoAim;
using Project.Modules.CombatSystem.KnockbackSystem;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.CombatSystem.Testing.Scripts
{
    public class DestructibleProp : MonoBehaviour, IDamageHitTarget, IKnockbackHitTarget, IAutoAimTarget, IHealthUserBehaviour
    {
        [Header("COMPONENTS")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        [SerializeField] private InterfaceReference<IDestructiblePropView, MonoBehaviour> _view;

        [Header("CONFIGURATION")]
        [SerializeField] private DestructiblePropConfig _config;
        [SerializeField] private AutoAimTargetDataConfig _autoAimTargetDataConfig;

        public HealthSystem HealthSystem { get; private set; }
        private IDestructiblePropView View => _view.Value;
        private TransformMotion _transformMotion;

        private Vector3 _spawnPosition;
        private Quaternion _spawnRotation;

        private Transform Transform => transform;
        
        
        
        public AutoAimTargetDataConfig DataConfig => _autoAimTargetDataConfig;
        Vector3 IAutoAimTarget.Position => Position;
        public GameObject GameObject => gameObject;
        public bool CanBeAimedAt(Vector3 aimFromPosition)
        {
            return true;
        }

        
        
        private Vector3 Position => transform.position;

        private void Awake()
        {
            HealthSystem = new HealthSystem(_config.MaxHealth);
            _transformMotion = new TransformMotion();
            _transformMotion.Configure(transform);

            View.Configure(_config.ViewConfig, Transform);

            _spawnPosition = Transform.position;
            _spawnRotation = Transform.rotation;
        }

        public void Spawn()
        {
            _transformMotion.SetPosition(_spawnPosition);
            _transformMotion.SetRotation(_spawnRotation);
            gameObject.SetActive(true);
            HealthSystem.HealToMax();

            if (!_rigidbody.isKinematic)
            {
                _rigidbody.velocity = Vector3.zero;
            }

            _collider.enabled = true;
        }
        

        public DamageHitTargetType GetDamageHitTargetType()
        {
            return DamageHitTargetType.Destructible;
        }

        public DamageHitResult TakeHitDamage(DamageHit damageHit)
        {
            int receivedDamage = HealthSystem.TakeDamage(damageHit.Damage);
            
            if (HealthSystem.IsDead())
            {
                OnKilledByDamage().Forget();
            }
            else
            { 
                OnDamageTaken();
            }

            return new DamageHitResult(this, gameObject, damageHit, receivedDamage, Position);
        }

        public bool CanBeDamaged(DamageHit damageHit)
        {
            return !HealthSystem.IsDead();
        }

        public bool IsDead()
        {
            return HealthSystem.IsDead();
        }


        private void OnDamageTaken()
        {
            View.PlayTakeDamageAnimation();
        }
        private async UniTaskVoid OnKilledByDamage()
        {
            _collider.enabled = false;
            await View.PlayDestroyedAnimation();
            gameObject.SetActive(false);
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
            return (1-_config.KnockbackResistance);
        }
    }
}