using System;
using Popeye.Core.Pool;
using Popeye.IDSystem;
using Popeye.Modules.Enemies.General;
using Popeye.Modules.Enemies.Hazards;
using Popeye.Scripts.Core.Scenes;
using UnityEngine;

namespace Popeye.Modules.Enemies
{
    public abstract class AEnemy : SceneTrackableRecyclableObject
    {
        protected Transform _attackTarget;
        public Action<AEnemy> OnDeathComplete;
        protected IHazardFactory _hazardFactory;
        private ISceneReference _belongingScene;
        
        [Header("GENERIC")]
        [SerializeField] private EnemyID _id;
        public ID Id => _id;
        
        
        
        public abstract void SetPatrollingWaypoints(Transform[] waypoints);
        public virtual void AwakeInit(Transform attackTarget)
        {
            _attackTarget = attackTarget;
        }

        public void SetAttackTarget(Transform attackTarget)
        {
            _attackTarget = attackTarget;
        }
        
        protected void InvokeOnDeathComplete()
        {
            OnDeathComplete?.Invoke(this);
        }

        public virtual void InitAfterSpawn(IHazardFactory hazardFactory)
        {
            _hazardFactory = hazardFactory;
        }

        public abstract void DieFromOrder();
        

        public override void OnBelongSceneWasUnloaded()
        {
            DieFromOrder();
        }
    }
}
