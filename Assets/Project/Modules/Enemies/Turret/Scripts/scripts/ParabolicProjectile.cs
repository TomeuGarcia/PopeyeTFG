using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Popeye.Core.Pool;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.Enemies.Hazards;
using Popeye.Modules.VFX.Generic;
using Popeye.Modules.VFX.ParticleFactories;
using Popeye.Scripts.Collisions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class ParabolicProjectile : RecyclableObject
{
    private Transform _firePoint;
    [SerializeField] private float _height;
    [FormerlySerializedAs("speed")] [SerializeField] private float _speed;
    private Transform _playerTransform;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private MeshRenderer _bulletBody;
    [SerializeField] private float _predictMagnitude;
    [SerializeField] private TrailRenderer _trail;
    private Vector3 _lastFrameTargetPosition = Vector3.zero;
    private bool _shoot = false;

    private DamageHit _contactDamageHit;
    [SerializeField] private DamageHitConfig _contactDamageConfig;
    private ICombatManager _combatManager;

    private IHazardFactory _hazardFactory;
    [SerializeField] private CollisionProbingConfig _defaultProbingConfig;

    [SerializeField] float _distanceToTargetThreshold;
    [SerializeField] ParticleTypes _projectileExplosion;
    [SerializeField] ParticleTypes _projectileArea;
    
    private IParticleFactory _particleFactory;


    private void Update()
    {
        if (_playerTransform != null)
        {

                if (_shoot)
                {
                    Vector3 playerMoveDir = (_playerTransform.position - _lastFrameTargetPosition).normalized;
                    Vector3 direction = (_playerTransform.position + playerMoveDir * _predictMagnitude) - _firePoint.position;
                    Vector3 groundDirection = new Vector3(direction.x, 0, direction.z);
                    Vector3 targetPos = new Vector3(groundDirection.magnitude, direction.y, 0);
                    float angle;
                    float v0;
                    float time;
                    
                    CalculatePathWithHeight(targetPos, _height, out v0, out angle, out time);
                    _shoot = false;
                    _bulletBody.enabled = true;
                    Movement(groundDirection.normalized, v0, angle, time);
                }

            
                _lastFrameTargetPosition = _playerTransform.position;
        }

    }

    public void SetParticleFactory(IParticleFactory particleFactory)
    {
        _particleFactory = particleFactory;
    }
    
    public void PrepareShot(Transform playerTransform,IHazardFactory hazardFactory,Transform firePoint)
    {
        _shoot = false;
        _hazardFactory = hazardFactory;
        _playerTransform = playerTransform;
        _firePoint = firePoint;
        _bulletBody.enabled = false;
        _trail.Clear();
        _trail.enabled = false;
        gameObject.SetActive(true);
    }

    private void Start()
    {
        _contactDamageHit = new DamageHit(_contactDamageConfig);
        _combatManager = ServiceLocator.Instance.GetService<ICombatManager>();
    }
    
    public void Shoot()
    {
        _shoot = true;
    }

    

    private async UniTaskVoid Movement(Vector3 direction, float v0,float angle,float time)
    {
        float t = 0;
        Vector3 origin = _firePoint.position;
        while (t < time*2)
        {
            float x = v0 * t * Mathf.Cos(angle);
            float y = (float)(v0 * t * Math.Sin(angle) - (1f/2f) * -Physics.gravity.y * Mathf.Pow(t,2));
            if (_rigidbody != null)
            {
                _rigidbody.MovePosition(origin + direction * x + Vector3.up * y);
            }
            if (!_trail.enabled)
            {
                
                _trail.enabled = true;
                _trail.Clear();
            }
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit,1f,_defaultProbingConfig.CollisionLayerMask,_defaultProbingConfig.QueryTriggerInteraction))
                {

                    var startRot = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(new Vector3(0,90,90f));
                    _hazardFactory.CreateDamageArea(hit.point, startRot);
                    _particleFactory.Create(_projectileExplosion, hit.point, startRot);
                    _particleFactory.Create(_projectileArea, hit.point, startRot);
                    Recycle();
                    t = time * 2;
                }
                t += Time.deltaTime * _speed;
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

    internal override void Release()
    {
        _shoot = false;
        _bulletBody.enabled = false;
    }
}
