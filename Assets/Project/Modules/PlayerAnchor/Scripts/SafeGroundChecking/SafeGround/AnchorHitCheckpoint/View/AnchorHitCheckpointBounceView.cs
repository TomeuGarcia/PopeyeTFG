using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.AnchorHitCheckpoint
{
    public class AnchorHitCheckpointBounceView : MonoBehaviour
    {
        [Header("BOUNCE ANIMATION")]
        [SerializeField] private Transform _directionComputeOrigin;
        [SerializeField] private Transform _anchorPointTransform;
        [SerializeField] private Transform _clapperTransform;
        
        private Coroutine _animationCoroutine;
        private Coroutine _bounceCoroutine;
        private Vector3 _bounceRotationAxis = Vector3.forward;

        private AnchorHitCheckpointViewConfig.BouncesViewConfig _bouncesConfig;
        
        public void Configure(AnchorHitCheckpointViewConfig.BouncesViewConfig bouncesConfig)
        {
            _bouncesConfig = bouncesConfig;
        }
        
        
        public void ComputeBounceAxis(Vector3 hitOrigin)
        {
            Vector3 forward = Vector3.ProjectOnPlane(_directionComputeOrigin.position - hitOrigin, Vector3.up).normalized;
            _bounceRotationAxis = Vector3.Cross(forward, Vector3.up).normalized;
        }
        
        public void PlayBounceAnimation()
        {
            ResetAnimation();
            _animationCoroutine = StartCoroutine(DoPlayBounceAnimation());
        }
        
        private IEnumerator DoPlayBounceAnimation()
        {
            int direction = 1;
            
            float totalBounces = _bouncesConfig.NumberOfBounces - 1;

            _bounceCoroutine = StartCoroutine(DoBounce(
                    _bouncesConfig.AnglesCurve.Evaluate(0), 
                    _bouncesConfig.DurationCurve.Evaluate(0), 
                    _bouncesConfig.StartEase));
            yield return _bounceCoroutine;


            float t = 0f;
            float goalAngles;
            direction *= -1;
            for (int i = 1; i < _bouncesConfig.NumberOfBounces - 1; ++i)
            {
                t = i / totalBounces;

                goalAngles = _bouncesConfig.AnglesCurve.Evaluate(t);
                goalAngles *= direction;
                direction *= -1;

                _bounceCoroutine = StartCoroutine(DoBounce(
                    goalAngles, 
                    _bouncesConfig.DurationCurve.Evaluate(t), 
                    _bouncesConfig.MiddleEase));
                yield return _bounceCoroutine;
            }
            
            
            goalAngles = _bouncesConfig.AnglesCurve.Evaluate(1) * direction;

            _bounceCoroutine = StartCoroutine(DoBounce(
                goalAngles, 
                _bouncesConfig.DurationCurve.Evaluate(1), 
                _bouncesConfig.EndEase));
            yield return _bounceCoroutine;

            _animationCoroutine = _bounceCoroutine = null;
        }
        
        private IEnumerator DoBounce(float goalAngles, float duration, Ease ease)
        {
            _anchorPointTransform.DORotate(_bounceRotationAxis * goalAngles, duration)
                .SetEase(ease);
            
            _clapperTransform.DOLocalRotate(
                    _bounceRotationAxis * (goalAngles * _bouncesConfig.ClapperAngleMultiplier), 
                    duration * _bouncesConfig.ClapperDurationMultiplier)
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