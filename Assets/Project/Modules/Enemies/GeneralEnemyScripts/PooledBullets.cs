using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Pool;
using UnityEngine;

public class PooledBullets : RecyclableObject
{
    [SerializeField] private float _lifeTime;
    internal override void Init()
    {
        Invoke(nameof(Recycle),_lifeTime);
    }

    internal override void Release()
    {
        //release particle
    }
}
