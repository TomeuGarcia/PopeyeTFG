using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.Enemies;
using Popeye.Modules.Enemies.VFX;
using Popeye.Modules.ValueStatSystem;
using Popeye.Modules.CombatSystem;
using UnityEngine;

public class TurretHealth : MonoBehaviour, IDamageHitTarget
{
    private HealthSystem _healthSystem;
    [SerializeField] protected EnemyVisuals _enemyVisuals;
    [SerializeField, Range(0, 100)] private int _maxHealth = 50;
    [SerializeField] private ProximityTargetGetterBehaviour _enemy;

    private Vector3 Position => transform.position;
    
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
        int receivedDamage = _healthSystem.TakeDamage(damageHit.Damage);
        if (IsDead())
        {   
            _enemy.Die();
            Destroy(gameObject);
        }
        else
        {
            _enemyVisuals.PlayHitEffects(_healthSystem.GetValuePer1Ratio(), damageHit);
        }

        return new DamageHitResult(this, gameObject, damageHit, receivedDamage, Position);
    }

    public bool CanBeDamaged(DamageHit damageHit)
    {
        return !_healthSystem.IsDead() && !_healthSystem.IsInvulnerable;
    }

    public bool IsDead()
    {
        return _healthSystem.IsDead();
    }
}
