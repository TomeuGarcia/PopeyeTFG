using System;
using UnityEngine;

namespace Popeye.CollisionNotifiers
{
    public class ColliderOnlyTriggerNotifierBehaviour : MonoBehaviour
    {
        [SerializeField] private Collider _collider;

        public Action<Collider> OnEnter;
        public Action<Collider> OnStay;
        public Action<Collider> OnExit;


        private void OnTriggerEnter(Collider otherCollider)
        {
            if (otherCollider.isTrigger) return;
            OnEnter?.Invoke(otherCollider);
        }

        private void OnTriggerStay(Collider otherCollider)
        {
            if (otherCollider.isTrigger) return;
            OnStay?.Invoke(otherCollider);
        }

        private void OnTriggerExit(Collider otherCollider)
        {        
            if (otherCollider.isTrigger) return;
            OnExit?.Invoke(otherCollider);
        }


        public void EnableCollider()
        {
            _collider.enabled = true;
        }
    
        public void DisableCollider()
        {
            _collider.enabled = false;
        }

    }
}