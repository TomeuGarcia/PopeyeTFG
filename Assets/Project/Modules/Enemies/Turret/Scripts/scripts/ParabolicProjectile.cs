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
    [SerializeField] private LineRenderer _line;
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
    
    [SerializeField] private Material _lineAimingMat;
    [SerializeField] private Material _lineShootingMat;

    private IHazardFactory _hazardFactory;
    [SerializeField] private CollisionProbingConfig _defaultProbingConfig;
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
            if (_aiming)
            {
                
                CalculatePathWithHeight(targetPos, _height, out v0, out angle, out time);
                if (_shoot)
                {
                    _shoot = false;
                    _aiming = false;
                    _bulletBody.enabled = true;
                    Movement(groundDirection.normalized, v0, angle, time);
                }
            }
            
            
        }

    }

    public void PrepareShot(Transform firePoint,Transform playerTransform,IHazardFactory hazardFactory)
    {
        _hazardFactory = hazardFactory;
        _firePoint = firePoint;
        _aiming = false;
        _playerTransform = playerTransform;
        _line.material = _lineAimingMat;
        _bulletBody.enabled = false;
        gameObject.SetActive(true);
    }

    private void Start()
    {
        _contactDamageHit = new DamageHit(_contactDamageConfig);
        _combatManager = ServiceLocator.Instance.GetService<ICombatManager>();
    }

    public void StartAiming()
    {
        _aiming = true;
    }
    public void Shoot()
    {
        _line.material = _lineShootingMat;
        _shoot = true;
    }
    private void DrawPath(Vector3 direction, float v0, float angle,float time, float step)
    {
        step = Mathf.Max(0.01f, step);
        _line.positionCount = (int)(time / step) + 2;
        int count = 0;
        
        for (float i = 0; i < time; i += step)
        {
            float x = v0 * i * Mathf.Cos(angle);
            float y = (float)(v0 * i * Math.Sin(angle) - (1f/2f) * -Physics.gravity.y * Mathf.Pow(i,2));
            _line.SetPosition(count,_firePoint.position + direction * x + Vector3.up * y);
            count++;
        }
        float xFinal = v0 * time * Mathf.Cos(angle);
        float yFinal = (float)(v0 * time * Math.Sin(angle) - (1f/2f) * -Physics.gravity.y * Mathf.Pow(time,2));
        _line.SetPosition(count,_firePoint.position +direction * xFinal + Vector3.up * yFinal);
    }
    private async UniTaskVoid Movement(Vector3 direction, float v0,float angle,float time)
    {
        float t = 0;
        while (t < time*2)
        {
            float x = v0 * t * Mathf.Cos(angle);
            float y = (float)(v0 * t * Math.Sin(angle) - (1f/2f) * -Physics.gravity.y * Mathf.Pow(t,2));
            _rigidbody.MovePosition(_firePoint.position + direction * x + Vector3.up * y); //= ;
            
            t += Time.deltaTime * speed;
            await UniTask.NextFrame();
        }
        
        
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
            _contactDamageHit.DamageSourcePosition = transform.position;
            _contactDamageHit.UpdateKnockbackPushDirection(PositioningHelper.Instance.GetDirectionAlignedWithFloor(transform.position, other.transform.position));
            _combatManager.TryDealDamage(other.gameObject, _contactDamageHit, out DamageHitResult damageHitResult);
            _bulletBody.enabled = false;
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit,_defaultProbingConfig.ProbeDistance,_defaultProbingConfig.CollisionLayerMask,_defaultProbingConfig.QueryTriggerInteraction))
            {
                var startRot = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(new Vector3(0,90,90f));
                _hazardFactory.CreateDamageArea(hit.point, startRot);
            }
            Recycle();
    }

    internal override void Release()
    {
        //release projectile
    }
}
