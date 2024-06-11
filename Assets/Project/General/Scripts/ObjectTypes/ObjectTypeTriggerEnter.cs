using System;
using UnityEngine;

namespace Popeye.Scripts.ObjectTypes
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class ObjectTypeTriggerEnter : MonoBehaviour
    {
        [Header("CONFIGURATION")] 
        [SerializeField] private bool _disableOnFirstTrigger = true;
        [SerializeField] private ObjectTypeAsset[] _acceptObjectTypes;
        private Collider _collider;

        public delegate void GameObjectInteractsEvent(GameObject otherGameObject);
        public GameObjectInteractsEvent OnGameObjectEnters;


        private void Awake()
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = false;

            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (AcceptsOther(other))
            {
                OnGameObjectEnters?.Invoke(other.gameObject);
                if (_disableOnFirstTrigger)
                {
                    Disable();
                }
            }
        }


        private bool AcceptsOther(Collider other)
        {
            if (!other.TryGetComponent(out IObjectType objectType))
            {
                return false;
            }

            return objectType.IsOfAnyType(_acceptObjectTypes);
        }

        public void Disable()
        {
            _collider.enabled = false;
        }
        public void Enable()
        {
            _collider.enabled = true;
        }
    }
}