using System;
using UnityEngine;

namespace Project.PhysicsMovement
{
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicsMovementBehaviour : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        private Vector3 _movementDirection = Vector3.forward;
        private Vector3 _desiredVelocity;
        

        public float MovementSpeed { get; set; } = 5f;

        public Vector3 MovementDirection
        {
            get => _movementDirection;
            set => _movementDirection = value.normalized;
        } 

        
        

        private void Update()
        {
            _desiredVelocity = MovementDirection * MovementSpeed;
            _rigidbody.rotation = Quaternion.LookRotation(MovementDirection, Vector3.up);
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = _desiredVelocity;
        }


        public void UseGravity(bool useGravity)
        {
            _rigidbody.useGravity = useGravity;
        }
        
    }
    
}