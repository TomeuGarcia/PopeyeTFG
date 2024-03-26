using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Scripts.Collisions;
using UnityEngine;

namespace Popeye.Modules.Enemies.Components
{
    public class ShieldedDashing : MonoBehaviour
    {
        private ShieldedMediator _mediator;
        [SerializeField] private float _dashCooldown;
        [SerializeField] private float _telegraphTime;
        [SerializeField] private int _dashingTimeInMillis;
        [SerializeField] private float DashingSpeed;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private CollisionProbingConfig _defaultProbingConfig;
        [SerializeField] private CollisionProbingConfig _canSeePlayerProbingConfig;
        [SerializeField] private Transform _rayCastOriginCenter;
        [SerializeField] private Transform _rayCastOriginLeft;
        [SerializeField] private Transform _rayCastOriginRight;
        private float _coolDownTimer = 0f;
        private bool _dashing = false;
        private Transform _playerTransform;
        public void Configure(ShieldedMediator mediator)
        {
            _mediator = mediator;
        }

        public void SetPlayerTransform(Transform transform)
        {
            _playerTransform = transform;
        }
        private void Update()
        {
            
            if (!_dashing && _mediator.IsChasing())
            {
                if (_coolDownTimer <= _dashCooldown)
                {
                    _coolDownTimer += Time.deltaTime;
                }
                else
                {
                    Vector3 rayCastDirection = (_playerTransform.position - transform.position).normalized;
                    float rayCastDistance = (_playerTransform.position - transform.position).magnitude;
                    RaycastHit hit;
                    if (!Physics.Raycast(transform.position, rayCastDirection, out hit,rayCastDistance, _defaultProbingConfig.CollisionLayerMask,
                            _defaultProbingConfig.QueryTriggerInteraction))
                    {
                        
                            Dash();
                            _coolDownTimer = 0;
                    }
                    
                }
            }
            else if (_dashing)
            {
                if (Physics.Raycast(_rayCastOriginCenter.position, Vector3.down, _defaultProbingConfig.ProbeDistance,
                        _defaultProbingConfig.CollisionLayerMask,
                        _defaultProbingConfig.QueryTriggerInteraction))
                {
                    
                }
                else
                {
                    StopDashing();
                    _mediator.ActivateNavigation();
                }

                if (Physics.Raycast(_rayCastOriginCenter.position, transform.forward, 1f,
                        _defaultProbingConfig.CollisionLayerMask,
                        _defaultProbingConfig.QueryTriggerInteraction) || Physics.Raycast(_rayCastOriginLeft.position, transform.forward, 1f,
                        _defaultProbingConfig.CollisionLayerMask,
                        _defaultProbingConfig.QueryTriggerInteraction) || Physics.Raycast(_rayCastOriginRight.position, transform.forward, 1f,
                        _defaultProbingConfig.CollisionLayerMask,
                        _defaultProbingConfig.QueryTriggerInteraction))
                {
                    _rigidbody.velocity = Vector3.zero;
                    ResetDashingCooldown();
                    _dashing = false;
                    _mediator.Stun();
                }
            }
        }

        public void ResetDashingCooldown()
        {
            _coolDownTimer = 0;
        }

        private void Dash()
        {
            _dashing = true;
            _mediator.StartDashing();
            _mediator.DeactivateNavigation();
            PerformDash();
            
        }

        private async UniTaskVoid PerformDash()
        {
            
            
            //telegraph
            Vector3 targetPosition = new Vector3(_playerTransform.position.x, transform.position.y,
                _playerTransform.position.z);
            
            transform.DOLookAt(targetPosition, _telegraphTime);
            await transform.DOMove(transform.position + (transform.position-targetPosition).normalized *0.4f, _telegraphTime, false).AsyncWaitForCompletion();
            
            
            _rigidbody.velocity = transform.forward * DashingSpeed;

            await UniTask.Delay(_dashingTimeInMillis);
            if (_dashing)
            {
                StopDashing();
                _mediator.ActivateNavigation();
            }
            
            

        }

        private void StopDashing()
        {
            _rigidbody.velocity = Vector3.zero;
            ResetDashingCooldown();
            _mediator.StopDashing();
            _dashing = false;
        }
        private void OnCollisionEnter(Collision other)
        {
           /* if (_dashing)
            {
                Debug.Log("collided with the layer "+ other.gameObject.layer +" and the layer we are loking at is: " +_defaultProbingConfig.CollisionLayerMask);
                if (other.gameObject.layer == _defaultProbingConfig.CollisionLayerMask)
                {
                    Debug.Log("it has default layermask");
                    StopDashing();
                    _mediator.Stun();
                }
            }*/
        }
    }
}
