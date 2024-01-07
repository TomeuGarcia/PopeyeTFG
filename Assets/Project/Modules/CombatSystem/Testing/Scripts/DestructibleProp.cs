using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.ValueStatSystem;
using Popeye.Modules.PlayerAnchor;
using Project.Modules.CombatSystem.KnockbackSystem;
using UnityEngine;

namespace Popeye.Modules.CombatSystem.Testing.Scripts
{
    public class DestructibleProp : MonoBehaviour, IDamageHitTarget, IKnockbackHitTarget
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField, Range(0.0f, 1.0f)] private float _knockbackResistance = 0.0f;
        [SerializeField, Range(0, 100)] private int _maxHealth = 20;
        private HealthSystem _healthSystem;
        private TransformMotion _transformMotion;

        private Vector3 _spawnPosition;
        private Quaternion _spawnRotation;

        private Vector3 Position => transform.position;

        private void Awake()
        {
            _healthSystem = new HealthSystem(_maxHealth);
            _transformMotion = new TransformMotion();
            _transformMotion.Configure(transform);

            _spawnPosition = transform.position;
            _spawnRotation = transform.rotation;
        }

        public void Spawn()
        {
            transform.position = _spawnPosition;
            transform.rotation = _spawnRotation;
            gameObject.SetActive(true);
            _healthSystem.HealToMax();
        }
        

        public DamageHitTargetType GetDamageHitTargetType()
        {
            return DamageHitTargetType.Destructible;
        }

        public DamageHitResult TakeHitDamage(DamageHit damageHit)
        {
            int receivedDamage = _healthSystem.TakeDamage(damageHit.Damage);
            
            if (_healthSystem.IsDead())
            {
                PlayDieAnimation().Forget();
            }
            else
            {
                PlayTakeDamageAnimation(damageHit);
            }

            return new DamageHitResult(this, gameObject, receivedDamage, Position);
        }

        public bool CanBeDamaged(DamageHit damageHit)
        {
            return !_healthSystem.IsDead();
        }

        public bool IsDead()
        {
            return _healthSystem.IsDead();
        }

        private async UniTaskVoid PlayDieAnimation()
        {
            transform.DOPunchScale(Vector3.one * -0.5f, 0.4f)
                .SetEase(Ease.OutBounce);
            _transformMotion.Rotate(transform.rotation * Quaternion.Euler(-90, 500, 0), 0.5f);
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            
            gameObject.SetActive(false);
        }
        private void PlayTakeDamageAnimation(DamageHit damageHit)
        {
            transform.DOPunchScale(Vector3.one * -0.5f, 0.4f)
                .SetEase(Ease.OutBounce);
        }

        
        public Rigidbody GetRigidbodyToKnockback()
        {
            return _rigidbody;
        }

        public bool CanBeKnockbacked()
        {
            return true;
        }

        public float GetKnockbackEffectiveness()
        {
            return (1-_knockbackResistance);
        }
    }
}