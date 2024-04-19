using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Popeye.Modules.WorldElements.WorldInteractors
{
        
    public class Barrier : AWorldInteractor
    {
        [System.Serializable]
        private struct StateEaseData
        {
            
        }
        
        [Header("ACTIVATED")]
        [SerializeField] private Transform _activatedStateSpot;
        [SerializeField, Range(0.0f, 10.0f)] private float _activateDuration = 0.5f;
        [SerializeField] private bool _activatedColliderEnabledState = true;

        [Header("DEACTIVATED")]
        [SerializeField] private Transform _deactivatedStateSpot;
        [SerializeField, Range(0.0f, 10.0f)] private float _deactivateDuration = 0.5f;
        [SerializeField] private bool _deactivatedColliderEnabledState = false;
        
        [Header("REFERENCES")]
        [SerializeField] private Transform _barrierTransform;
        [SerializeField] private Collider _collider;
        [SerializeField] private bool _startActivated = false;

        private bool _isActivated = false;

        public bool StartActivated => _startActivated;
        public Transform BarrierTransform => _barrierTransform;
        public Transform ActivatedStateSpot => _activatedStateSpot;
        public Transform DeactivatedStateSpot => _deactivatedStateSpot;
        public float ActivateDuration => _activateDuration;
        public float DeactivateDuration => _deactivateDuration;
        
        

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
            SetState(_activatedStateSpot, _activateDuration);
            SetCollisionEnabled(true);
            _isActivated = true;
        }

        protected override void DoEnterDeactivatedState()
        {
            SetState(_deactivatedStateSpot, _deactivateDuration);
            SetCollisionEnabledDelayed(false, _deactivateDuration).Forget();
            _isActivated = false;
        }

        public override async UniTask EnterActivatedStateAwait()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_activateDuration), ignoreTimeScale: true);
        }

        private void SetStateInstantly(Transform goalStateSpot)
        {
            _barrierTransform.position = goalStateSpot.position;
            _barrierTransform.rotation = goalStateSpot.rotation;
        }
        
        private void SetState(Transform goalStateSpot, float duration)
        {
            _barrierTransform.DOMove(goalStateSpot.position, duration)
                .SetUpdate(true)
                .SetEase(Ease.OutBounce);
            _barrierTransform.DORotateQuaternion(goalStateSpot.rotation, duration)
                .SetUpdate(true)
                .SetEase(Ease.OutBounce);
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

