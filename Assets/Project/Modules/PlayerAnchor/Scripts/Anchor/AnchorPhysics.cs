using System;
using AYellowpaper;
using Popeye.Modules.Notifiers;
using Project.Modules.PlayerAnchor.Chain;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class AnchorPhysics : MonoBehaviour
    {

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        
        [SerializeField] private ColliderOnlyTriggerNotifierBehaviour _obstacleHitNotifierBehaviour;
        
        
        private IAnchorMediator _anchorMediator;
        


        public void Configure(IAnchorMediator anchorMediator)
        {
            _anchorMediator = anchorMediator;
            
            _rigidbody.transform.localPosition = Vector3.zero;
            _rigidbody.interpolation = RigidbodyInterpolation.None;
            _rigidbody.isKinematic = true;
        }
        

        public void EnableTension()
        {
            _rigidbody.gameObject.SetActive(true);
            _collider.enabled = true;
        }
        
        public void DisableTension()
        {
            _collider.enabled = false;
            _rigidbody.gameObject.SetActive(false);
        }

        public void SubscribeToOnObstacleHit(Action<Collider> callback)
        {
            _obstacleHitNotifierBehaviour.OnEnter += callback;
        }
        public void UnsubscribeToOnObstacleHit(Action<Collider> callback)
        {
            _obstacleHitNotifierBehaviour.OnEnter -= callback;
        }
    }
}