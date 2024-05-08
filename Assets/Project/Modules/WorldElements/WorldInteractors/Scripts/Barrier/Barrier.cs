using System;
using System.Collections;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.WorldElements.WorldInteractors
{
        
    public class Barrier : AWorldInteractor
    {
        [Header("AUDIO")]        
        [SerializeField] private BarrierAudio _audio;
        [SerializeField] private GameObject _soundsSource;

        [Header("VIEW")] 
        [SerializeField] private InterfaceReference<IBarrierView, MonoBehaviour> _viewReference;
        private IBarrierView _view;

        [Header("ACTIVATED")]
        [SerializeField] private bool _activatedColliderEnabledState = true;
        [SerializeField] private Transform _activatedStateSpot;
        [SerializeField] private TweenEaseReference _activatedEase = new TweenEaseReference();

        [Space(10)]
        [Header("DEACTIVATED")]
        [SerializeField] private bool _deactivatedColliderEnabledState = false;
        [SerializeField] private Transform _deactivatedStateSpot;
        [SerializeField] private TweenEaseReference _deactivatedEase = new TweenEaseReference();
        
        [Space(30)]
        [Header("REFERENCES")]
        [SerializeField] private Transform _barrierTransform;
        [SerializeField] private Collider _collider;
        [SerializeField] private bool _startActivated = false;


        private bool _isActivated = false;

        public bool StartActivated => _startActivated;
        public Transform BarrierTransform => _barrierTransform;
        public Transform ActivatedStateSpot => _activatedStateSpot;
        public Transform DeactivatedStateSpot => _deactivatedStateSpot;
        public float ActivateDuration => _activatedEase.Value.Duration;
        public float DeactivateDuration => _deactivatedEase.Value.Duration;
        
        

        protected override void DoAwake()
        {
            _isActivated = _startActivated;
            if (_startActivated)
            {
                SetStateInstantly(_activatedStateSpot);
            }
            else
            {
                SetStateInstantly(_deactivatedStateSpot);
            }

            SetCollisionEnabled(_startActivated);

            if (_soundsSource == null)
            {
                _soundsSource = gameObject;
            }

            _view = _viewReference.Value ?? new NullBarrierView();
        }

        protected override void DoEnterActivatedState()
        {
            SetActivatedState(_activatedStateSpot, _activatedEase.Value).Forget();
            _isActivated = true;
            
            _audio.PlayActivatedSound(_soundsSource);
        }

        protected override void DoEnterDeactivatedState()
        {
            SetDeactivatedState(_deactivatedStateSpot, _deactivatedEase.Value).Forget();
            SetCollisionEnabledDelayed(false, DeactivateDuration).Forget();
            _isActivated = false;
            
            _audio.PlayDeactivatedSound(_soundsSource);
        }

        public override async UniTask EnterActivatedStateAwait()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(ActivateDuration + _view.ActivateDuration), ignoreTimeScale: true);
        }

        private void SetStateInstantly(Transform goalStateSpot)
        {
            _barrierTransform.position = goalStateSpot.position;
            _barrierTransform.rotation = goalStateSpot.rotation;
        }
        
        private async UniTaskVoid SetActivatedState(Transform goalStateSpot, TweenEaseConfig easeConfig)
        {
            await _view.PlayActivateAnimation();        
            DoSetState(goalStateSpot, easeConfig).Forget();
            SetCollisionEnabled(true);
        }
        private async UniTaskVoid SetDeactivatedState(Transform goalStateSpot, TweenEaseConfig easeConfig)
        {
            await DoSetState(goalStateSpot, easeConfig);
            await _view.PlayDeactivateAnimation();        
        }
        private async UniTask DoSetState(Transform goalStateSpot, TweenEaseConfig easeConfig)
        {
            _barrierTransform.DOMove(goalStateSpot.position, easeConfig.Duration)
                .SetUpdate(true)
                .SetEase(easeConfig);
            await _barrierTransform.DORotateQuaternion(goalStateSpot.rotation, easeConfig.Duration)
                .SetUpdate(true)
                .SetEase(easeConfig)
                .AsyncWaitForCompletion();
        }


        private void SetCollisionEnabled(bool isActivated)
        {
            if (_collider != null)
            {
                _collider.enabled = isActivated
                    ? _activatedColliderEnabledState 
                    : _deactivatedColliderEnabledState;
            }
        }
        private async UniTaskVoid SetCollisionEnabledDelayed(bool isActivated, float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), ignoreTimeScale: true);
            SetCollisionEnabled(isActivated);
        }


    }
}

