namespace Popeye.Modules.ValueStatSystem.Segmented
{
    public class SegmentedValueStatBar : ASegmentedValueStatBar
    {
        private AValueStat _valueStat;
        protected override AValueStat ValueStat => _valueStat;
        
        

        public void Init(AValueStat valueStat)
        {
            _valueStat = valueStat;
            BaseInit();
        }
        
        
        protected override bool HasSubscriptionReferences()
        {
            return _valueStat != null;
        }

        protected override void DoSubscribeToEvents()
        {
            _valueStat.OnValueUpdate += UpdateSegments;
        }

        protected override void DoUnsubscribeToEvents()
        {
            _valueStat.OnValueUpdate -= UpdateSegments;
        }
    }
}