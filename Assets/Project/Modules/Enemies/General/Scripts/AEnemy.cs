using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using Popeye.IDSystem;
using UnityEngine;

namespace Popeye.Modules.Enemies
{
    public class AEnemy : MonoBehaviour
    {
        protected Transform _attackTarget;
        public Action<AEnemy> OnDeathComplete;
        
        
        [Header("GENERIC")]
        [SerializeField] private IDAsset _id;
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
