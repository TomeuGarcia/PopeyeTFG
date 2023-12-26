namespace Popeye.Modules.ValueStatSystem
{
    public abstract class ATimeValueStat : AValueStat
    {
        public delegate void TimeValueStatEvent(float durationToMax);
        public TimeValueStatEvent OnValueStartUpdate;
        public ValueStatEvent OnValueStopUpdate;
        public ValueStatEvent OnValueExhausted;
        
        
        protected void InvokeOnValueStartUpdate(float durationToMax)
        {
            OnValueStartUpdate?.Invoke(durationToMax);
        }
        protected void InvokeOnValueStopUpdate()
        {
            OnValueStopUpdate?.Invoke();
        }
        protected void InvokeOnValueExhausted()
        {
            OnValueExhausted?.Invoke();
        }

    }
}