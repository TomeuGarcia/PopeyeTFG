using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Services.GameReferences;
using Popeye.Core.Services.ServiceLocator;
using UnityEngine;

namespace Popeye.Modules.Enemies
{
    public class ShieldedMind : AEnemy
    {
        [SerializeField] private ShieldedMediator _shieldedMediator;

        
        public void Die()
        {
            InvokeOnDeathComplete();
            Recycle();
        }
        internal override void Init()
        {
            _shieldedMediator.SetShieldedMind(this);
            if (_attackTarget != null)
            {
                _shieldedMediator.SetPlayerTransform(_attackTarget);
            }
            else
            {
                _shieldedMediator.SetPlayerTransform(ServiceLocator.Instance.GetService<IGameReferences>().GetPlayerTargetForEnemies());
            }
            
            _shieldedMediator.Init();
        }

        internal override void Release()
        {

        }

        public override void SetPatrollingWaypoints(Transform[] waypoints)
        {
 
        }

        public override void DieFromOrder()
        {
            _shieldedMediator.DieFromOrder();
            Recycle();
        }
    }
}
