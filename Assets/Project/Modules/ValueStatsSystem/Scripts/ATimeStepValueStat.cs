namespace Popeye.Modules.ValueStatSystem
{
    public abstract class ATimeStepValueStat : AValueStat
    {
        public delegate void TimeStepValueStatEvent();
        public TimeStepValueStatEvent OnValueStepRestored;


        protected void InvokeOnValueStepRestored()
        {
            OnValueStepRestored?.Invoke();
        }

    }
}