using System;
using Project.Scripts.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "AnchorThrowConfig", 
        menuName = ScriptableObjectsHelper.ANCHOR_ASSETS_PATH + "AnchorThrowConfig")]
    public class AnchorThrowConfig : ScriptableObject
    {
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


        [Header("MOVEMENT")] 
        [SerializeField] private AnimationCurve _moveInterpolationCurve;
        [SerializeField] private AnimationCurve _rotateInterpolationCurve;
        
        public AnimationCurve MoveInterpolationCurve => _moveInterpolationCurve;        
        public AnimationCurve RotateInterpolationCurve => _rotateInterpolationCurve;
        
        

        [Header("TRAJECTORY OFFSET")]
        [SerializeField] private AnimationCurve _heightDisplacementCurve;
        
        public AnimationCurve HeightDisplacementCurve => _heightDisplacementCurve;
        
        
        
        

        private void OnValidate()
        {
            _maxThrowDistance = Mathf.Max(_maxThrowDistance, _minThrowDistance);
            _maxThrowMoveDuration = Mathf.Max(_maxThrowMoveDuration, _minThrowMoveDuration);
        }

        private void Awake()
        {
            OnValidate();
        }
    }
}