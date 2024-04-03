using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Timers
{
    public class Counter
    {
        private int _total;
        private int _counter;

        public int Time => _counter;
        public int Total => _total;
        
        public Counter(int total)
        {
            SetTotal(total);
            Clear();
        }

        public void SetTotal(int total)
        {
            _total = total;
        }

        public void Clear()
        {
            _counter = 0;
        }

        public void Add(int amount)
        {
            _counter += amount;        
        }
    
        public bool HasReachedTotal()
        {
            return _counter >= _total;
        }
    
        public float GetCounterRatio01()
        {
            return Mathf.Clamp01((float)_counter / _total);
        }
    
    }
}


