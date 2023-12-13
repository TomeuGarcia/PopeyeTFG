using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.ValueStatSystem;
using Project.Modules.CombatSystem;
using UnityEngine;

public class TurretHealth : MonoBehaviour, IDamageHitTarget
{
    private HealthSystem _healthSystem;
    [SerializeField, Range(0, 100)] private int _maxHealth = 50;
    
    
    
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
        Debug.Log("youyou");
        if (IsDead())
        {   
            //TODO: death feedback
            Destroy(gameObject);
        }

        return new DamageHitResult(this, gameObject, receivedDamage);
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
