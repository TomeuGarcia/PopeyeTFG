using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Pool;
using Popeye.Core.Services.GameReferences;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Enemies.Components;
using Popeye.Modules.VFX.Generic;
using UnityEngine;
using FMODUnity;
using Popeye.Modules.Enemies.EnemyFactories;

namespace Popeye.Modules.Enemies
{
    public class SlimeMindEnemy : AEnemy
    {
        [Header("SLIME MIND")]
        [SerializeField] private SlimeSize _startingStartSize;
        [SerializeField] private List<SlimeData> _sizeToPrefab;
        private Dictionary<SlimeSize, GameObject> _sizeToPrefabDictionary = new Dictionary<SlimeSize, GameObject>();
        private int _currentSlimesCount;
        
        [SerializeField] private EnemyPatrolling.PatrolType _patrolType = EnemyPatrolling.PatrolType.None;
        [SerializeField] private Transform[] _wayPoints;
        
        
        [SerializeField] private Transform _transform;
        private ObjectPool _objectPool;
        [SerializeField] private PooledParticle _explosionParticles;

        private SlimeMediator _slimeMediator;
        public enum SlimeSize
        {
            SlimeSize1,
            SlimeSize2,
            SlimeSize3
        }

        [Serializable]
        public class SlimeData
        {
            public SlimeSize size;
            public GameObject prefab;
        }

        private void Awake()
        {
            _objectPool = new ObjectPool(_explosionParticles, _transform);
            _objectPool.Init(15);
        }

        public ObjectPool GetParticlePool()
        {
            return _objectPool;
        }

        public Transform GetPlayerTransform()
        {
            return _attackTarget;
        }
        public void InitAfterSpawn(SlimeMediator slimeMediator,EnemyPatrolling.PatrolType patrolType)
        {
            _patrolType = patrolType;
            _slimeMediator = slimeMediator;
            if(_patrolType == EnemyPatrolling.PatrolType.None){slimeMediator.StartChasing();}
        }


        public void AddSlimeToList()
        {
            _currentSlimesCount++;
        }

        public void RemoveSlimeFromList()
        {
            _currentSlimesCount--;

            if (_currentSlimesCount <= 0)
            {
                InvokeOnDeathComplete();
            }
        }

        internal override void Init()
        {
            _currentSlimesCount = 1;
            _attackTarget = ServiceLocator.Instance.GetService<IGameReferences>().GetPlayer();
            
        }

        internal override void Release()
        {
            throw new NotImplementedException();
        }

        public override void SetPatrollingWaypoints(Transform[] waypoints)
        {
            _slimeMediator.SetWayPoints(waypoints);
        }
    }
}
