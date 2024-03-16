using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.Enemies.Components
{
    public class TurretSpineRotator : MonoBehaviour
    {
        private TurretMediator _mediator;
        private Transform _playerTransform;
        [SerializeField] private float _speed;
        public void Configure(TurretMediator mediator,Transform playerTransform)
        {
            _mediator = mediator;
            _playerTransform = playerTransform;

        }

        public void LookAtPlayer(float delta)
        {
            Debug.Log("looking at playe wtf " + _playerTransform.position);
            Vector3 targetDirection = _playerTransform.position - transform.position;
            float singleStep = _speed * delta;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}
