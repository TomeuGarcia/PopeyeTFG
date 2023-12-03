using System;
using Project.Modules.PlayerAnchor;
using UnityEngine;

namespace Project.Modules.CombatSystem
{
    public class DamageTrigger : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        private DamageDealer _damageDealer;

        
        private void Awake()
        {
            _damageDealer = new DamageDealer();
            _collider.isTrigger = true;
        }

        public void Configure(ICombatManager combatManager, DamageHit damageHit)
        {
            _damageDealer.Configure(combatManager, damageHit);
        }

        public void Activate()
        {
            _collider.enabled = true;
        } 
        public void Deactivate()
        {
            _collider.enabled = false;
        } 
        
        private void OnTriggerEnter(Collider other)
        {
            _damageDealer.TryDealDamage(other.gameObject, out DamageHitResult damageHitResult); //TODO
        }

    }
}