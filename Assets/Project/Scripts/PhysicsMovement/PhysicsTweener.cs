using System.Collections.Generic;
using Popeye.Timers;
using UnityEngine;

namespace Project.PhysicsMovement
{
    public class PhysicsTweener
    {
        private List<PhysicsTweenObject> _tweenObjects;

        
        public PhysicsTweener(int tweenObjectsCapacity)
        {
            _tweenObjects = new List<PhysicsTweenObject>(tweenObjectsCapacity);
        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            for (int i = 0; i < _tweenObjects.Count; ++i)
            {
                if (_tweenObjects[i].FixedUpdate(fixedDeltaTime))
                {
                    _tweenObjects.RemoveAt(i);
                    --i;
                }
            }
        }

        public void AddObject(PhysicsTweenObject physicsTweenObject)
        {
            _tweenObjects.Add(physicsTweenObject);
        }
        
    }
}