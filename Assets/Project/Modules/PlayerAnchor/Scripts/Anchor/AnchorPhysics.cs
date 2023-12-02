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


        private void OnCollisionEnter(Collision other)
        {
            _anchorMediator.OnCollisionWithObstacle(other);
        }


        public void Configure(IAnchorMediator anchorMediator)
        {
            _anchorMediator = anchorMediator;
        }
        

        public void DisableAllPhysics()
        {
            _collider.enabled = false;
            UseGravity(false);
            _rigidbody.interpolation = RigidbodyInterpolation.None;
            SetImmovable();
        }
        
        public void EnableAllPhysics()
        {
            _collider.enabled = true;
            UseGravity(true);
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            SetMovable();
        }


        public void UseGravity(bool useGravity)
        {
            _rigidbody.useGravity = useGravity;

            _rigidbody.drag = useGravity ? 0 : float.MaxValue;
            _rigidbody.mass = useGravity ? 3 : float.MaxValue;
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