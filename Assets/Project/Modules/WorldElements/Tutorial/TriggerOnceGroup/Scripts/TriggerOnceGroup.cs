using System;
using Popeye.Scripts.ObjectTypes;
using UnityEngine;

namespace Popeye.Modules.WorldElements.Tutorial
{
    [RequireComponent(typeof(Rigidbody))]
    public class TriggerOnceGroup : MonoBehaviour
    {

        [SerializeField] private Collider[] _activationTriggers;
        [SerializeField] private Collider[] _deactivationTriggers;

        [SerializeField] private ObjectTypeAsset _acceptObjectType;

        private bool _hasActivated = false;
        private IWorldTriggerable _worldTriggerable;


        private void Awake()
        {
            foreach (Collider trigger in _activationTriggers)
            {
                trigger.isTrigger = true;
            }
            foreach (Collider trigger in _deactivationTriggers)
            {
                trigger.isTrigger = true;
            }

            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }


        private void OnTriggerEnter(Collider other)
        {

            if (other.TryGetComponent(out ObjectTypeBehaviour objectTypeBehaviour))
            {
                if (!objectTypeBehaviour.IsOfType(_acceptObjectType))
                {
                    return;
                }
            }
            else
            {
                return;
            }



            if(_hasActivated)
            {
                if(OtherIsTouchingTriggers(other, _deactivationTriggers))
                {
                    DisableTriggers(_deactivationTriggers);
                    _worldTriggerable.Deactivate();
                }
            }
            else
            {
                if(OtherIsTouchingTriggers(other, _activationTriggers))
                {
                    DisableTriggers(_activationTriggers);
                    _hasActivated = true;
                    _worldTriggerable.Activate();
                }

            }
        }


        public void Init(IWorldTriggerable worldTriggerable)
        {
            _worldTriggerable = worldTriggerable;
        }

        private bool OtherIsTouchingTriggers(Collider other, Collider[] triggers)
        {
            foreach (Collider trigger in triggers) 
            {
                if(trigger.bounds.Intersects(other.bounds)) return true;
            }

            return false;
        }

        private void DisableTriggers(Collider[] triggers)
        {
            foreach(Collider trigger in triggers)
            {
                trigger.enabled = false;
            }
        }
    }

}

