using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class AnchorPhysics : MonoBehaviour
    {

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private SphereCollider _collider;

        private float _originalRadius;
        
        
        private IAnchorMediator _anchorMediator;
        


        public void Configure(IAnchorMediator anchorMediator)
        {
            _anchorMediator = anchorMediator;
            
            _rigidbody.interpolation = RigidbodyInterpolation.None;
            _rigidbody.isKinematic = true;

            _originalRadius = _collider.radius;
        }
        

        public void EnableCollision()
        {   
            _collider.radius = _originalRadius;
            /*
            _rigidbody.gameObject.SetActive(true);
            _collider.enabled = true;
            */
        }
        
        public void DisableCollision()
        {
            _collider.radius = 0.01f;

            // Buttons stop working if uncommented
            /* 
            _collider.enabled = false;
            _rigidbody.gameObject.SetActive(false);
            */
        }


    }
}