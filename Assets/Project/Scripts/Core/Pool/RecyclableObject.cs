using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Core.Pool
{
    public abstract class RecyclableObject : MonoBehaviour
    {
        private ObjectPool _objectPool;

        internal void Configure(ObjectPool objectPool)
        {
            _objectPool = objectPool;
        }

        public void Recycle()
        {
            _objectPool.RecycleGameObject(this);
        }
        internal abstract void Init();
        internal abstract void Release();
    }
}