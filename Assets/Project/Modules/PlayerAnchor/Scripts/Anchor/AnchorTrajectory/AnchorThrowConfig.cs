using System;
using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "AnchorThrowConfig", 
        menuName = ScriptableObjectsHelper.ANCHOR_ASSETS_PATH + "AnchorThrowConfig")]
    public class AnchorThrowConfig : ScriptableObject
    {
        [System.Serializable]
        public class ExtraDistanceData
        {
            [SerializeField, Range(0.0f, 20.0f)] private float _distance = 4.0f;
            [SerializeField, Range(0.01f, 10.0f)] private float _adaptInDuration = 0.5f;
            [SerializeField] private AnimationCurve _adaptEaseInCurve = AnimationCurve.Linear(0,0,1,1);
            [SerializeField, Range(0.01f, 10.0f)] private float _adaptOutDuration = 0.5f;
            [SerializeField] private AnimationCurve _adaptEaseOutCurve = AnimationCurve.Linear(0,0,1,1);
            [SerializeField, Range(0.0f, 360.0f)] private float _angleToConsider = 180.0f;

            public float Distance => _distance;
            public float AdaptInDuration => _adaptInDuration;
            public AnimationCurve AdaptEaseInCurve => _adaptEaseInCurve;
            public float AdaptOutDuration => _adaptOutDuration;
            public AnimationCurve AdaptEaseOutCurve => _adaptEaseOutCurve;

            public float DotToConsider { get; private set; }

            public void OnValidate()
            {
                DotToConsider = Mathf.Cos((_angleToConsider / 2) * Mathf.Deg2Rad);
            }
        }
        
        
        [Header("FORCE")]
        [SerializeField] private AnimationCurve _throwForceCurve;
        
        public AnimationCurve ThrowForceCurve => _throwForceCurve;

        
        [Header("DURATIONS")]
        [SerializeField, Range(0.0f, 10.0f)] private float _maxThrowForceChargeDuration = 1.0f;
        [SerializeField, Range(0.01f, 10.0f)] private float _minThrowMoveDuration = 0.2f;
        [SerializeField, Range(0.01f, 10.0f)] private float _maxThrowMoveDuration = 0.5f;
        
        public float MaxThrowForceChargeDuration => _maxThrowForceChargeDuration;
        public float MinThrowMoveDuration => _minThrowMoveDuration;
        public float MaxThrowMoveDuration => _maxThrowMoveDuration;
        
        
        [Header("DISTANCES")]
        [SerializeField, Range(0.0f, 20.0f)] private float _minThrowDistance = 2.0f;
        [SerializeField, Range(0.0f, 20.0f)] private float _maxThrowDistance = 7.0f;

        public float MinThrowDistance => _minThrowDistance;
        public float MaxThrowDistance => _maxThrowDistance;

        
        [Header("EXTRA DISTANCE")] 
        [SerializeField] private ExtraDistanceData _movingForwardExtraDistanceData;
        public ExtraDistanceData MovingForwardExtraDistanceData => _movingForwardExtraDistanceData;


        [Header("MOVEMENT")] 
        [SerializeField] private AnimationCurve _moveInterpolationCurve;
        [SerializeField] private AnimationCurve _rotateInterpolationCurve;
        
        public AnimationCurve MoveInterpolationCurve => _moveInterpolationCurve;        
        public AnimationCurve RotateInterpolationCurve => _rotateInterpolationCurve;
        
        

        [Header("TRAJECTORY OFFSET")]
        [SerializeField] private AnimationCurve _heightDisplacementCurve;
        
        public AnimationCurve HeightDisplacementCurve => _heightDisplacementCurve;


        [Header("END ROTATION CORRECTION")]

        [SerializeField] private AnimationCurve _endRotationWeightCurve;
        [SerializeField, Range(0f, 1f)] private float _correctionAmount = 0.8f;
        public AnimationCurve EndRotationWeightCurve => _endRotationWeightCurve;
        public float CorrectionAmount => _correctionAmount;
        
        

        private void OnValidate()
        {
            _maxThrowDistance = Mathf.Max(_maxThrowDistance, _minThrowDistance);
            _maxThrowMoveDuration = Mathf.Max(_maxThrowMoveDuration, _minThrowMoveDuration);
            
            _movingForwardExtraDistanceData.OnValidate();
        }

        private void Awake()
        {
            OnValidate();
        }
    }
}