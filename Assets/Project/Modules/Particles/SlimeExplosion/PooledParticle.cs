using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Pool;
using UnityEngine;

public class PooledParticle : RecyclableObject
{
    internal override void Init()
    {
        Invoke(nameof(Recycle),5);
    }

    internal override void Release()
    {
       //release particle
    }
}
