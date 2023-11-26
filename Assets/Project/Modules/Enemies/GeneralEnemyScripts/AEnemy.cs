using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEnemy : MonoBehaviour
{
    protected Transform _attackTarget;
    
    public Action<AEnemy> OnDeathComplete;
    
    public virtual void AwakeInit(Transform attackTarget)
    {
        _attackTarget = attackTarget;
    }

    protected void InvokeOnDeathComplete()
    {
        OnDeathComplete?.Invoke(this);
    }
}
