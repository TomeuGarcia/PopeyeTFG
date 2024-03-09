namespace Popeye.Modules.ValueStatSystem.Segmented
{
    public class TimeStepSegmentedValueStatBar : ASegmentedValueStatBar
    {
        private ATimeStepValueStat _timeStepValueStat;

        protected override AValueStat ValueStat => _timeStepValueStat;



        public void Init(ATimeStepValueStat timeStepValueStat)
        {
            _timeStepValueStat = timeStepValueStat;
            BaseInit();
        }
        
        
        protected override bool HasSubscriptionReferences()
        {
            return _timeStepValueStat != null;
        }

        protected override void DoSubscribeToEvents()
        {
            _timeStepValueStat.OnValueUpdate += UpdateSegments;
            _timeStepValueStat.OnValueStepRestored += UpdateSegments;
        }

        protected override void DoUnsubscribeToEvents()
        {
            _timeStepValueStat.OnValueUpdate -= UpdateSegments;
            _timeStepValueStat.OnValueStepRestored -= UpdateSegments;
        }
        
        
        
    }
}