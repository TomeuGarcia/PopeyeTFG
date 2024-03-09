using Cysharp.Threading.Tasks;

namespace Popeye.Modules.ValueStatSystem.Segmented
{
    public class TimeSegmentedValueStatBar : ASegmentedValueStatBar
    {
        private ATimeValueStat _timeValueStat;

        protected override AValueStat ValueStat => _timeValueStat;

        private bool _updateToMaxInterrupted;


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
            _timeValueStat.OnValueUpdate += UpdateSegments;
            _timeValueStat.OnValueStartUpdate += UpdateSegmentsToMax;
            _timeValueStat.OnValueStopUpdate += CompleteAllUpdates;
        }

        protected override void DoUnsubscribeToEvents()
        {
            _timeValueStat.OnValueUpdate -= UpdateSegments;
            _timeValueStat.OnValueStartUpdate -= UpdateSegmentsToMax;
            _timeValueStat.OnValueStopUpdate -= CompleteAllUpdates;
        }

        private void UpdateSegmentsToMax(float durationToMax)
        {
            _updateToMaxInterrupted = false;
            DoUpdateSegmentsToMax(durationToMax).Forget();
        }

        private async UniTaskVoid DoUpdateSegmentsToMax(float durationToMax)
        {
            float durationToMaxPerSegment = durationToMax / (NumberOfSegments - _currentBarIndex);
            
            for (int i = _currentBarIndex + 1; i < NumberOfSegments && !_updateToMaxInterrupted; ++i)
            {
                _currentBarIndex = i;
                await _imageFillBars[i].UpdateFillToMax(durationToMaxPerSegment);
            }
        }

        protected override void CompleteAllUpdates()
        {
            base.CompleteAllUpdates();
            _updateToMaxInterrupted = true;
        }
    }
    
}