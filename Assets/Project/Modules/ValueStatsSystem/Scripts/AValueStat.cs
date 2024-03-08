using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.ValueStatSystem
{
    public abstract class AValueStat
    {
        public abstract int MaxValue { get; }  
        
        public delegate void ValueStatEvent();
        public ValueStatEvent OnValueUpdate;
        
        protected void InvokeOnValueUpdate()
        {
            OnValueUpdate?.Invoke();
        }
        
        public abstract float GetValuePer1Ratio();

    }
}


