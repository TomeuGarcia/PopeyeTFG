using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMediator : MonoBehaviour
{
    [SerializeField] private SlimeMovement _slimeMovement;
    [SerializeField] private SlimeHealth _slimeHealth;
    [SerializeField] private SlimeAnimation _slimeAnimation;
    [SerializeField] private SlimeDivider _slimeDivider;
    
    [SerializeField] private BoxCollider _boxCollider;
    private SlimeMindEnemy _slimeMindEnemy;
    public Transform playerTransform { get; private set; }

    private void Awake()
    {
        _slimeMovement.Configure(this);
        _slimeHealth.Configure(this);
        _slimeAnimation.Configure(this);
        _slimeDivider.Configure(this);
        _slimeMindEnemy = transform.parent.GetComponent<SlimeMindEnemy>();
    }

    public void SetPlayerTransform(Transform _playerTransform)
    {
        playerTransform = _playerTransform;
        _slimeMovement.SetTarget(playerTransform);
    }
    public void AddSlimesToSlimeMindList(SlimeMediator mediator)
    {
        _slimeMindEnemy.AddSlimeToList();
    }
    public void SpawningFromDivision(Vector3 explosionForceDir)
    {
        StartCoroutine(ApplyDivisionExplosionForces(explosionForceDir));
    }

    IEnumerator ApplyDivisionExplosionForces(Vector3 explosionForceDir)
    {
        //TODO: this should be unitask
        _slimeMovement.DeactivateNavigation();
        _slimeAnimation.DeactivateAnimation();
        _slimeHealth.SetIsInvulnerable(true);
        _boxCollider.isTrigger = false;
        _slimeMovement.ApplyExplosionForce(explosionForceDir);
        
        yield return new WaitForSeconds(0.5f);
        
        _slimeMovement.StopExplosionForce();
        _boxCollider.isTrigger = true;
        _slimeMovement.ActivateNavigation();
        _slimeAnimation.ActivateAnimation();
        _slimeHealth.SetIsInvulnerable(false);
    }
    public void Divide()
    {
        _slimeAnimation.SpawnExplosionParticles();
        _slimeDivider.SpawnSlimes();
        _slimeMindEnemy.RemoveSlimeFromList();
        Destroy(gameObject);
    }
}
