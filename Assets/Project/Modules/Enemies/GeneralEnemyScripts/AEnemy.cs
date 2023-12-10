using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.Enemies
{
    public class AEnemy : MonoBehaviour
    {
        protected Transform _attackTarget;
        public Action<AEnemy> OnDeathComplete;
        [SerializeField] private string _id;
        public string Id => _id;
        

        public virtual void AwakeInit(Transform attackTarget)
        {
            _attackTarget = attackTarget;
        }

        protected void InvokeOnDeathComplete()
        {
            OnDeathComplete?.Invoke(this);
        }
    }
}
