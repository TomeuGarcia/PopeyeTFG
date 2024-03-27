using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class AnchorPhysics : MonoBehaviour
    {

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        
        
        
        private IAnchorMediator _anchorMediator;
        


        public void Configure(IAnchorMediator anchorMediator)
        {
            _anchorMediator = anchorMediator;
            
            _rigidbody.interpolation = RigidbodyInterpolation.None;
            _rigidbody.isKinematic = true;
        }
        

        public void EnableCollision()
        {   
            /*
            _rigidbody.gameObject.SetActive(true);
            _collider.enabled = true;
            */
        }
        
        public void DisableCollision()
        {
            // Buttons stop working if uncommented
            /* 
            _collider.enabled = false;
            _rigidbody.gameObject.SetActive(false);
            */
        }


    }
}