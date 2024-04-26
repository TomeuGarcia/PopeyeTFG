using UnityEngine;

namespace Popeye.Modules.ValueStatSystem
{
    public class SimpleValueStat : AValueStat
    {
        private int _maxValue;
        private int _currentValue;

        public SimpleValueStat(int maxValue, int startCurrentValue)
        {
            _maxValue = maxValue;
            _currentValue = startCurrentValue;
        }

        public override int MaxValue => _maxValue;
        public override float GetValuePer1Ratio()
        {
            return (float)_currentValue / MaxValue;
        }

        public override int GetValue()
        {
            return _currentValue;
        }

        protected override void DoResetMaxValue(int maxValue, bool setValueToMax)
        {
            _maxValue = maxValue;
            SetCurrentValue(setValueToMax ? _maxValue : _currentValue);
        }

        public void SetCurrentValue(int currentValue)
        {
            _currentValue = Mathf.Clamp(currentValue, 0, _maxValue);
            InvokeOnValueUpdate();
        }
        
    }
}