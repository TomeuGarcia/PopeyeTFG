using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "AnchorThrowConfig", 
        menuName = AnchorConfigHelper.SO_ASSETS_PATH + "AnchorThrowConfig")]
    public class AnchorThrowConfig : ScriptableObject
    {
        [Header("FORCE")]
        [SerializeField] private AnimationCurve _throwForceCurve;
        
        [Header("DURATIONS")]
        [SerializeField, Range(0.0f, 10.0f)] private float _maxThrowForceChargeDuration = 1.0f;
        [SerializeField, Range(0.01f, 10.0f)] private float _minThrowMoveDuration = 0.2f;
        [SerializeField, Range(0.01f, 10.0f)] private float _maxThrowMoveDuration = 0.5f;
        
        [Header("DISTANCES")]
        [SerializeField, Range(0.0f, 20.0f)] private float _minThrowDistance = 2.0f;
        [SerializeField, Range(0.0f, 20.0f)] private float _maxThrowDistance = 7.0f;

        
        public AnimationCurve ThrowForceCurve => _throwForceCurve;
        
        public float MaxThrowForceChargeDuration => _maxThrowForceChargeDuration;
        public float MinThrowMoveDuration => _minThrowMoveDuration;
        public float MaxThrowMoveDuration => _maxThrowMoveDuration;
        
        
        public float MinThrowDistance => _minThrowDistance;
        public float MaxThrowDistance => _maxThrowDistance;


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