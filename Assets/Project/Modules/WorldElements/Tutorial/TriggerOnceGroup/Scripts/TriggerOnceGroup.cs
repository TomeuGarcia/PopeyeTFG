using Popeye.Scripts.ObjectTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.WorldElements.Tutorial
{
    [RequireComponent(typeof (Rigidbody))]
    public class TriggerOnceGroup : MonoBehaviour
    {

        [SerializeField] private Collider[] _activationTriggers;
        [SerializeField] private Collider[] _deactivationTriggers;

        [SerializeField] private ObjectTypeAsset _acceptObjectType;

        private bool _hasActivated = false;
        private IWorldTriggerable _worldTriggerable;


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
                    Debug.Log("DeactivateTrigger");
                    DisableTriggers(_deactivationTriggers);
                    _worldTriggerable.Deactivate();
                }
                else
                {
                    Debug.Log("NotDeactivateTrigger");
                }
            }
            else
            {
                if(OtherIsTouchingTriggers(other, _activationTriggers))
                {
                    Debug.Log("ActivateTrigger");
                    DisableTriggers(_activationTriggers);
                    _hasActivated = true;
                    _worldTriggerable.Activate();
                }
                else
                {
                    Debug.Log("NotActivateTrigger");
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

