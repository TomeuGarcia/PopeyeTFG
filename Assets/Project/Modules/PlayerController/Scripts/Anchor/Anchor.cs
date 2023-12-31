using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Anchor : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] private Transform _anchorTransform;
    [SerializeField] private Transform _anchorMeshTransform;
    [SerializeField] private Transform _anchorBindLineOrigin;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private SpringJoint _springJoint;
    [SerializeField] private LineRenderer _trajectoryLine;
    [SerializeField] private LineRenderer _ownerBinderLine;
    [SerializeField] private Collider _hitTrigger;
    [SerializeField] private SphereCollider _collider;
    [SerializeField] public AnchorDamageDealer _anchorDamageDealer;
    [SerializeField] private LayerMask _obstacleLayers;
    [SerializeField] private GroundedAnchor _groundedAnchor;
    [SerializeField] private AnchorSnapper _anchorSnapper;
    [SerializeField] private AnchorHealthDrainer _anchorHealthDrainer;
    [SerializeField] private AnchorElectricChain _anchorElectricChain;
    public AnchorElectricChain AnchorElectricChain => _anchorElectricChain;

    private bool _anchorIsBeingSnapped;

    [Header("STAMINA")]
    [SerializeField] private ValueStatBar _staminaBar;
    [SerializeField, Range(0.0f, 500.0f)] private float _requiredDrainedHealth = 150.0f;
    private StaminaSystem _staminaSystem;

    [Header("NEW TRAJECTORY")]
    [SerializeField] private LineRenderer _maxForceTrajectory;
    [SerializeField] private LineRenderer _minForceTrajectory;
    [SerializeField] private LineRenderer _currentTrajectoryDebug;
    private Vector3[] _trajectoryPathPoints;
    [SerializeField, Range(0.0f, 10.0f)] float _maxThrowDuration = 0.6f;
    [SerializeField, Range(0.0f, 10.0f)] float _minThrowDuration = 0.3f;
    [SerializeField] private AnimationCurve _forceCurveNewTrajectory;
    [SerializeField] private Transform _anchorTrajectoryEndSpot;
    private bool _drawDebugLine;

    [Header("OWNER")]
    [SerializeField] private Transform _ownerTransform;
    [SerializeField] private Vector3 _grabbedPosition = new Vector3(1.0f, 0.0f, 0.5f);
    [SerializeField] private Vector3 _grabbedRotation = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private Vector3 _aimingPosition = new Vector3(1.0f, 0.0f, 0.5f);
    [SerializeField] private Vector3 _aimingRotation = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private Vector3 _launchDirectionOffset = new Vector3(0.0f, 1.0f, 0.0f);
    public (Vector3, Vector3) ChainExtremes => (_anchorBindLineOrigin.position, _ownerTransform.position);

    [Header("PULL")]
    [SerializeField, Range(0.0f, 20.0f)] private float _pulledTowardsOwnerAccelearation = 2.0f;
    private bool _isBeingPulled = false;

    [Header("PULL ATTACK")]
    [SerializeField, Range(0, 1)] float _pullAttackArchLerp = 0;
    [SerializeField, Range(0f, 180f)] float _canPullAttackAngleThreshold = 20f;

    [Header("FORCE")]
    [SerializeField] private AnimationCurve _forceCurve;
    [SerializeField, Range(0.0f, 100.0f)] private float _maxForce = 20.0f;
    private Vector3 _velocity;
    private float _throwStrength01;

    [Header("ABILITIES")]
    [SerializeField, Range(0.0f, 500.0f)] private float _explosionStamina = 100.0f;


    public Vector3 OwnerPosition => _ownerTransform.position;
    public Vector3 Position => _anchorTransform.position;
    private Vector3 _throwStartPosition;

    public Transform Transform => _anchorTransform;

    private bool _canMeleeAttack = true;
    public bool CanMeleeAttack
    {
        get { return _canMeleeAttack; }
        set { _canMeleeAttack = value; }
    }


    public float MinDistanceFromOwner => _springJoint.minDistance;
    public float MaxDistanceFromOwner => _springJoint.maxDistance;
    public float CurrentDistanceFromOwner => Vector3.Distance(Position, _ownerTransform.position);

    private bool _alreadyCollidedWithWall;



    public enum AnchorStates
    {
        WithOwner,
        OnAir,
        OnGround
    }

    private AnchorStates _currentState;




    public void Awake()
    {
        RespawnReset();

        _trajectoryPathPoints = new Vector3[_maxForceTrajectory.positionCount];

        _staminaSystem = new StaminaSystem(_requiredDrainedHealth, 0.0f);
        _staminaBar.Init(_staminaSystem);
        _anchorHealthDrainer.AwakeInit(_staminaSystem);

        _drawDebugLine = true;
    }

    private void Start()
    {
        _anchorElectricChain.StartInit(this);
    }

    private void Update()
    {
        UpdateOwnerBinderLine();

        //DrawTrajectory(_velocity);
        //ShowTrajectory();

        if (Input.GetKeyDown(KeyCode.L))
        {
            _drawDebugLine = !_drawDebugLine;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsOnGround()) return;


        _rigidbody.DOKill();

        Vector3 normal = collision.contacts[0].normal;
        if (Vector3.Dot(normal, Vector3.up) < 0.5f)
        {
            SnapToFloor();
            _anchorDamageDealer.DealGroundHitDamage(Position, _throwStrength01);
            _alreadyCollidedWithWall = true;
            return;
        }


        SetStill();
        ChangeState(AnchorStates.OnGround);

        if (!_alreadyCollidedWithWall)
        {
            _anchorDamageDealer.DealGroundHitDamage(Position, _throwStrength01);
        }        
        
        _groundedAnchor.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (IsOnAir())
        {
            _anchorDamageDealer.DealThrowHitDamage(otherCollider.gameObject, Position);
        }       
        else if (_isBeingPulled)
        {            
            //_anchorDamageDealer.DealPullBackDamage(otherCollider.gameObject, _ownerTransform.position);            
        }
    }

    public void RespawnReset()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        _springJoint.axis = Vector3.Scale(_springJoint.axis, Vector3.one);
        _rigidbody.DOKill();

        InstantReturnToOwner();
        HideTrajectory();

        SetStill();
    }

    public void InstantReturnToOwner()
    {
        ParentToOwner();
        _anchorTransform.localPosition = _grabbedPosition;
        _anchorTransform.localRotation = Quaternion.identity;
        _anchorMeshTransform.localRotation = Quaternion.Euler(_grabbedRotation);

        ChangeState(AnchorStates.WithOwner);
    }
    
    public void ReturnToOwner()
    {
        SetStill();
        _groundedAnchor.gameObject.SetActive(false);
        ParentToOwner();
        SetGrabbedPosition();

        ChangeState(AnchorStates.WithOwner);
    }

    public void ParentToOwner()
    {
        _anchorTransform.SetParent(_ownerTransform);
    }
    public void UnparentFromOwner()
    {
        _anchorTransform.SetParent(null);
    }

    public void GetPulledTowardsOwner()
    {
        Vector3 direction = (_springJoint.connectedBody.position - _rigidbody.position).normalized;
        _rigidbody.AddForce(direction * _pulledTowardsOwnerAccelearation, ForceMode.Acceleration);
    }

    public void GetThrown(float strength01, Vector3 lookDirection)
    {
        CorrectTrajectory(strength01, lookDirection);

        SetAimingPositionInstantly();
        UnparentFromOwner();

        LaunchAnchor();


        ChangeState(AnchorStates.OnAir);
    }

    public void SpinAttackFinish()
    {
        SetMovable();
        ChangeState(AnchorStates.OnAir);        

        SnapToFloor();
    }
    public void SpinAttackFinishTired()
    {
        SetMovable();
        ChangeState(AnchorStates.OnAir);
        _alreadyCollidedWithWall = true;

        SnapToFloor();
    }

    public void SnapToFloor()
    {
        if (Physics.Raycast(Position, Vector3.down, out RaycastHit hit, 1000f, _obstacleLayers, QueryTriggerInteraction.Ignore))
        {
            float duration = hit.distance * 0.2f;
            Vector3 snapPosition = hit.point + Vector3.up * 0.2f;
            _rigidbody.DOMove(snapPosition, duration);
        }
    }
    
    
    public void ComputeTrajectory(float strength01, Vector3 lookDirection)
    {
        _throwStrength01 = strength01;

        lookDirection += (_ownerTransform.rotation * _launchDirectionOffset).normalized;
        lookDirection.Normalize();

        float curveStrength01 = _forceCurve.Evaluate(strength01);
        _velocity = lookDirection * curveStrength01 * _maxForce;
        _throwStartPosition = Position;
        DrawTrajectory(_velocity);



        curveStrength01 = _forceCurveNewTrajectory.Evaluate(_throwStrength01);
        _currentTrajectoryDebug.positionCount = _maxForceTrajectory.positionCount;
        for (int i = 0; i < _maxForceTrajectory.positionCount; ++i)
        {
            Vector3 localPosition = Vector3.Lerp(_minForceTrajectory.GetPosition(i), _maxForceTrajectory.GetPosition(i), curveStrength01);
            Vector3 trajectoryPosition = _throwStartPosition + (_ownerTransform.rotation * localPosition);

            _trajectoryPathPoints[i] = trajectoryPosition;
            _currentTrajectoryDebug.SetPosition(i, trajectoryPosition);
        }

        ComputeTrajectoryEndSpot();
    }

    private void ComputeTrajectoryEndSpot()
    {
        Vector3 lastTrajectoryPoint = _trajectoryPathPoints[_trajectoryPathPoints.Length - 1];

        Vector3 trajectoryEndPoint = lastTrajectoryPoint;
        Quaternion trajectoryEndRotation = Quaternion.identity;

        bool hitDuringTrajectory = false;


        for (int i = 0; i < _trajectoryPathPoints.Length - 1; ++i)
        {
            Vector3 rayVector = _trajectoryPathPoints[i+1] - _trajectoryPathPoints[i];

            if (Physics.Raycast(_trajectoryPathPoints[i], rayVector.normalized, out RaycastHit trajectoryHit, rayVector.magnitude, _obstacleLayers, QueryTriggerInteraction.Ignore))
            {
                hitDuringTrajectory = true;
                trajectoryEndPoint = trajectoryHit.point;
                Vector3 forward = Vector3.ProjectOnPlane(Vector3.up, trajectoryHit.normal).normalized;
                trajectoryEndRotation = Quaternion.LookRotation(forward, trajectoryHit.normal);
                break;
            }
        }


        if (!hitDuringTrajectory && Physics.Raycast(lastTrajectoryPoint, Vector3.down, out RaycastHit hit, 1000, _obstacleLayers, QueryTriggerInteraction.Ignore))
        {
            trajectoryEndPoint = hit.point;
            trajectoryEndRotation = Quaternion.identity;
        }

        _anchorTrajectoryEndSpot.position = trajectoryEndPoint;
        _anchorTrajectoryEndSpot.rotation = trajectoryEndRotation;
    }


    private void CorrectTrajectory(float strength01, Vector3 lookDirection)
    {
        ComputeTrajectory(strength01, lookDirection);

        _anchorSnapper.ClearState();
        _anchorIsBeingSnapped = _anchorSnapper.HasSnapTarget(_trajectoryPathPoints);
        if (_anchorIsBeingSnapped)
        {
            _anchorSnapper.CorrectTrajectoryPath(_trajectoryPathPoints);
        }
    }


    public bool IsOnAir()
    {
        return _currentState == AnchorStates.OnAir;
    }
    public bool IsOnGround()
    {
        return _currentState == AnchorStates.OnGround;
    }
    public bool CanBeGrabbed()
    {
        return IsOnGround();
    }


    public void ChangeState(AnchorStates newState)
    {
        _currentState = newState;

        if (newState == AnchorStates.OnAir)
        {
            _alreadyCollidedWithWall = false;
        }
    }


    private Vector3 GetTrajectoryPosition(float time, Vector3 startVelocity, Vector3 startPosition)
    {
        return startPosition + (startVelocity * time) + 0.5f * ((Physics.gravity + _springJoint.currentForce) * _rigidbody.mass * time * time );
    }


    private void LaunchAnchor()
    {
        SetMovable();
        //_rigidbody.AddForce(startVelocity, ForceMode.Impulse);
        float trajectoryDistance = Vector3.Distance(_trajectoryPathPoints[0], _trajectoryPathPoints[_trajectoryPathPoints.Length - 1]) * 0.15f;
        float maxThrowDuration = _maxThrowDuration * trajectoryDistance;
        float minThrowDuration = _minThrowDuration * trajectoryDistance;

        float duration = Mathf.Lerp(minThrowDuration, maxThrowDuration, _forceCurveNewTrajectory.Evaluate(_throwStrength01));

        if (_anchorIsBeingSnapped)
        {
            _rigidbody.DOLocalPath(_trajectoryPathPoints, duration, PathType.CatmullRom)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => {
                    if (IsOnAir())
                    {
                        SetSnapped();
                    }
                });

            _anchorSnapper.ConfirmSnapping(duration);

            Quaternion rotation = _anchorTransform.rotation;
            _anchorTransform.localRotation = Quaternion.identity;
            _anchorMeshTransform.rotation = rotation;
            _anchorMeshTransform.DOLocalRotateQuaternion(_anchorSnapper.GetTargetSnapRotation(), duration).SetEase(Ease.InOutSine);
        }
        else
        {
            _rigidbody.DOLocalPath(_trajectoryPathPoints, duration, PathType.CatmullRom)
                .OnComplete(() => {
                    if (IsOnAir())
                    {
                        SnapToFloor();
                    }
                });

            Quaternion rotation = _anchorTransform.rotation;
            _anchorTransform.localRotation = Quaternion.identity;
            _anchorMeshTransform.rotation = rotation;
            _anchorMeshTransform.DOBlendableLocalRotateBy(_anchorMeshTransform.right * 60, duration).SetEase(Ease.InOutSine);
        }
    }

    private void DrawTrajectory(Vector3 startVelocity)
    {
        int stepCount = 30;
        float time = 0.2f;
        float step = time / stepCount;


        _trajectoryLine.positionCount = stepCount;

        Vector3 position;
        Vector3 previousPosition = Position;
        for (int i = 0; i < stepCount; ++i)
        {
            float t = i * step;
            position = GetTrajectoryPosition(t, startVelocity, _throwStartPosition);
            _trajectoryLine.SetPosition(i, position);
            
            if (Physics.SphereCast(position, _collider.radius, (position - previousPosition).normalized, out RaycastHit hit, 0.1f, 
                _obstacleLayers, QueryTriggerInteraction.Ignore))
            {
                for (int  j = i+1; j < stepCount; ++j)
                {
                    _trajectoryLine.SetPosition(j, position);
                }
                return;
            }

            previousPosition = position;
        }
    }


    public async void SnapToFloorAndSetStill()
    {
        // This is kinda scuffed... need to find a better way
        if (Physics.Raycast(Position, Vector3.down, out RaycastHit hit))
        {
            if (hit.distance > _collider.radius + 0.1f)
            {
                _rigidbody.AddForce(Vector3.down *  hit.distance, ForceMode.Impulse);
                await Task.Delay(400);
                SetStill();
                return;
            }            
        }

        SetStill();
    }

    public void SetStill()
    {
        if (!_rigidbody.isKinematic)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
        _rigidbody.interpolation = RigidbodyInterpolation.None;

        _hitTrigger.enabled = false;

        _isBeingPulled = false;
    }
    public void SetMovable()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.useGravity = true;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

        _hitTrigger.enabled = true;

        _isBeingPulled = false;
    }
    public void SetPullMovable()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.useGravity = true;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

        _hitTrigger.enabled = true;

        _isBeingPulled = true;
    }

    private void SetSnapped()
    {
        SetStill();
        ChangeState(AnchorStates.OnGround);
    }

    private void UpdateOwnerBinderLine()
    {
        _ownerBinderLine.positionCount = 2;
        _ownerBinderLine.SetPosition(0, _anchorBindLineOrigin.position);
        _ownerBinderLine.SetPosition(1, _ownerTransform.position);
    }

    public void ShowTrajectory()
    {
        //_trajectoryLine.enabled = true;

        _currentTrajectoryDebug.enabled = _drawDebugLine;
        _anchorTrajectoryEndSpot.gameObject.SetActive(true);
    }
    
    public void HideTrajectory()
    {
        //_trajectoryLine.enabled = false;

        _currentTrajectoryDebug.enabled = false;
        _anchorTrajectoryEndSpot.gameObject.SetActive(false);
    }

    public void SetAimingPosition()
    {
        float duration = Vector3.Distance(_anchorTransform.position, _aimingPosition) * 0.01f;

        _anchorTransform.DOKill();
        _anchorTransform.DOLocalMove(_aimingPosition, duration);
        _anchorTransform.DOLocalRotateQuaternion(Quaternion.identity, duration);
        _anchorMeshTransform.DOLocalRotateQuaternion(Quaternion.Euler(_aimingRotation), duration);
    }
    public void SetAimingPositionInstantly()
    {
        _anchorTransform.DOKill();
        _anchorTransform.localPosition = _aimingPosition;
        _anchorTransform.localRotation = Quaternion.identity;
        _anchorMeshTransform.localRotation = Quaternion.Euler(_aimingRotation);
    }
    public void SetGrabbedPosition()
    {
        float duration = Vector3.Distance(_anchorTransform.position, _grabbedPosition) * 0.01f;

        _anchorTransform.DOKill();
        _anchorTransform.DOLocalMove(_grabbedPosition, duration);
        _anchorTransform.DOLocalRotateQuaternion(Quaternion.identity, duration);
        _anchorMeshTransform.DOLocalRotateQuaternion(Quaternion.Euler(_grabbedRotation), duration);
    }



    
    public async Task MeleeAttack()
    {
        float prepareDuration = 0.1f;
        float recoverDuration = 0.2f;
        float duration = 0.5f;
        float halfDuration = duration / 2;

        _anchorDamageDealer.StartMeleeHitDamage(Position, prepareDuration, duration);

        _canMeleeAttack = false;


        _anchorTransform.DOComplete();

        _anchorTransform.DOBlendableLocalMoveBy(new Vector3(-0.5f, 0.0f, 1.0f), prepareDuration).OnComplete(() =>
        {
            _anchorTransform.DOBlendableLocalMoveBy(new Vector3(0.0f, 0.0f, 1.2f), halfDuration).OnComplete(() =>
            {
                _anchorTransform.DOBlendableLocalMoveBy(new Vector3(0.0f, 0.0f, -1.2f), halfDuration);
            });

            _anchorTransform.DOBlendableLocalMoveBy(new Vector3(2.0f, 0.0f, 0.0f), duration).OnComplete(() =>
            {
                _anchorTransform.DOLocalMove(_grabbedPosition, recoverDuration).OnComplete(() =>
                {
                    _canMeleeAttack = true;
                });
            });
        });

        await Task.Delay((int)((prepareDuration + duration + recoverDuration)*1000));
    }

    private float GetDistanceFromOwner()
    {
        return Vector3.Distance(Position, _springJoint.connectedBody.position);
    }
    private Vector3 DirectionTowardsOwner()
    {
        return (_springJoint.connectedBody.position - Position).normalized;
    }
    private Vector3 DirectionTowardsOwnerOnPlane()
    {
        Vector3 directionTowardsOwner = _springJoint.connectedBody.position - Position;
        directionTowardsOwner.y = 0;
        return directionTowardsOwner.normalized;
    }

    public bool IsOwnerTensionLimit()
    {
        float distance = GetDistanceFromOwner();
        float thresholdDistance = _springJoint.maxDistance;

        if (distance < thresholdDistance)
        {
            return false;
        }

        
        if (Physics.Raycast(_anchorBindLineOrigin.position, DirectionTowardsOwner(), distance, _obstacleLayers, QueryTriggerInteraction.Ignore))
        {
            return false;
        }

        return true;
    }

    public async Task AttractOwner(float duration)
    {
        Transform ownerTransform = _springJoint.connectedBody.transform;
        Vector3 ownerPosition = ownerTransform.position;
        Vector3 directionToAnchor = (Position - ownerPosition).normalized;
        float offsetDistance = 2.5f;
        float offsetHeight = 2.5f;

        Vector3 endPosition = Position;
        endPosition += directionToAnchor * offsetDistance;
        endPosition.y += offsetHeight;

        Vector3 directionToEndPosition = (endPosition - ownerPosition).normalized;
        if (Physics.Raycast(ownerPosition, directionToEndPosition, out RaycastHit hit, Vector3.Distance(ownerPosition, endPosition), _obstacleLayers, 
            QueryTriggerInteraction.Ignore))
        {
            endPosition = hit.point - directionToEndPosition * 0.6f;
        }


        await ownerTransform.DOMove(endPosition, duration).SetEase(Ease.OutQuad).AsyncWaitForCompletion();
    }


    public bool CanDoChargedPullAttack()
    {
        if (Vector3.Distance(Position, _ownerTransform.position) < 2.0f) return false;

        Vector3 ownerForward = _ownerTransform.forward;
        Vector3 directionFromOwner = -DirectionTowardsOwnerOnPlane();

        float angle = Mathf.Acos(Vector3.Dot(ownerForward, directionFromOwner)) * Mathf.Rad2Deg;

        return angle > _canPullAttackAngleThreshold;
    }

    public async void StartChargedPullAttack(float duration)
    {
        Vector3 ownerPosition = _ownerTransform.position;
        Vector3 ownerForward = _ownerTransform.forward;
        float distance = _springJoint.maxDistance * 0.5f;

        Vector3 endPosition = ownerPosition + (ownerForward * distance);

        Vector3 offsetToEnd = endPosition - Position;
        Vector3 moveDirection = offsetToEnd.normalized;

        transform.DOBlendableMoveBy(offsetToEnd, duration)
            .SetEase(Ease.InOutSine);

        Vector3 up = Vector3.up;
        Vector3 right = Vector3.Cross(moveDirection, up).normalized;

        Vector3 moveArchDirection = Vector3.Lerp(up, right, _pullAttackArchLerp).normalized;

        transform.DOBlendableMoveBy(moveArchDirection * distance, duration / 2)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => {
                transform.DOBlendableMoveBy(-moveArchDirection * distance, duration / 2)
                .SetEase(Ease.InOutSine);
            });

        await Task.Delay((int)(duration * 1000));

        //_anchorDamageDealer.DealGroundHitDamage(Position, Mathf.Min(1.0f, distance / _springJoint.maxDistance));

        ChangeState(AnchorStates.OnAir);

        _throwStrength01 = 1.0f;

        SetMovable();
        _rigidbody.AddForce(Vector3.down * 50f, ForceMode.Impulse);
    }


    public bool CanUseExplosionAbility()
    {
        if (!_staminaSystem.HasEnoughStamina(_explosionStamina))
        {
            _staminaBar.PlayErrorAnimation();
            return false;
        }

        return true;
    }

    public void UseExplosionAbility()
    {
        _anchorDamageDealer.DealExplosionDamage(Position);
        _staminaSystem.Spend(_explosionStamina);
    }


    public void SetMeshRotation(Quaternion meshDirection)
    {
        _anchorMeshTransform.rotation = meshDirection;
    }

}
