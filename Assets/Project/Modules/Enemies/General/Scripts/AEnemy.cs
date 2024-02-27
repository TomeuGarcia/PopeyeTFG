using System;
using Popeye.Core.Pool;
using Popeye.IDSystem;
using Popeye.Modules.Enemies.General;
using UnityEngine;

namespace Popeye.Modules.Enemies
{
    public abstract class AEnemy : RecyclableObject
    {
        protected Transform _attackTarget;
        public Action<AEnemy> OnDeathComplete;
        
        
        [Header("GENERIC")]
        [SerializeField] private EnemyID _id;
        public ID Id => _id;
        

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
        
   
    }
}
