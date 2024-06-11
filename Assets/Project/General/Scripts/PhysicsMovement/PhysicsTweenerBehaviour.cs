using System;
using UnityEngine;

namespace Project.PhysicsMovement
{
    public class PhysicsTweenerBehaviour : MonoBehaviour
    {
        [SerializeField, Min(1)] private int _tweenObjectsCapacity = 10;
        private PhysicsTweenUpdater _physicsTweenUpdater;

        private void Awake()
        {
            _physicsTweenUpdater = new PhysicsTweenUpdater(_tweenObjectsCapacity);
        }

        private void FixedUpdate()
        {
            _physicsTweenUpdater.FixedUpdate(Time.fixedDeltaTime);
        }

        public void AddObject(PhysicsTweenObject physicsTweenObject)
        {
            _physicsTweenUpdater.AddObject(physicsTweenObject);
        }
    }
}