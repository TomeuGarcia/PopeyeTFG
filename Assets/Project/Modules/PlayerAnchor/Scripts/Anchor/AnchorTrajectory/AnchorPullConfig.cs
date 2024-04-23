using System;
using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    
    [CreateAssetMenu(fileName = "AnchorPullConfig", 
        menuName = ScriptableObjectsHelper.ANCHOR_ASSETS_PATH + "AnchorPullConfig")]
    public class AnchorPullConfig : ScriptableObject
    {
        
        [Header("DISTANCES")]
        [SerializeField, Range(0.01f, 20.0f)] private float _minPullDistance = 2.0f;
        [SerializeField, Range(0.01f, 20.0f)] private float _maxPullDistance = 10.0f;
        
        public float MinPullDistance => _minPullDistance;
        public float MaxPullDistance => _maxPullDistance;
        
        
        [Header("MOVEMENT")] 
        [SerializeField] private AnimationCurve _moveInterpolationCurve;
        [SerializeField] private AnimationCurve _rotateInterpolationCurve;
        public AnimationCurve MoveInterpolationCurve => _moveInterpolationCurve;
        public AnimationCurve RotateInterpolationCurve => _rotateInterpolationCurve;
        
        
        [Header("DURATIONS")]
        [SerializeField, Range(0.01f, 10.0f)] private float _minPullMoveDuration = 0.2f;
        [SerializeField, Range(0.01f, 10.0f)] private float _maxPullMoveDuration = 0.5f;
        
        public float MinPullMoveDuration => _minPullMoveDuration;
        public float MaxPullMoveDuration => _maxPullMoveDuration;
        
        
        
        [Header("PULL TRAJECTORY BEND")]
        [SerializeField, Range(2, 16)] private int _trajectoryBendSharpness = 5;
        public int TrajectoryBendSharpness => _trajectoryBendSharpness;

        
        [Header("STRAIGHT vs CHAIN trajectory by distance")]
        [SerializeField] private AnimationCurve _trajectoryByDistance = AnimationCurve.Linear(0,0,1,1);
        public AnimationCurve TrajectoryByDistance => _trajectoryByDistance;

        
        private void OnValidate()
        {
            _maxPullDistance = Mathf.Max(_maxPullDistance, _minPullDistance);
        }
    }
}