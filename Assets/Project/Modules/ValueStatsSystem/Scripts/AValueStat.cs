
namespace Popeye.Modules.ValueStatSystem
{
    public abstract class AValueStat
    {
        public abstract int MaxValue { get; }  
        
        public delegate void ValueStatEvent();
        public ValueStatEvent OnValueUpdate;
        public ValueStatEvent OnMaxValueUpdate;
        
        protected void InvokeOnValueUpdate()
        {
            OnValueUpdate?.Invoke();
        }
        private void InvokeOnMaxValueUpdate()
        {
            OnMaxValueUpdate?.Invoke();
        }
        
        public abstract float GetValuePer1Ratio();
        public abstract int GetValue();

        public void ResetMaxValue(int maxValue, bool setValueToMax)
        {
            DoResetMaxValue(maxValue, setValueToMax);
            InvokeOnMaxValueUpdate();
        }
        protected abstract void DoResetMaxValue(int maxValue,bool setValueToMax);

    }
}


