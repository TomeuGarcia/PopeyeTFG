using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Popeye.Core.Pool;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.Enemies.Hazards;
using Popeye.Scripts.Collisions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class ParabolicProjectile : RecyclableObject
{
    private Transform _firePoint;
    [SerializeField] private float _height;
    [SerializeField] private float speed;
    private Transform _playerTransform;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private MeshRenderer _bulletBody;
    private bool _shoot = false;
    private bool _aiming = false;

    private DamageHit _contactDamageHit;
    [SerializeField] private DamageHitConfig _contactDamageConfig;
    private ICombatManager _combatManager;

    private IHazardFactory _hazardFactory;
    [SerializeField] private CollisionProbingConfig _defaultProbingConfig;

    [SerializeField] float _distanceToTargetThreshold;
    float _sqrDistanceToTargetThreshold;
    private Vector3 _shotTarget;
    private void Update()
    {
        if (_playerTransform != null)
        {
            
            Vector3 direction = _playerTransform.position - _firePoint.position;
            Vector3 groundDirection = new Vector3(direction.x, 0, direction.z);
            Vector3 targetPos = new Vector3(groundDirection.magnitude, direction.y, 0);
            float angle;
            float v0;
            float time;

                
                
                if (_shoot)
                {
                    _shotTarget = _playerTransform.position+Vector3.down;
                    CalculatePathWithHeight(targetPos, _height, out v0, out angle, out time);
                    _shoot = false;
                    _aiming = false;
                    _bulletBody.enabled = true;
                    Movement(groundDirection.normalized, v0, angle, time);
                }

            
            
        }

    }

    public void PrepareShot(Transform firePoint,Transform playerTransform,IHazardFactory hazardFactory)
    {
        _hazardFactory = hazardFactory;
        _firePoint = firePoint;
        _aiming = false;
        _playerTransform = playerTransform;
        _bulletBody.enabled = false;
        gameObject.SetActive(true);
    }

    private void Start()
    {
        _sqrDistanceToTargetThreshold = _distanceToTargetThreshold * _distanceToTargetThreshold;
        _contactDamageHit = new DamageHit(_contactDamageConfig);
        _combatManager = ServiceLocator.Instance.GetService<ICombatManager>();
    }
    
    public void Shoot()
    {
        _shoot = true;
    }
    private float DistanceToTargetSqrMagnitude(Vector3 targetPos)
    {
        return (targetPos - transform.position).sqrMagnitude;
    }
    
    private bool IsCloseToTarget()
    {
        return DistanceToTargetSqrMagnitude(_shotTarget) < _sqrDistanceToTargetThreshold;
    }
    private async UniTaskVoid Movement(Vector3 direction, float v0,float angle,float time)
    {
        float t = 0;
        while (t < time*2)
        {
            float x = v0 * t * Mathf.Cos(angle);
            float y = (float)(v0 * t * Math.Sin(angle) - (1f/2f) * -Physics.gravity.y * Mathf.Pow(t,2));
            _rigidbody.MovePosition(_firePoint.position + direction * x + Vector3.up * y);
            if (IsCloseToTarget())
            {
                Debug.Log("djpanchi");
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit,_defaultProbingConfig.ProbeDistance,_defaultProbingConfig.CollisionLayerMask,_defaultProbingConfig.QueryTriggerInteraction))
                {
                    
                    var startRot = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(new Vector3(0,90,90f));
                    WaitAndRecycle(hit,startRot);
                }
                
            }
            t += Time.deltaTime * speed;
            await UniTask.NextFrame();
        }
        
        
    }

    private async UniTaskVoid WaitAndRecycle(RaycastHit hit,quaternion startRot)
    {
        //UniTask.Delay(1500);
        _hazardFactory.CreateDamageArea(hit.point, startRot);
        Recycle();
    }
    public float QuadraticEquation(float a, float b, float c, float sign)
    {
        return (-b + sign * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
    }
    private void CalculatePathWithHeight(Vector3 targetPos, float h, out float v0, out float angle, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float b = Mathf.Sqrt(2 * g * h);
        float a = (-0.5f * g);
        float c = -yt;

        float tPlus = QuadraticEquation(a, b, c, 1);
        float tMinus = QuadraticEquation(a, b, c, -1);
        time = tPlus > tMinus ? tPlus : tMinus;

        angle = Mathf.Atan(b * time / xt);

        v0 = b / Mathf.Sin(angle);


    }

    internal override void Init()
    {
        Invoke(nameof(Recycle),5);
    }

    private void OnCollisionEnter(Collision other)
    {
           /* Debug.Log("collided with " + other.transform.name);
            _contactDamageHit.DamageSourcePosition = transform.position;
            _contactDamageHit.UpdateKnockbackPushDirection(PositioningHelper.Instance.GetDirectionAlignedWithFloor(transform.position, other.transform.position));
            _combatManager.TryDealDamage(other.gameObject, _contactDamageHit, out DamageHitResult damageHitResult);
            _bulletBody.enabled = false;
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit,_defaultProbingConfig.ProbeDistance,_defaultProbingConfig.CollisionLayerMask,_defaultProbingConfig.QueryTriggerInteraction))
            {
                Debug.Log("raycast hit " + hit.transform.name);
                var startRot = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(new Vector3(0,90,90f));
                _hazardFactory.CreateDamageArea(hit.point, startRot);
            }
            Recycle();*/
    }

    internal override void Release()
    {
        _shoot = false;
        _aiming = false;
        _bulletBody.enabled = false;
    }
}
