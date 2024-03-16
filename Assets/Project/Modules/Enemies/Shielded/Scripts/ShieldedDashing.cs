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
        [SerializeField] private float _dashingTimeInSeconds;
        [SerializeField] private float _dashingForce;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private CollisionProbingConfig _defaultProbingConfig;
        [SerializeField] private CollisionProbingConfig _playerProbingConfig;
        [SerializeField] private Transform _rayCastOrigin;
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
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, rayCastDirection, out hit,_playerProbingConfig.ProbeDistance, _playerProbingConfig.CollisionLayerMask,
                            _defaultProbingConfig.QueryTriggerInteraction))
                    {
                        if (hit.transform.gameObject.layer != _defaultProbingConfig.CollisionLayerMask)
                        {
                            _dashing = true;
                            Dash();
                        }
                        _coolDownTimer = 0;
                    }
                    
                }
            }
        }

        private void Dash()
        {
            _mediator.StartDashing();
            
            PerformDash();
            
        }

        private async UniTaskVoid PerformDash()
        {
            Vector3 dashEndPosition;
            Vector3 rayCastDirection = (_playerTransform.position - transform.position).normalized;
            Vector3 direction = (_playerTransform.position+Vector3.down - transform.position).normalized;
            RaycastHit hit;
            if (Physics.Raycast(_rayCastOrigin.position, rayCastDirection, out hit,_defaultProbingConfig.ProbeDistance, _defaultProbingConfig.CollisionLayerMask,
                    _defaultProbingConfig.QueryTriggerInteraction))
            {
                
                dashEndPosition = hit.point-direction;
            }
            else
            {
                dashEndPosition = transform.position + direction * _defaultProbingConfig.ProbeDistance;
            }

            dashEndPosition = new Vector3(dashEndPosition.x, transform.position.y, dashEndPosition.z);
            float distance = Vector3.Distance(dashEndPosition, transform.position);
            float duration = (_dashingTimeInSeconds * distance) / _defaultProbingConfig.ProbeDistance;
            _mediator.DeactivateNavigation();
            
            //telegraph
            await transform.DOMove(transform.position + (transform.position-dashEndPosition).normalized *0.4f, 0.5f, false).AsyncWaitForCompletion();
            //dash
            await transform.DOMove(dashEndPosition, duration, false).AsyncWaitForCompletion();
            
            _rigidbody.velocity = Vector3.zero;
            _mediator.ActivateNavigation();
            _coolDownTimer = 0;
            _mediator.StopDashing();
            _dashing = false;
            

        }
    }
}
