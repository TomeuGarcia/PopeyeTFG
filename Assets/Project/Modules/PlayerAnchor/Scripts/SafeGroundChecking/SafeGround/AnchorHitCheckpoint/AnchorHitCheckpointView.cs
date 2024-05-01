using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using Popeye.Scripts.ObjectTypes;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.AnchorHitCheckpoint
{
    public class AnchorHitCheckpointView : MonoBehaviour
    {
        [SerializeField] private Transform _directionComputeOrigin;

        [Header("ANCHOR")]
        [SerializeField] private Transform _anchorPointTransform;
        [SerializeField] private Ease _startEase = Ease.OutQuint;
        [SerializeField] private Ease _middleEase = Ease.InOutQuint;
        [SerializeField] private Ease _endEase = Ease.InQuint;
        [SerializeField] private AnimationCurve _anglesCurve = AnimationCurve.Linear(1,100,0,0);
        [SerializeField] private AnimationCurve _durationCurve = AnimationCurve.Linear(1,1,0,0);
        [SerializeField] private int _numberOfBounces = 7;

        [Header("TEETH")]
        [SerializeField] private Transform _clapperTransform;
        [SerializeField, Range(-2.0f, 2.0f)] private float _clapperAngleMultiplier = 0.2f;
        [SerializeField, Range(0.0f, 2.0f)] private float _clapperDurationMultiplier = 0.3f;
        
        
        private Coroutine _animationCoroutine;
        private Coroutine _bounceCoroutine;
        private Vector3 _bounceRotationAxis = Vector3.forward;
        
        
        
        private void OnValidate()
        {
            _numberOfBounces = Mathf.Max(2, _numberOfBounces);
        }
        

        
        public void ComputeBounceAxis(Vector3 hitOrigin)
        {
            Vector3 forward = Vector3.ProjectOnPlane(_directionComputeOrigin.position - hitOrigin, Vector3.up).normalized;
            _bounceRotationAxis = Vector3.Cross(forward, Vector3.up).normalized;
        }
        
        [Button()]
        public void PlayBounceAnimation()
        {
            ResetAnimation();
            _animationCoroutine = StartCoroutine(DoPlayBounceAnimation());
        }
        
        private IEnumerator DoPlayBounceAnimation()
        {
            int direction = 1;
            
            float totalBounces = _numberOfBounces - 1;

            _bounceCoroutine = StartCoroutine(
                this.DoBounce(_anglesCurve.Evaluate(0), _durationCurve.Evaluate(0), _startEase));
            yield return _bounceCoroutine;


            float t = 0f;
            float goalAngles;
            direction *= -1;
            for (int i = 1; i < _numberOfBounces - 1; ++i)
            {
                t = i / totalBounces;

                goalAngles = _anglesCurve.Evaluate(t);
                goalAngles *= direction;
                direction *= -1;

                _bounceCoroutine = StartCoroutine(
                    this.DoBounce(goalAngles, _durationCurve.Evaluate(t), _middleEase));
                yield return _bounceCoroutine;
            }
            
            
            goalAngles = _anglesCurve.Evaluate(1) * direction;

            _bounceCoroutine = StartCoroutine(
                DoBounce(goalAngles, _durationCurve.Evaluate(1), _endEase));
            yield return _bounceCoroutine;

            _animationCoroutine = _bounceCoroutine = null;
        }
        
        private IEnumerator DoBounce(float goalAngles, float duration, Ease ease)
        {
            _anchorPointTransform.DORotate(_bounceRotationAxis * goalAngles, duration)
                .SetEase(ease);
            
            _clapperTransform.DOLocalRotate(
                    _bounceRotationAxis * (goalAngles * _clapperAngleMultiplier), 
                    duration * _clapperDurationMultiplier)
                .SetEase(ease);

            yield return new WaitForSeconds(duration);
        }


        
        private void ResetAnimation()
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
                _animationCoroutine = null;
            }
            if (_bounceCoroutine != null)
            {
                StopCoroutine(_bounceCoroutine);
                _bounceCoroutine = null;
            }
            
            _anchorPointTransform.DOKill();
            _clapperTransform.DOKill();
        }
    }
}