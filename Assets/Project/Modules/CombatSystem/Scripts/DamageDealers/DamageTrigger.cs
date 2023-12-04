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

        
        [SerializeField] private Collider _collider;
        

        
        public void Configure(ICombatManager combatManager, DamageHit damageHit)
        {
            _damageDealer = new DamageDealer();
            _damageDealer.Configure(combatManager, damageHit);

            _hitTargetsHistory = new HashSet<GameObject>();
            _collider.isTrigger = true;
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
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject.name);
            if (_hitTargetsHistory.Contains(other.gameObject))
            {
                return;
            }

            _damageDealer.UpdateDamageHit(transform.position, transform.forward);
            
            if (_damageDealer.TryDealDamage(other.gameObject, out DamageHitResult damageHitResult))
            {
                _hitTargetsHistory.Add(damageHitResult.DamageHitTargetGameObject);
            }
        }

    }
}