using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "AnchorThrowConfig", 
        menuName = AnchorConfigHelper.SO_ASSETS_PATH + "AnchorThrowConfig")]
    public class AnchorThrowConfig : ScriptableObject
    {
        [SerializeField] private AnimationCurve _throwForceCurve;
        [SerializeField, Range(0.0f, 10.0f)] private float _maxThrowForceChargeDuration = 1.0f;
        [SerializeField, Range(0.0f, 20.0f)] private float _minThrowDistance = 2.0f;
        [SerializeField, Range(0.0f, 20.0f)] private float _maxThrowDistance = 7.0f;
        [SerializeField, Range(0.01f, 10.0f)] private float _maxThrowMoveDuration = 0.5f;

        public AnimationCurve ThrowForceCurve => _throwForceCurve;
        public float MaxThrowForceChargeDuration => _maxThrowForceChargeDuration;
        public float MinThrowDistance => _minThrowDistance;
        public float MaxThrowDistance => _maxThrowDistance;
        public float MaxThrowMoveDuration => _maxThrowMoveDuration;


        private void OnValidate()
        {
            _maxThrowDistance = Mathf.Max(_maxThrowDistance, _minThrowDistance);
        }

        private void Awake()
        {
            OnValidate();
        }
    }
}