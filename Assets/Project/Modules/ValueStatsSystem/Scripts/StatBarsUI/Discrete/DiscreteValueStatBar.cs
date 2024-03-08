using UnityEngine;

namespace Popeye.Modules.ValueStatSystem
{
    public class DiscreteValueStatBar : MonoBehaviour
    {
        [SerializeField] private DiscreteValueStatBarElement _barElementPrefab;
        [SerializeField] private RectTransform _barsHolder;

        [SerializeField] private ValueStatBarViewConfig _viewConfig;
        
        [SerializeField, Range(1, 100)] private int _statPointsPerBar = 20;
        
        private DiscreteValueStatBarElement[] _bars;
        private int _currentBarIndex;


        private bool _isSubscribed;
        
        
        private AValueStat _valueStat;
        //protected override AValueStat ValueStat => _valueStat;

        public void Init(AValueStat valueStat)
        {
            _valueStat = valueStat;

            int numberOfBars = _valueStat.MaxValue / _statPointsPerBar;
            _bars = new DiscreteValueStatBarElement[numberOfBars];
            
            for (int i = 0; i < numberOfBars; ++i)
            {
                DiscreteValueStatBarElement bar = Instantiate(_barElementPrefab, _barsHolder);
                bar.Init(_viewConfig);
                _bars[i] = bar;
            }

            _currentBarIndex = numberOfBars - 1;
        }
            
        
    
        private void OnEnable()
        {
            if (HasSubscriptionReferences())
            {
                SubscribeToEvents();
            }
        }
    
        private void OnDisable()
        {
            if (HasSubscriptionReferences())
            {
                UnsubscribeToEvents();
            }
        }


        protected void BaseInit()
        {
            _isSubscribed = false;
            
            SubscribeToEvents();
            InstantUpdateState();
        }
        
        protected bool HasSubscriptionReferences()
        {
            return _valueStat != null;
        }

        private void SubscribeToEvents()
        {
            if (_isSubscribed) return;
            _isSubscribed = true;
    
            DoSubscribeToEvents();
        }
        private void UnsubscribeToEvents()
        {
            if (!_isSubscribed) return;
            _isSubscribed = false;
    
            DoUnsubscribeToEvents();
        }
        
        protected void DoSubscribeToEvents()
        {
            _valueStat.OnValueUpdate += UpdateState;
        }

        protected void DoUnsubscribeToEvents()
        {
            _valueStat.OnValueUpdate -= UpdateState;
        }
        
        
        
        private void InstantUpdateState()
        {
            int newCurrentBarIndex = StatRatioToBarIndex();

            // TODO
        }

        protected void UpdateState()
        {
            int newCurrentBarIndex = StatRatioToBarIndex();
            bool isIncrementing = _currentBarIndex < newCurrentBarIndex;
            
            // TODO
        }


        private int StatRatioToBarIndex()
        {
            return (int)(_valueStat.GetValuePer1Ratio() * (_bars.Length - 1));
        }
        
    }
}