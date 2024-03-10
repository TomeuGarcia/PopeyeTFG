using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.Enemies.Components;
using UnityEngine;

namespace Popeye.Modules.Enemies
{
    public class ShieldedMediator : AEnemyMediator
    {
        [SerializeField] private ShieldedMovement _shieldedMovement;
        [SerializeField] private EnemyPatrolling _enemyPatrolling;
        private ShieldedMind _shieldedMind;

        internal override void Init()
        {
            _shieldedMovement.Configure(this);
            _enemyPatrolling.Configure(this);
        }

        public void SetShieldedMind(ShieldedMind shieldedMind)
        {
            shieldedMind = _shieldedMind;
        }
        
        public void SetPlayerTransform(Transform _playerTransform)
        {
            _shieldedMovement.SetTarget(_playerTransform);
            _enemyPatrolling.SetPlayerTransform(_playerTransform);
        }
        public void StartChasing()
        {
            _enemyPatrolling.SetPatrolling(false);
            _shieldedMovement.StartChasing();
        }

        public void StartPatrolling()
        {
            _shieldedMovement.StopChasing();
            _enemyPatrolling.SetPatrolling(true);
        }
        
        internal override void Release()
        {
            _enemyPatrolling.ResetPatrolling();
        }

        public override Vector3 Position { get; }
        public override void OnPlayerClose()
        {
            StartChasing();
        }

        public override void OnPlayerFar()
        {
            StartPatrolling();
        }

        public override void DieFromOrder()
        {
            Recycle();
        }
    }
}
