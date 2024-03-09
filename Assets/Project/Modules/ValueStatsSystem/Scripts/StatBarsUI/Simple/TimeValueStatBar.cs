namespace Popeye.Modules.ValueStatSystem
{
    public class TimeValueStatBar : AValueStatBar
    {
        private ATimeValueStat _timeValueStat;

        protected override AValueStat ValueStat => _timeValueStat;


        public void Init(ATimeValueStat timeValueStat)
        {
            _timeValueStat = timeValueStat;
            BaseInit();
        }
        
        
        protected override bool HasSubscriptionReferences()
        {
            return _timeValueStat != null;
        }

        protected override void DoSubscribeToEvents()
        {
            _timeValueStat.OnValueUpdate += UpdateFillImage;
            _timeValueStat.OnValueStartUpdate += UpdateToMax;
            _timeValueStat.OnValueStopUpdate += KillAllUpdates;
        }

        protected override void DoUnsubscribeToEvents()
        {
            _timeValueStat.OnValueUpdate -= UpdateFillImage;
            _timeValueStat.OnValueStartUpdate -= UpdateToMax;
            _timeValueStat.OnValueStopUpdate -= KillAllUpdates;
        }

        private void UpdateToMax(float durationToMax)
        {
            _imageFillBar.UpdateFillToMax(durationToMax);
            
        }

    }
}