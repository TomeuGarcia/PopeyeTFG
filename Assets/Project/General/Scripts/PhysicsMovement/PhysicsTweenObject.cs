using Popeye.Timers;
using UnityEngine;

namespace Project.PhysicsMovement
{
    public class PhysicsTweenObject
    {
        private readonly Rigidbody _rigidbody;
        private readonly Timer _timer;
        private readonly Vector3 _startVelocity;
        private readonly Vector3 _acceleration;
        private readonly BodyState _initialBodyState;

        private struct BodyState
        {
            public BodyState(Rigidbody rigidbody)
            {
                _drag = rigidbody.drag;
                _useGravity = rigidbody.useGravity;
                _isKinematic = rigidbody.isKinematic;
            }

            public void ApplyBodyState(Rigidbody rigidbody)
            {
                rigidbody.drag = _drag;
                rigidbody.useGravity = _useGravity;
                rigidbody.isKinematic = _isKinematic;
            }

            private float _drag;
            private bool _useGravity;
            private bool _isKinematic;
        }
        

        public PhysicsTweenObject(Rigidbody rigidbody, float duration, 
            Vector3 startPosition, Vector3 endPosition)
        {
            _rigidbody = rigidbody;
            _timer = new Timer(duration);
                
            _acceleration = (endPosition - startPosition) / (-(0.5f * (duration * duration)));
            _startVelocity = -_acceleration * duration;
                
            // X = Xo + Vo*t + 1/2*a*t^2
            // V = Vo + a*t
                
            // X = Xo + (V - a*t)*t + 1/2*a*t^2
            // X = Xo + V*t - a*t^2 + 1/2*a*t^2
            // X = Xo + V*t - 1/2*a*t^2
            // X - Xo - V*t = - 1/2*a*t^2
            // (X - Xo - V*t) / (- 1/2*t^2) = a
            
            // a = (X - Xo - V) / (-t + 1/2*t^2)

            _initialBodyState = new BodyState(_rigidbody);
            SetupRigidbodyForTweening();
        }
            
        public bool FixedUpdate(float fixedDeltaTime)
        {
            Vector3 currentVelocity = _startVelocity + _acceleration * _timer.Time;
            _rigidbody.velocity = currentVelocity;
                
            _timer.Update(fixedDeltaTime);
            return _timer.HasFinished();
        }

        public bool WasDestroyed()
        {
            return _rigidbody == null;
        }
        
        public void OnTweeningCompleted()
        {
            _initialBodyState.ApplyBodyState(_rigidbody);
        }
        
        private void SetupRigidbodyForTweening()
        {
            _rigidbody.drag = 0;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = false;
        }
        
    }
}