using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Popeye.Modules.ValueStatSystem.Segmented
{
    public abstract class ASegmentedValueStatBar : MonoBehaviour
    {
        [Header("COMPONENTS")] 
        [Required] [SerializeField] private RectTransform _barsHolder;
        [Required] [SerializeField] private GridLayoutGroup _barsGridLayoutGroup;

        [Header("CONFIGURATION")]
        [Required] [SerializeField] private ImageFillBar _imageFillBarPrefab;
        [Expandable] [Required] [SerializeField] private SegmentedValueStatBarConfig _config;
        [Expandable] [Required] [SerializeField] private ImageFillBarConfig _viewConfig;
        
        
        
        protected ImageFillBar[] _imageFillBars;
        protected int _currentBarIndex;

        private bool _isSubscribed;
        
        protected abstract AValueStat ValueStat { get; }
        
        protected int NumberOfSegments => _imageFillBars.Length;
        

        /*
        [Button("Validate")]
        private void OnValidate()
        {
            if (_imageFillBar && _viewConfig)
            {
                _imageFillBar.Init(_viewConfig);
            }
        }
        */
    
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
            
            //OnValidate();
            _config.Init();
            InstantiateDefaultSegments();
    
            SubscribeToEvents();
            InstantUpdateSegments();
        }

        private void InstantiateDefaultSegments()
        {
            int numberOfSegments = _config.NumberOfSegments(ValueStat.MaxValue, out int reminder);

            _imageFillBars = new ImageFillBar[numberOfSegments];
            for (int i = 0; i < numberOfSegments; ++i)
            {
                ImageFillBar imageFillBar = Instantiate(_imageFillBarPrefab, _barsGridLayoutGroup.transform);
                imageFillBar.Init(_viewConfig);
                _imageFillBars[i] = imageFillBar;
            }

            SetupBarsHolder();
            
            _currentBarIndex = CurrentValueToBarIndex();
        }


        protected abstract bool HasSubscriptionReferences();
        protected abstract void DoSubscribeToEvents();
        protected abstract void DoUnsubscribeToEvents();

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
    
    
        private void InstantUpdateSegments()
        {
            _currentBarIndex = CurrentValueToBarIndex();
            
            for (int i = 0; i <= _currentBarIndex; ++i)
            {
                _imageFillBars[i].InstantUpdateFill(1);
            }
            for (int i = _currentBarIndex+1; i < _imageFillBars.Length; ++i)
            {
                _imageFillBars[i].InstantUpdateFill(0);
            }
        }
    
        protected void UpdateSegments()
        {
            int newBarIndex = CurrentValueToBarIndex();
            
            DoUpdateSegments(_currentBarIndex, newBarIndex).Forget();
            
            _currentBarIndex = newBarIndex;
        }
        
        private async UniTaskVoid DoUpdateSegments(int currentBarIndex, int newBarIndex)
        {
            bool isSubtracting = newBarIndex < _currentBarIndex;

            if (isSubtracting)
            {
                for (int i = currentBarIndex; i > newBarIndex; --i)
                {
                    await _imageFillBars[i].UpdateFill(0);
                }
            }
            else
            {
                for (int i = currentBarIndex + 1; i <= newBarIndex; ++i)
                {
                    await _imageFillBars[i].UpdateFill(1);
                }
            }
        } 
        
        
        protected virtual void KillAllUpdates()
        {
            for (int i = 0; i < _imageFillBars.Length; ++i)
            {
                _imageFillBars[i].KillAllUpdates();
            }
        }


        private int CurrentValueToBarIndex()
        {
            return _config.IndexOfSegment(ValueStat.GetValue());
        }

        private void SetupBarsHolder()
        {
            Rect barsHolderRect = _barsHolder.rect;
            
            _barsGridLayoutGroup.cellSize = _config.ComputeCellSize(NumberOfSegments, barsHolderRect);
            _barsGridLayoutGroup.spacing = _config.ComputeSpacingBetweenCells(NumberOfSegments, barsHolderRect);
            _barsGridLayoutGroup.padding = _config.ComputePaddingCells(barsHolderRect);
        }


        protected void OnMaxValueUpdated()
        {
            foreach (ImageFillBar imageFillBar in _imageFillBars)
            {
                Destroy(imageFillBar.gameObject);
            }

            InstantiateDefaultSegments();
            InstantUpdateSegments();
        }
        
    }
}