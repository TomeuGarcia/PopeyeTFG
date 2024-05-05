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
        protected int _currentBarValue;

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
            int numberOfSegments = ComputeNumberOfSegments(out int reminder);

            _imageFillBars = new ImageFillBar[numberOfSegments];
            for (int i = 0; i < numberOfSegments; ++i)
            {
                ImageFillBar imageFillBar = Instantiate(_imageFillBarPrefab, _barsGridLayoutGroup.transform);
                imageFillBar.Init(_viewConfig);
                _imageFillBars[i] = imageFillBar;
            }

            SetupBarsHolder();
            
            _currentBarIndex = CurrentValueToBarIndex();
            _currentBarValue = ValueStat.GetValue();
        }

        private int ComputeNumberOfSegments(out int reminder)
        {
            return _config.NumberOfSegments(ValueStat.MaxValue, out reminder);
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
            _currentBarValue = ValueStat.GetValue();
            
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
            _currentBarValue = ValueStat.GetValue();
        }
        
        private async UniTaskVoid DoUpdateSegments(int currentBarIndex, int newBarIndex)
        {
            float currentLocalValue = (currentBarIndex + 1) + _imageFillBars[currentBarIndex].Value;
            float newLocalValue = ValueStat.GetValuePer1Ratio() * NumberOfSegments;
            int newValue = ValueStat.GetValue();
            int currentValue = _currentBarValue;
            
            
            /*
            bool isSubtracting = newBarIndex < currentBarIndex;
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
            */
            
            
            
            
            bool isSubtracting = newValue < currentValue;
            /*
            Debug.Log("newLocalValue: " + newLocalValue);
            Debug.Log("currentLocalValue: " + currentLocalValue);
            if (isSubtracting)
            {
                int i = currentBarIndex;
                float f = newLocalValue - currentLocalValue;
                for (; f < 0f; f += 1f, --i)
                {
                    Debug.Log("dec: " + i + " " + f);
                    await _imageFillBars[i].UpdateFill(0);
                }
                if (f > 0)
                {
                    Debug.Log("dec n: " + i + " " + f);
                    await _imageFillBars[i].UpdateFill(f);
                }
            }
            else
            {
                int i = currentBarIndex + 1;
                float f = newLocalValue - currentLocalValue;
                
                for (; f > 1f; f -= 1f, ++i)
                {
                    Debug.Log("inc: " + i + " " + f);
                    await _imageFillBars[i].UpdateFill(1);
                }
                if (f > 0)
                {
                    Debug.Log("inc n: " + i + " " + f);
                    await _imageFillBars[i].UpdateFill(f);
                }
            }
            */

            int i = Mathf.Max(0,currentValue-1) / _config.StatValueAmountPerUnit;
            if (!isSubtracting)
            {
                int v = newValue - currentValue;
                Debug.Log(v);
                if (v % _config.StatValueAmountPerUnit == 0)
                {
                    ++i; // no acaba d'anar bé
                }

                for (; v >= _config.StatValueAmountPerUnit; v -= _config.StatValueAmountPerUnit, ++i)
                {
                    await _imageFillBars[i].UpdateFill(1);
                }

                if (v > 0)
                {
                    await _imageFillBars[i].UpdateFill((float)v/_config.StatValueAmountPerUnit);
                }
            }
            else
            {
                int v = currentValue - newValue;
                for (; v >= _config.StatValueAmountPerUnit; v -= _config.StatValueAmountPerUnit, --i)
                {
                    await _imageFillBars[i].UpdateFill(0);
                }

                if (v > 0)
                {
                    v = _config.StatValueAmountPerUnit - v;
                    await _imageFillBars[i].UpdateFill((float)v/_config.StatValueAmountPerUnit);
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
            
            _barsGridLayoutGroup.cellSize = _config.ComputeCellSize(NumberOfSegments, barsHolderRect, _barsGridLayoutGroup);
            _barsGridLayoutGroup.spacing = _config.ComputeSpacingBetweenCells(NumberOfSegments, barsHolderRect, _barsGridLayoutGroup);
            _barsGridLayoutGroup.padding = _config.ComputePaddingCells(barsHolderRect, _barsGridLayoutGroup);
        }


        protected void OnMaxValueUpdated()
        {
            DestroyCurrentSegments();
            InstantiateDefaultSegments();
            InstantUpdateSegments();
        }

        private void DestroyCurrentSegments()
        {
            foreach (ImageFillBar imageFillBar in _imageFillBars)
            {
                Destroy(imageFillBar.gameObject);
            }
        }
        
    }
}