using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.CollisionNotifiers
{
    public class TriggerNotifierBehaviour : MonoBehaviour
    {
        [SerializeField] private Collider _collider;


        public Action<Collider> OnEnter;
        public Action<Collider> OnStay;
        public Action<Collider> OnExit;
        

        private void OnTriggerEnter(Collider otherCollider)
        {
            OnEnter?.Invoke(otherCollider);
        }

        private void OnTriggerStay(Collider otherCollider)
        {
            OnStay?.Invoke(otherCollider);
        }

        private void OnTriggerExit(Collider otherCollider)
        {        
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

