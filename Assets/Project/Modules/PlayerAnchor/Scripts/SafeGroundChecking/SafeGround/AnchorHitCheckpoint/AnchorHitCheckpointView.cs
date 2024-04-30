using System;
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
        [SerializeField] private ObjectTypeTriggerEnter _triggerEnter;
        [SerializeField] private Transform _anchorTransform;
        
        [SerializeField] private TweenPunchConfig _rotationPunch;
        [SerializeField] private TweenPunchConfig _scalePunch;

        [SerializeField] private Ease _startEase = Ease.OutQuint;
        [SerializeField] private Ease _middleEase = Ease.InOutQuint;
        [SerializeField] private Ease _endEase = Ease.InQuint;
        [SerializeField] private AnimationCurve _anglesCurve = AnimationCurve.Linear(1,100,0,0);
        [SerializeField] private AnimationCurve _durationCurve = AnimationCurve.Linear(1,1,0,0);
        [SerializeField] private int _numberOfBounces = 7;

        private void OnValidate()
        {
            _numberOfBounces = Mathf.Max(2, _numberOfBounces);
        }

        private void OnEnable()
        {
            _triggerEnter.OnGameObjectEnters += OnGameObjectEnters;
        }
        private void OnDisable()
        {
            _triggerEnter.OnGameObjectEnters -= OnGameObjectEnters;
        }


        private void OnGameObjectEnters(GameObject other)
        {
            PlayHitAnimation();
        }


        [Button()]
        private void PlayHitAnimation()
        {
            _anchorTransform.PunchRotation(_rotationPunch);
            _anchorTransform.PunchScale(_scalePunch);
        }

        private CancellationTokenSource _animationCancellation;
        
        [Button()]
        private async UniTaskVoid PlayBounceAnimation()
        {

            
            int direction = 1;
            
            float totalBounces = _numberOfBounces - 1;

            await DoBounce(_anglesCurve.Evaluate(0), _durationCurve.Evaluate(0), _startEase);

            float t = 0f;
            float goalAngles;
            direction *= -1;
            for (int i = 1; i < _numberOfBounces - 1; ++i)
            {
                t = i / totalBounces;

                goalAngles = _anglesCurve.Evaluate(t);
                goalAngles *= direction;
                direction *= -1;
                
                await DoBounce(goalAngles, _durationCurve.Evaluate(t), _middleEase);
            }

            goalAngles = _anglesCurve.Evaluate(1) * direction;
            await DoBounce(goalAngles, _durationCurve.Evaluate(1), _endEase);
        }
        
        private async UniTask DoBounce(float goalAngles, float duration, Ease ease)
        {            
            await _anchorTransform.DORotate(Vector3.forward * goalAngles, duration)
                .SetEase(ease)
                .AsyncWaitForCompletion();
        }
    }
}