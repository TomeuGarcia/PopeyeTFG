using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Pool;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.CombatSystem;
using UnityEngine;

public class AreaDamageOverTime : RecyclableObject
{
    
    private DamageHit _contactDamageHit;
    [SerializeField] private DamageHitConfig _contactDamageConfig;

    private ICombatManager _combatManager;

    private bool _standingOnArea = false;
    private GameObject _player;

    [SerializeField] private float _burnRate = 0.3f;
    [SerializeField] private float _lifeTime = 0.3f;
    private float burnTimer = 0;

    
    
    private void Start()
    {
        _contactDamageHit = new DamageHit(_contactDamageConfig);
        _combatManager = ServiceLocator.Instance.GetService<ICombatManager>();
        Invoke("Despawn",_lifeTime);
    }

    private void Update()
    {
        if (_standingOnArea == true)
        {
            if (burnTimer >= _burnRate)
            {
                burnTimer = 0;
                _combatManager.TryDealDamage(_player, _contactDamageHit, out DamageHitResult damageHitResult);
            }
            burnTimer += Time.deltaTime;
        }
    }

    private void Despawn()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (_player == null)
            {
                _player = other.gameObject;
            }
            _standingOnArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _standingOnArea = false;
            burnTimer = 0;
        }
    }

    internal override void Init()
    {
        
    }

    internal override void Release()
    {
        _standingOnArea = false;
        burnTimer = 0;
    }
}
