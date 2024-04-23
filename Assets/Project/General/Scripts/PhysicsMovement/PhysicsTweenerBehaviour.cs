using System;
using UnityEngine;

namespace Project.PhysicsMovement
{
    public class PhysicsTweenerBehaviour : MonoBehaviour
    {
        [SerializeField, Min(1)] private int _tweenObjectsCapacity = 10;
        private PhysicsTweener _physicsTweener;

        private void Awake()
        {
            _physicsTweener = new PhysicsTweener(_tweenObjectsCapacity);
        }

        private void FixedUpdate()
        {
            _physicsTweener.FixedUpdate(Time.fixedDeltaTime);
        }

        public void AddObject(PhysicsTweenObject physicsTweenObject)
        {
            _physicsTweener.AddObject(physicsTweenObject);
        }
    }
}