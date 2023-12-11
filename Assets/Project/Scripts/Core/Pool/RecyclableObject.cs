using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Core.Pool
{
    public abstract class RecyclableObject : MonoBehaviour
    {
        private ObjectPool _objectPool;
        [FormerlySerializedAs("rb")] [SerializeField] private Rigidbody _rb;

        internal void Configure(ObjectPool objectPool)
        {
            _objectPool = objectPool;
        }

        public void Recycle()
        {
            if(isActiveAndEnabled)
            _objectPool.RecycleGameObject(this);
        }

        public void Shoot(Vector3 dir, float relativeSpeed)
        {
            _rb.velocity = _rb.transform.TransformDirection(dir * relativeSpeed);
        }
        internal abstract void Init();
        internal abstract void Release();
    }
}