using System;
using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Pool;
using Popeye.Core.Services.GameReferences;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Enemies.Components;
using UnityEngine;

namespace Popeye.Modules.Enemies
{
    public class SlimeMindEnemy : AEnemy
    {
        [SerializeField] private SlimeSize _startingStartSize;
        [SerializeField] private List<SlimeData> _sizeToPrefab;
        private Dictionary<SlimeSize, GameObject> _sizeToPrefabDictionary = new Dictionary<SlimeSize, GameObject>();
        private int _currentSlimesCount;
        
        [SerializeField] private EnemyPatrolling.PatrolType _patrolType = EnemyPatrolling.PatrolType.None;
        [SerializeField] private Transform[] _wayPoints;
        
        
        [SerializeField] private Transform _transform;
        [SerializeField] private Core.Pool.ObjectPool _objectPool;
        [SerializeField] private PooledParticle _explosionParticles;
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

        private void Start()
        {
            foreach (var slimeData in _sizeToPrefab)
            {
                _sizeToPrefabDictionary.Add(slimeData.size, slimeData.prefab);
            }
            InstantiateFirstSlime();
            _currentSlimesCount++;
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
        
        private void InstantiateFirstSlime()
        {
            GameObject go = Instantiate(_sizeToPrefabDictionary[_startingStartSize], transform);
            SlimeMediator mediator = go.GetComponent<SlimeMediator>();
            _objectPool = new ObjectPool(_explosionParticles, _transform);
            _objectPool.Init(15);
            mediator.SetObjectPool(_objectPool);
            mediator.Init();
            _attackTarget = ServiceLocator.Instance.GetService<IGameReferences>().GetPlayer();
            mediator.SetPlayerTransform(_attackTarget);
            mediator.SetSlimeMind(this);
            mediator.PlayMoveAnimation();
            if(_patrolType == EnemyPatrolling.PatrolType.FixedWaypoints){mediator.SetWayPoints(_wayPoints);}
            if(_patrolType == EnemyPatrolling.PatrolType.None){mediator.StartChasing();}
        }
    }
}
