using System;
using System.Collections.Generic;
using Popeye.Modules.PlayerAnchor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.CombatSystem
{
    public class DamageTrigger : MonoBehaviour
    {
        private DamageDealer _damageDealer;
        private HashSet<GameObject> _hitTargetsHistory;

        [SerializeField] private bool _damageTargetsOncePerActivation = false;
        [SerializeField] private bool _isKnockbackPushOrigin = false;
        [SerializeField] private Collider _collider;

        public Vector3 Position => transform.position;
        
        
        public Action<DamageTrigger, GameObject> OnBeforeDamageDealt;
        public Action<DamageHitResult> OnDamageDealt;

        
        private void OnTriggerEnter(Collider other)
        {
            if (_damageTargetsOncePerActivation && _hitTargetsHistory.Contains(other.gameObject))
            {
                return;
            }

            TryDealDamage(other);
        }
        
        
        
        public void Configure(ICombatManager combatManager, DamageHit damageHit = null)
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

        public void UpdateKnockbackEndPosition(Vector3 knockbackEndPosition)
        {
            _damageDealer.UpdateKnockbackEndPosition(knockbackEndPosition);
        }
        public void UpdateDamageKnockbackDirection(Vector3 pushDirection)
        {
            _damageDealer.UpdateKnockbackPushDirection(pushDirection);
        }
        
        private void UpdateDamageKnockbackDirection(Vector3 originPosition, Vector3 targetPosition)
        {
            Vector3 pushDirection = 
                PositioningHelper.Instance.GetDirectionAlignedWithFloor(originPosition, targetPosition);
            
            UpdateDamageKnockbackDirection(pushDirection);
        }
        
        

        private void TryDealDamage(Collider collider)
        {
            _damageDealer.UpdatePosition(Position);
            if (_isKnockbackPushOrigin)
            {
                UpdateDamageKnockbackDirection(Position, collider.transform.position);
            }
            
            OnBeforeDamageDealt?.Invoke(this, collider.gameObject);
            if (_damageDealer.TryDealDamage(collider.gameObject, out DamageHitResult damageHitResult))
            {
                _hitTargetsHistory.Add(damageHitResult.DamageHitTargetGameObject);
                OnDamageDealt?.Invoke(damageHitResult);
            }
        }
        
    }
}