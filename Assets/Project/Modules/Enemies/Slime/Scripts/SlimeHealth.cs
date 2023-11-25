using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeHealth : MonoBehaviour,IDamageHitTarget
{
        private HealthSystem _healthSystem;
        [SerializeField, Range(0.0f, 100.0f)] private float _maxHealth = 50.0f;
        [SerializeField] private GameObject _slimeToSpawn;
        [SerializeField] private int _numberOfSlimes;
        [SerializeField] private GameObject _explosionParticles;
        

        private void Awake()
        {
            _healthSystem = new HealthSystem(_maxHealth);
        }
    
        public DamageHitTargetType GetDamageHitTargetType()
        {
            return DamageHitTargetType.Enemy;
        }
    
        public DamageHitResult TakeHitDamage(DamageHit damageHit)
        {
            _healthSystem.TakeDamage(damageHit.Damage);
            if (IsDead())
            {
                Instantiate(_explosionParticles, transform.position, Quaternion.identity);
                SpawnSlimes();
                Destroy(transform.parent.gameObject);
            }
    
            return new DamageHitResult(damageHit.Damage);
        }
    
        public bool CanBeDamaged(DamageHit damageHit)
        {
            return !_healthSystem.IsDead() && !_healthSystem.IsInvulnerable;
        }
    
        public bool IsDead()
        {
            return _healthSystem.IsDead();
        }

        public void SetIsInvulnerable(bool _isInvulnerable)
        {
            _healthSystem.IsInvulnerable = _isInvulnerable;
        }

        private void SpawnSlimes()
        {
            for (int i = 0; i < _numberOfSlimes; i++)
            {
                float angle = i * 360f / _numberOfSlimes;
                Vector3 dir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
                
                GameObject slimeSpawned = Instantiate(_slimeToSpawn,transform.position,Quaternion.identity);
                slimeSpawned.transform.GetChild(0).GetComponent<SlimeMovement>().SpawningFromExplosion(dir);
            }
        }
}
