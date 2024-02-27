using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Pool;
using Popeye.Core.Services.GameReferences;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Enemies.Hazards;
using UnityEngine;
using UnityEngine.Pool;

namespace Popeye.Modules.Enemies
{
    public class TurretMindEnemy : AEnemy
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private ParabolicProjectile _parabolicProjectile;
        [SerializeField] private TurretMediator _turretMediator;
        


        public void Die()
        {
            InvokeOnDeathComplete();
            Recycle();
        }
        private void InitTurret()
        {
            _turretMediator.SetTurretMind(this);
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
            
        }

        internal override void Release()
        {
            
        }

        public override void SetPatrollingWaypoints(Transform[] waypoints)
        {
            
        }

        public override void InitAfterSpawn(IHazardFactory hazardFactory)
        {
            base.InitAfterSpawn(hazardFactory);
            _turretMediator.SetHazardFactory(hazardFactory);
            InitTurret();
        }
    }
}
