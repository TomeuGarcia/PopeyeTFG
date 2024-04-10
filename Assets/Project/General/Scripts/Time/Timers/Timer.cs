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
        public float Duration => _duration;
        
        public Timer(float duration)
        {
            SetDuration(duration);
            Clear();
        }

        public void SetDuration(float newDuration)
        {
            _duration = Mathf.Max(newDuration, 0.00000001f);
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
            return Mathf.Clamp01(_counter / _duration);
        }
    
    }
}


