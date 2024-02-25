using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerController.Inputs;
using Popeye.Modules.PlayerController.LookRotation;
using Popeye.Scripts.Collisions;
using UnityEngine;



using UnityEngine.Serialization;


namespace Popeye.Modules.PlayerController
{
    public class PlayerController : MonoBehaviour
    {
 
      

        // Input
        public IMovementInputHandler MovementInputHandler { get; set; }
        public IInputCorrector InputCorrector { get; set; }
        private Vector3 _movementInput;
        private Vector3 _lookInput;
        private Vector3 _movementDirection;

        [Header("COMPONENTS")]
        [SerializeField] private Rigidbody _rigidbody;
        public Rigidbody Rigidbody => _rigidbody;
        public Vector3 Position => _rigidbody.position;
        public Transform Transform => _rigidbody.transform;
        public Transform LookTransform => _lookTransform;



        [Header("LOOK")] 
        [SerializeField] public bool useLookInput = true;
        [SerializeField] private Transform _lookTransform;
        [SerializeField] private OverTimeLookRotationUpdater.Configuration _lookOverTimeConfig;
        
        
        public Vector3 LookDirection => _lookTransform.forward;
        public Vector3 RightDirection => _lookTransform.right;
        public bool CanRotate { get; set; }

        private ILookRotationUpdater _currentLookRotationUpdater;
        private ILookRotationUpdater _instantLookRotationUpdater;
        private ILookRotationUpdater _overTimeLookRotationUpdater;
        


        [Header("VELOCITY")]
        [SerializeField, Range(0.0f, 100.0f)] private float _maxSpeed = 10.0f;

        private Vector3 _velocity;
        private Vector3 _desiredVelocity;

        public float MaxSpeed
        {
            get { return _maxSpeed; }
            set { _maxSpeed = value; }
        }

        public float CurrentSpeedXZ
        {
            get
            {
                Vector2 velocityXZ = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.z);
                return velocityXZ.magnitude;
            }
        }

        public float CurrentSpeedY => Mathf.Abs(_rigidbody.velocity.y);

        [Header("ACCELERATION")] 
        [SerializeField, Range(0.0f, 100.0f)] private float _maxAcceleration = 10.0f;

        [SerializeField, Range(0.0f, 100.0f)] private float _maxAirAcceleration = 5.0f;
        [SerializeField, Range(0.0f, 100.0f)] private float _airFallAcceleration = 10.0f;

        [Header("GROUND")]
        [SerializeField] private LayerMask _groundProbeMask = -1;


        [Tooltip("Used to define ground contacts, not to limit movement slopes (use _maxAirAcceleration for that)")]
        [SerializeField, Range(0.0f, 90.0f)] private float _maxGroundAngle;

        private float _minGroundDotProduct;
        [SerializeField, Range(0.0f, 100.0f)] private float _groundSnapBreakSpeed = 100.0f;
        [SerializeField, Range(0.0f, 10.0f)] private float _groundProbeDistance = 1.5f;
        private const float SPEED_COMPARISON_THRESHOLD = 0.2f;

        [Header("STAIRS")] [SerializeField] private LayerMask _stairsProbeMask = -1;
        [SerializeField, Range(0.0f, 90.0f)] private float _maxStairsAngle = 50.0f;
        private float _minStairsDotProduct;


        [Header("LEDGE")] 
        [SerializeField] private bool _checkLedges = false;
        [SerializeField] private CollisionProbingConfig _ledgeGroundCollisionProbingConfig;
        [SerializeField] private LedgeDetectionConfig _ledgeDetectionConfig;
        private LedgeDetectionController _ledgeDetectionController;



        private Vector3 _contactNormal;
        public Vector3 ContactNormal => _contactNormal;
        public Vector3 GroundNormal { get; private set; }
        private int _groundContactCount;

        private bool OnGround => _groundContactCount > 0;

        private Vector3 _steepNormal;
        private int _steepContactCount;
        private bool OnSteep => _steepContactCount > 0;

