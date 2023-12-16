using System;
using System.Collections.Generic;
using Project.Modules.PlayerAnchor;
using UnityEngine;

namespace Project.Modules.CombatSystem
{
    public class DamageTrigger : MonoBehaviour
    {
        private DamageDealer _damageDealer;
        private HashSet<GameObject> _hitTargetsHistory;

        [SerializeField] private bool _damageTargetsOncePerActivation = false;
        [SerializeField] private Collider _collider;

        public Vector3 Position => transform.position;
        
        
        public Action<DamageTrigger, GameObject> OnBeforeDamageDealt;
        public Action<DamageHitResult> OnDamageDealt;

        
        public void Configure(ICombatManager combatManager, DamageHit damageHit)
        {
            _damageDealer = new DamageDealer();
            _damageDealer.Configure(combatManager, damageHit);

            _hitTargetsHistory = new HashSet<GameObject>();
            _collider.isTrigger = true;
        }

        public void SetDamageHit(DamageHit damageHit)
        {
            _damageDealer.SetDamageHit(damageHit);
        }
        
        public void Activate()
        {
            _hitTargetsHistory.Clear();
            _collider.enabled = true;
        } 
        public void Deactivate()
        {
            _collider.enabled = false;
        }

        public void UpdateDamageKnockbackDirection(Vector3 knockbackDirection)
        {
            _damageDealer.UpdateKnockbackDirection(knockbackDirection);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (_damageTargetsOncePerActivation && _hitTargetsHistory.Contains(other.gameObject))
            {
                return;
            }

            _damageDealer.UpdatePosition(transform.position);
            
            OnBeforeDamageDealt?.Invoke(this, other.gameObject);
            if (_damageDealer.TryDealDamage(other.gameObject, out DamageHitResult damageHitResult))
            {
                _hitTargetsHistory.Add(damageHitResult.DamageHitTargetGameObject);
                OnDamageDealt?.Invoke(damageHitResult);
            }
        }

    }
}