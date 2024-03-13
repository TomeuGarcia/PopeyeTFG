using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Popeye.Modules.ValueStatSystem
{
    [RequireComponent(typeof(ImageFillBar))]
    public abstract class AValueStatBar : MonoBehaviour
    {
        [Header("COMPONENTS")]
        [Required] [SerializeField] protected ImageFillBar _imageFillBar;

        [Header("CONFIGURATION")]
        [Expandable] [Required] [SerializeField] private ImageFillBarConfig _viewConfig;


        private bool _isSubscribed;
        
        protected abstract AValueStat ValueStat { get; }


        
        [Button("Validate")]
        private void OnValidate()
        {
            if (_imageFillBar && _viewConfig)
            {
                _imageFillBar.Init(_viewConfig);
            }
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
            
            OnValidate();
    
            SubscribeToEvents();
            InstantUpdateFillImage();
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
    
    
        private void InstantUpdateFillImage()
        {
            _imageFillBar.InstantUpdateFill(ValueStat.GetValuePer1Ratio());
        }
    
        protected void UpdateFillImage()
        {
            _imageFillBar.UpdateFill(ValueStat.GetValuePer1Ratio()).Forget();
        }
        
        protected void KillAllUpdates()
        {
            _imageFillBar.KillAllUpdates();
        }
    }
}