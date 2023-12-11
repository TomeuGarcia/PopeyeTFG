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
        }
        

        public void DisableAllPhysics()
        {
            _collider.enabled = false;
            _rigidbody.interpolation = RigidbodyInterpolation.None;
            SetImmovable();
        }
        
        public void EnableAllPhysics()
        {
            _collider.enabled = true;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            SetMovable();
        }
        
        
        public void SetMovable()
        {
            _rigidbody.isKinematic = false;
        }
        public void SetImmovable()
        {
            _rigidbody.isKinematic = true;
        }
        
    }
}