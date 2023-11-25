using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random=UnityEngine.Random;

public class SlimeMovement : MonoBehaviour
{
    [SerializeField] private Vector2 _speeedThreshold = new Vector2(5, 7);
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private ProximityTargetGetterBehaviour _proximityTargetGetterBehaviour;
    [SerializeField] private SlimeHealth _slimeHealth;
    [SerializeField] private int _spawnForce = 10;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private BoxCollider _bc;
    private Transform _playerTransform = null;
    private bool _followPlayer = false;
    
    
    [SerializeField] private float _squashAmount = 0.8f;
    [SerializeField] private float _stretchAmount = 1.2f;
    [SerializeField] private float _animationSpeed = 5f;

    private bool _startSquashing = true;
    private bool _isSquashing = false;
    

    private void Start()
    {
        _navMeshAgent.speed = Random.Range(_speeedThreshold.x, _speeedThreshold.y);
    }

    void Update()
    {
        if (_followPlayer)
        {
            _navMeshAgent.SetDestination(_playerTransform.position);
        }
        
        
        if (_startSquashing)  
        {
            if (!_isSquashing)
            {
                // Start squash animation
                StartCoroutine(SquashAndStretch());
            }
        }
    }

    public void SetTarget()
    {
        _playerTransform = _proximityTargetGetterBehaviour.CurrentTarget.transform;
        if (_navMeshAgent.isActiveAndEnabled)
        {
            _followPlayer = true;
        }
    }

    public void SpawningFromExplosion(Vector3 spawnDir)
    {
        StartCoroutine(SpawnKnockback(spawnDir));
    }

    IEnumerator SpawnKnockback(Vector3 spawnDir)
    {
        _startSquashing = false;
        _bc.isTrigger = false;
        _navMeshAgent.enabled = false;
        _slimeHealth.SetIsInvulnerable(true);
        _rb.AddForce(spawnDir * _spawnForce,ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        _rb.velocity = Vector3.zero;
        _bc.isTrigger = true;
        _slimeHealth.SetIsInvulnerable(false);
        _navMeshAgent.enabled = true;
        _followPlayer = true;
        _startSquashing = true;
    }
    
    
    
    IEnumerator SquashAndStretch()
    {
        _isSquashing = true;

        // Squash
        while (transform.localScale.y > _squashAmount)
        {
            transform.localScale -= new Vector3(0f, Time.deltaTime * _animationSpeed, 0f);
            transform.localScale += new Vector3(Time.deltaTime * _animationSpeed/2, 0, Time.deltaTime * _animationSpeed/2);
            yield return null;
        }

        // Stretch
        while (transform.localScale.y < _stretchAmount)
        {
            transform.localScale += new Vector3(0f, Time.deltaTime * _animationSpeed, 0f);
            transform.localScale -= new Vector3(Time.deltaTime * _animationSpeed/2, 0, Time.deltaTime * _animationSpeed/2);
            yield return null;
        }


        _isSquashing = false;
    }
}