        private int _stepsSinceLastGrounded;


        private void OnValidate()
        {
            _minGroundDotProduct = Mathf.Cos(_maxGroundAngle * Mathf.Deg2Rad);
            _minStairsDotProduct = Mathf.Cos(_maxStairsAngle * Mathf.Deg2Rad);

            // Eliminate inconsistent _groundSnapBreakSpeed float precision
            if (Mathf.Abs(_maxSpeed - _groundSnapBreakSpeed) > SPEED_COMPARISON_THRESHOLD)
            {
                _groundSnapBreakSpeed = _maxSpeed + SPEED_COMPARISON_THRESHOLD;
            }
        }

        public void AwakeConfigure()
        {
            OnValidate();
            
            if (MovementInputHandler == null)
            {
                MovementInputHandler = new WorldAxisMovementInput();
            }

            if (InputCorrector == null)
            {
                InputCorrector = new DefaultInputCorrector();
            }

            _ledgeDetectionController =
                new LedgeDetectionController(_ledgeDetectionConfig, _ledgeGroundCollisionProbingConfig);

            CanRotate = true;

            _instantLookRotationUpdater = new InstantLookRotationUpdater(_lookTransform);
            _overTimeLookRotationUpdater = new OverTimeLookRotationUpdater(_lookTransform, _lookOverTimeConfig);
            SetOverTimeRotationMode();
        }

        private void Update()
        {
            _movementInput = MovementInputHandler.GetMovementInput();

            _lookInput = useLookInput ? MovementInputHandler.GetLookInput() : Vector3.zero;
            _movementDirection = _movementInput;
            
            _desiredVelocity = _movementDirection * _maxSpeed;
        }
        
        private void FixedUpdate()
        {
            if (_checkLedges && _movementInput.sqrMagnitude > 0.01f)
            {
                _movementDirection = _ledgeDetectionController.
                    UpdateMovementDirectionFromMovementInput(Position, _movementInput);
                
                _desiredVelocity = _movementDirection * _maxSpeed;
            }
            
            UpdateState();
            AdjustVelocity();
            
            _rigidbody.velocity = _velocity;

            ClearState();
        }

