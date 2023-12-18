using System;
using Project.Modules.PlayerAnchor.Chain;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class AnchorPhysics : MonoBehaviour
    {

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;

        private IAnchorMediator _anchorMediator;



        public void Configure(IAnchorMediator anchorMediator)
        {
            _anchorMediator = anchorMediator;
            
            _rigidbody.transform.localPosition = Vector3.zero;
            _rigidbody.interpolation = RigidbodyInterpolation.None;
            _rigidbody.isKinematic = true;
        }
        

        public void DisableAllPhysics()
        {
            _collider.enabled = false;
            _rigidbody.gameObject.SetActive(false);
        }
        
        public void EnableAllPhysics()
        {
            _rigidbody.gameObject.SetActive(true);
            _collider.enabled = true;
        }
        
    }
}