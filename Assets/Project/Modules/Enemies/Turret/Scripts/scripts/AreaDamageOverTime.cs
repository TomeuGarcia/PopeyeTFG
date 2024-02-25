using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Pool;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.CombatSystem;
using Popeye.Scripts.ObjectTypes;
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
    [SerializeField] private ObjectTypeAsset _playerType;
    private float burnTimer = 0;

    
    
    private void Start()
    {
        _contactDamageHit = new DamageHit(_contactDamageConfig);
        _combatManager = ServiceLocator.Instance.GetService<ICombatManager>();
        
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
        Recycle();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (AcceptsOtherCollider(other))
        {
            if (_player == null)
            {
                _player = other.gameObject;
            }
            _standingOnArea = true;
        }
    }

    private bool AcceptsOtherCollider(Collider other)
    {
        if (!other.TryGetComponent(out IObjectType otherObjectType)) return false;
        return otherObjectType.IsOfType(_playerType);
    }
    private void OnTriggerExit(Collider other)
    {
        if (AcceptsOtherCollider(other))
        {
            _standingOnArea = false;
            burnTimer = 0;
        }
    }

    internal override void Init()
    {
        Invoke("Despawn",_lifeTime);
    }

    internal override void Release()
    {
        _standingOnArea = false;
        burnTimer = 0;
    }
}
