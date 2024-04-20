using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.WorldElements.WorldInteractors
{
        
    public class Barrier : AWorldInteractor
    {
        [Header("ACTIVATED")]
        [SerializeField] private bool _activatedColliderEnabledState = true;
        [SerializeField] private Transform _activatedStateSpot;
        [SerializeField] private TweenEaseReference _activatedEase;

        [Space(10)]
        [Header("DEACTIVATED")]
        [SerializeField] private bool _deactivatedColliderEnabledState = false;
        [SerializeField] private Transform _deactivatedStateSpot;
        [SerializeField] private TweenEaseReference _deactivatedEase;
        
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
        }

        protected override void DoEnterActivatedState()
        {
            SetState(_activatedStateSpot, ActivateDuration, _activatedEase.Value.Ease);
            SetCollisionEnabled(true);
            _isActivated = true;
        }

        protected override void DoEnterDeactivatedState()
        {
            SetState(_deactivatedStateSpot, DeactivateDuration, _deactivatedEase.Value.Ease);
            SetCollisionEnabledDelayed(false, DeactivateDuration).Forget();
            _isActivated = false;
        }

        public override async UniTask EnterActivatedStateAwait()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(ActivateDuration), ignoreTimeScale: true);
        }

        private void SetStateInstantly(Transform goalStateSpot)
        {
            _barrierTransform.position = goalStateSpot.position;
            _barrierTransform.rotation = goalStateSpot.rotation;
        }
        
        private void SetState(Transform goalStateSpot, float duration, Ease ease)
        {
            _barrierTransform.DOMove(goalStateSpot.position, duration)
                .SetUpdate(true)
                .SetEase(ease);
            _barrierTransform.DORotateQuaternion(goalStateSpot.rotation, duration)
                .SetUpdate(true)
                .SetEase(ease);
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

