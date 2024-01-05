using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Timers
{
    public class Timer
    {
        private float _duration;
        private float _counter;

        public float Time => _counter;
        
        public Timer(float duration)
        {
            SetDuration(duration);
            Clear();
        }

        public void SetDuration(float newDuration)
        {
            _duration = newDuration;
        }

        public void Clear()
        {
            _counter = 0;
        }

        public void Update(float deltaTime)
        {
            _counter += deltaTime;        
        }
    
        public bool HasFinished()
        {
            return _counter >= _duration;
        }
    
        public float GetCounterRatio01()
        {
            return Mathf.Min(1.0f, _counter / _duration);
        }
    
    }
}


