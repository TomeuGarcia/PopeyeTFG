using System;
using System.Collections.Generic;
using Popeye.Core.Pool;
using Popeye.Core.Services.GameReferences;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Enemies.Components;
using Popeye.Modules.VFX.Generic;
using UnityEngine;


namespace Popeye.Modules.Enemies
{
    public class SlimeMindEnemy : AEnemy
    {
        [Header("SLIME MIND")]
        private HashSet<SlimeMediator> _slimeMediatorsUnderControl;

        [SerializeField] private EnemyPatrolling.PatrolType _patrolType = EnemyPatrolling.PatrolType.None;
        [SerializeField] private Transform[] _wayPoints;
        
        
        [SerializeField] private Transform _transform;
        private ObjectPool _objectPool;
        [SerializeField] private PooledParticle _explosionParticles;

        private SlimeMediator _slimeMediator;

        private void Awake()
        {
            _objectPool = new ObjectPool(_explosionParticles, _transform);
            _objectPool.Init(15);

            _slimeMediatorsUnderControl = new HashSet<SlimeMediator>(5);
        }

        public ObjectPool GetParticlePool()
        {
            return _objectPool;
        }

        public Transform GetPlayerTransform()
        {
            return _attackTarget;
        }
        public void InitAfterSpawn(SlimeMediator slimeMediator)
        {
            _patrolType = EnemyPatrolling.PatrolType.None;
            _slimeMediator = slimeMediator;
            if(_patrolType == EnemyPatrolling.PatrolType.None){slimeMediator.StartChasing();}

            AddSlimeToList(slimeMediator);
        }


        public void AddSlimeToList(SlimeMediator slimeMediator)
        {
            _slimeMediatorsUnderControl.Add(slimeMediator);
        }

        public void RemoveSlimeFromList(SlimeMediator slimeMediator)
        {
            _slimeMediatorsUnderControl.Remove(slimeMediator);

            if (_slimeMediatorsUnderControl.Count <= 0)
            {
                InvokeOnDeathComplete();
                Recycle();
            }
        }

        internal override void Init()
        {
            _attackTarget = ServiceLocator.Instance.GetService<IGameReferences>().GetPlayerTargetForEnemies();
        }

        internal override void Release()
        {
            
        }

        public override void SetPatrollingWaypoints(Transform[] waypoints)
        {
            _slimeMediator.SetWayPoints(waypoints);
        }

        public override void DieFromOrder()
        {
            foreach (SlimeMediator slimeMediator in _slimeMediatorsUnderControl)
            {
                slimeMediator.DieFromOrder();
            }
            
            _slimeMediatorsUnderControl.Clear();
            
            Recycle();
        }
    }
}
