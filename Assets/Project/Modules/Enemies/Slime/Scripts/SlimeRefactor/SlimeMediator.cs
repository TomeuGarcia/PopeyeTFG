using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.Enemies.Components;
using UnityEngine;
using UnityEngine.Serialization;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;


namespace Popeye.Modules.Enemies
{
    public class SlimeMediator : MonoBehaviour , IEnemyMediator
    {
        [SerializeField] private SlimeMovement _slimeMovement;
        [SerializeField] private EnemyHealth _enemyHealth;
        [SerializeField] private SquashStretchAnimator _squashStretchAnimator;
        [SerializeField] private SlimeDivider _slimeDivider;
        [SerializeField] private EnemyPatrolling _enemyPatrolling;

        [SerializeField] private BoxCollider _boxCollider;
        public SlimeMindEnemy slimeMindEnemy;
        public Transform playerTransform { get; private set; }
        [SerializeField] private Transform _slimeTransform;

        public void Init()
        {
            _slimeMovement.Configure(this);
            _enemyHealth.Configure(this);
            _squashStretchAnimator.Configure(this,_slimeTransform);
            _slimeDivider.Configure(this);
            _enemyPatrolling.Configure(this);
        }

        public void SetSlimeMind(SlimeMindEnemy slimeMind)
        {
            slimeMindEnemy = slimeMind;
        }
        public void PlayMoveAnimation()
        {
            _squashStretchAnimator.PlayMove();
        }
        public void SetPlayerTransform(Transform _playerTransform)
        {
            playerTransform = _playerTransform;
            _slimeMovement.SetTarget(playerTransform);
            _enemyPatrolling.SetPlayerTransform(playerTransform);
        }

        public void SetWayPoints(Transform[] wayPoints)
        {
            _enemyPatrolling.SetWayPoints(wayPoints);
        }
       
        public void AddSlimesToSlimeMindList(SlimeMediator mediator)
        {
            slimeMindEnemy.AddSlimeToList();
        }

        public void SpawningFromDivision(Vector3 explosionForceDir,EnemyPatrolling.PatrolType type,Transform[] wayPoints)
        {
            ApplyDivisionExplosionForces(explosionForceDir,type,wayPoints);
        }

        private async void ApplyDivisionExplosionForces(Vector3 explosionForceDir,EnemyPatrolling.PatrolType type,Transform[] wayPoints)
        {
            _slimeMovement.DeactivateNavigation();
            _squashStretchAnimator.PlayDeath();
            _enemyHealth.SetIsInvulnerable(true);
            _boxCollider.isTrigger = false;
            _slimeMovement.ApplyExplosionForce(explosionForceDir);

            await Task.Delay(TimeSpan.FromSeconds(0.5f));

            _slimeMovement.StopExplosionForce();
            _boxCollider.isTrigger = true;
            _slimeMovement.ActivateNavigation();
            if(type == EnemyPatrolling.PatrolType.FixedWaypoints){SetWayPoints(wayPoints);}
            else if (type == EnemyPatrolling.PatrolType.None){StartChasing();}
            PlayMoveAnimation();
            _enemyHealth.SetIsInvulnerable(false);
        }

        public void Divide()
        {
            _squashStretchAnimator.PlayDeath();
            _slimeDivider.SpawnSlimes();
            slimeMindEnemy.RemoveSlimeFromList();
            _squashStretchAnimator.StopMove();
            Destroy(gameObject);
        }

        public void OnDeath()
        {
            Divide();
        }

        public void OnPlayerClose()
        {
            StartChasing();
        }

        public void OnPlayerFar()
        {
            StartPatrolling();
        }

        public void StartChasing()
        {
            _enemyPatrolling.SetPatrolling(false);
            _slimeMovement.StartChasing();
        }

        public void StartPatrolling()
        {
            _slimeMovement.StopChasing();
            _enemyPatrolling.SetPatrolling(true);
        }
        public void OnHit()
        {
            //In this case, do nothing
        }

        public EnemyPatrolling.PatrolType GetPatrolType()
        {
            return _enemyPatrolling.GetPatrolType();
        }
        public Transform[] GetPatrolWaypoints()
        {
            return _enemyPatrolling.GetWaypoints();
        }
    }
}
