using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.CombatSystem;
using UnityEngine;

public class DealDamageScript : MonoBehaviour
{
    private DamageHit _contactDamageHit;
    [SerializeField] private Transform _transform;
    [SerializeField] private float _contactHitDamageAmount;
    [SerializeField] private float _contactHitStunDuration;
    [SerializeField] private float _contactHitKnockbackForce;

    private void Awake()
    {
        /*
        _contactDamageHit = new DamageHit(CombatManager.Instance.DamageOnlyPlayerPreset,
            _contactHitDamageAmount, _contactHitKnockbackForce, _contactHitStunDuration);
            */
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        _contactDamageHit.Position = _transform.position;
        _contactDamageHit.KnockbackDirection = PositioningHelper.Instance.GetDirectionAlignedWithFloor(_transform.position, other.transform.position);
        CombatManager.Instance.TryDealDamage(other.gameObject, _contactDamageHit, out DamageHitResult damageHitResult);
        */
    }
}
