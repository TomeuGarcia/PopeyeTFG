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

        [SerializeField] private BoxCollider _boxCollider;
        [FormerlySerializedAs("_slimeMindEnemy")] public SlimeMindEnemy slimeMindEnemy;
        public Transform playerTransform { get; private set; }
        [SerializeField] private Transform _slimeTransform;

        private void Awake()
        {
            _slimeMovement.Configure(this);
            _enemyHealth.Configure(this);
            _squashStretchAnimator.Configure(this,_slimeTransform);
            _slimeDivider.Configure(this);
            //_slimeMindEnemy = transform.parent.GetComponent<SlimeMindEnemy>();
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
        }

        public void AddSlimesToSlimeMindList(SlimeMediator mediator)
        {
            slimeMindEnemy.AddSlimeToList();
        }

        public void SpawningFromDivision(Vector3 explosionForceDir)
        {
            ApplyDivisionExplosionForces(explosionForceDir);
        }

        private async void ApplyDivisionExplosionForces(Vector3 explosionForceDir)
        {
            //TODO: this should be unitask
            _slimeMovement.DeactivateNavigation();
            _squashStretchAnimator.PlayDeath();
            _enemyHealth.SetIsInvulnerable(true);
            _boxCollider.isTrigger = false;
            _slimeMovement.ApplyExplosionForce(explosionForceDir);

            await Task.Delay(TimeSpan.FromSeconds(0.5f));

            _slimeMovement.StopExplosionForce();
            _boxCollider.isTrigger = true;
            _slimeMovement.ActivateNavigation();
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

        public void OnHit()
        {
            throw new NotImplementedException();
        }
    }
}
