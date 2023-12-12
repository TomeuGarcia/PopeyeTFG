using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.ValueStatSystem;
using Project.Modules.CombatSystem;
using UnityEngine;

public class StaticEnemyHealth : MonoBehaviour, IDamageHitTarget
{
    private HealthSystem _healthSystem;
    [SerializeField, Range(0.0f, 100)] private int _maxHealth = 50;

    void Awake()
    {
        _healthSystem = new HealthSystem(_maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void TakeHit(DamageHit anchorHit)
    {
        _healthSystem.TakeDamage(anchorHit.Damage);
        if (_healthSystem.IsDead())
        {
            Destroy(transform.parent.gameObject);
        }
    }

    public DamageHitTargetType GetDamageHitTargetType()
    {
        throw new System.NotImplementedException();
    }

    public DamageHitResult TakeHitDamage(DamageHit damageHit)
    {
        throw new System.NotImplementedException();
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