        private void LateUpdate()
        {
            if (CanRotate)
            {
                UpdateLookTransform();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            EvaluateCollision(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            EvaluateCollision(collision);
        }

        private void EvaluateCollision(Collision collision)
        {
            for (int i = 0; i < collision.contactCount; ++i)
            {
                Vector3 normal = collision.GetContact(i).normal;
                float minDot = GetGroundCollisionMinDot(collision.gameObject.layer);
                if (normal.y >= minDot)
                {
                    ++_groundContactCount;
                    _contactNormal += normal;
                }
                else if (normal.y > -0.01f)
                {
                    _steepContactCount += 1;
                    _steepNormal += normal;
                }
            }
        }


        private void UpdateState()
        {
            _stepsSinceLastGrounded += 1;
            _velocity = _rigidbody.velocity;

            if (CheckSnapToGround() || OnGround || CheckSteepContacts())
            {
                _stepsSinceLastGrounded = 0;
                if (_groundContactCount > 1)
                {
                    _contactNormal.Normalize();
                }
            }
            else
            {
                _contactNormal = Vector3.up;
            }
        }

        private void ClearState()
        {
            _groundContactCount = _steepContactCount = 0;
            _contactNormal = _steepNormal = Vector3.zero;
        }


        private void AdjustVelocity()
        {
            Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
            Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

            float currentX = Vector3.Dot(_velocity, xAxis);
            float currentZ = Vector3.Dot(_velocity, zAxis);

            float acceleration = OnGround ? _maxAcceleration : _maxAirAcceleration;

            float maxSpeedChange = acceleration * Time.deltaTime;

            float newX = Mathf.MoveTowards(currentX, _desiredVelocity.x, maxSpeedChange);
            float newZ = Mathf.MoveTowards(currentZ, _desiredVelocity.z, maxSpeedChange);

            _velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);

            if (!OnGround)
            {
                _velocity += Vector3.down * (_airFallAcceleration * Time.deltaTime);
            }
        }

        private bool CheckSnapToGround()
        {
            if (_stepsSinceLastGrounded > 1)
            {
                return false;
            }

            if (!Physics.Raycast(_rigidbody.position, Vector3.down, out RaycastHit hit, _groundProbeDistance,
                    _groundProbeMask))
            {
                GroundNormal = Vector3.up;
                return false;
            }
            GroundNormal = hit.normal;


            float speed = _velocity.magnitude;
            if (speed > _groundSnapBreakSpeed)
            {
                return false;
            }

            if (hit.normal.y < GetGroundCollisionMinDot(hit.collider.gameObject.layer))
            {
                return false;
            }

            _groundContactCount = 1;
            _contactNormal = hit.normal;
            float dot = Vector3.Dot(_velocity, hit.normal);
            if (dot > 0.0f)
            {
                // When colliding against a wall
                _velocity = (_velocity - hit.normal * dot).normalized * speed;
            }

            return true;
        }

        private bool CheckSteepContacts()
        {
            if (_steepContactCount > 1)
            {
                _steepNormal.Normalize();
                if (_steepNormal.y >= _minGroundDotProduct)
                {
                    _groundContactCount = 1;
                    _contactNormal = _steepNormal;
                    return true;
                }
            }

            return false;
        }
        

        private Vector3 ProjectOnContactPlane(Vector3 vector)
        {
            return ProjectOnPlane(vector, _contactNormal);
        }

        private Vector3 ProjectOnPlane(Vector3 vector, Vector3 planeNormal)
        {
            return vector - (planeNormal * Vector3.Dot(vector, planeNormal));
        }

        private float GetGroundCollisionMinDot(int layer)
        {
            return (_stairsProbeMask & (1 << layer)) == 0 ? _minGroundDotProduct : _minStairsDotProduct;
        }


        private void UpdateLookTransform()
        {
            bool usingLookInput = _lookInput.sqrMagnitude > 0.1f;
            Vector3 lookDirection = usingLookInput ? _lookInput : _movementInput;

            if (lookDirection.sqrMagnitude < 0.01f) return;

            if (usingLookInput)
            {
                lookDirection = InputCorrector.CorrectLookInput(lookDirection,
                    MovementInputHandler.ForwardAxis, MovementInputHandler.RightAxis);
            }
            
            Quaternion goalRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            _currentLookRotationUpdater.UpdateLocalRotation(goalRotation);
        }

        public void LookTowardsPosition(Vector3 lookPosition)
        {
            Vector3 lookDirection = ProjectOnPlane(lookPosition - _rigidbody.position, Vector3.up).normalized;

            _lookTransform.localRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        }


        public void GetPushed(Vector3 pushForce)
        {
            _rigidbody.AddForce(pushForce, ForceMode.Impulse);
        }


        public void ResetRigidbody()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
        
        public async UniTaskVoid DisableForDuration(float duration)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;
            enabled = false;
            
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            
            _rigidbody.useGravity = true;
            _rigidbody.isKinematic = false;
            enabled = true;
        }


        public Vector3 GetFloorAlignedLookDirection()
        {
            return ProjectOnPlane(LookDirection, GroundNormal).normalized;
        }


        public void SetCheckLedges(bool checkLedges, bool checkingIgnoreLedges)
        {
            _checkLedges = checkLedges;
            _ledgeDetectionController.SetCheckingIgnoreLedges(checkingIgnoreLedges);
        }

        public void SetInstantRotationMode()
        {
            _currentLookRotationUpdater = _instantLookRotationUpdater;
        }
        
        public void SetOverTimeRotationMode()
        {
            _currentLookRotationUpdater = _overTimeLookRotationUpdater;
        }
        

    }
}