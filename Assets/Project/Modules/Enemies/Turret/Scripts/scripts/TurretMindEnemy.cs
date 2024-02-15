using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Pool;
using Popeye.Core.Services.GameReferences;
using Popeye.Core.Services.ServiceLocator;
using UnityEngine;
using UnityEngine.Pool;

namespace Popeye.Modules.Enemies
{
    public class TurretMindEnemy : AEnemy
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private GameObject _turret;
        
        private Core.Pool.ObjectPool _projectilePool;
        [SerializeField] private ParabolicProjectile _parabolicProjectile;
        private TurretMediator _turretMediator;
        
        private void Start()
        {
            _projectilePool = new ObjectPool(_parabolicProjectile, _transform);
            _projectilePool.Init(5);
        }

        public void Die()
        {
            InvokeOnDeathComplete();
            Recycle();
        }
        private void InstantiateTurret()
        {
            GameObject go = Instantiate(_turret, _transform);
            _turretMediator = go.GetComponent<TurretMediator>();
            _turretMediator.SetTurretMind(this);
            _turretMediator.SetObjectPool(_projectilePool);
            if (_attackTarget != null)
            {
                _turretMediator.SetPlayerTransform(_attackTarget);
            }
            else
            {
                _turretMediator.SetPlayerTransform(ServiceLocator.Instance.GetService<IGameReferences>().GetPlayer());
            }
            
            _turretMediator.Init();
        }

        internal override void Init()
        {
            InstantiateTurret();
        }

        internal override void Release()
        {
            
        }
    }
}
