using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.GameReferences;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops;
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
            _shieldedMediator.SetBoostDropFactory(ServiceLocator.Instance.GetService<IPowerBoostDropFactory>());
            _shieldedMediator.SetEventSystem(ServiceLocator.Instance.GetService<IEventSystemService>());
            _shieldedMediator.StartChasing();
        }

        internal override void Release()
        {

        }

        public override void SetPatrollingWaypoints(Transform[] waypoints)
        {
            _shieldedMediator.SetWayPoints(waypoints);
        }
        
        public override void DieFromOrder()
        {
            _shieldedMediator.DieFromOrder();
            Recycle();
        }
    }
}
