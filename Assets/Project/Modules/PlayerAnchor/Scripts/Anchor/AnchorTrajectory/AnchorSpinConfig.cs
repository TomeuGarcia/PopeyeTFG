using System;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    
    [CreateAssetMenu(fileName = "AnchorSpinConfig", 
        menuName = ScriptableObjectsHelper.ANCHOR_ASSETS_PATH + "AnchorSpinConfig")]
    public class AnchorSpinConfig : ScriptableObject
    {
        [SerializeField, Range(0.0f, 20.0f)] private float _spinRadius = 5.0f;
        [SerializeField, Range(0.01f, 10.0f)] private float _durationPerSpin = 0.5f;
        
        public float SpinRadius => _spinRadius;
        public float SpinSpeed  { get; private set; }
        
        
        [Header("SPIN START")]
        [SerializeField, Range(0.01f, 10.0f)] private float _spinStartDuration = 0.25f;
        [SerializeField] private AnimationCurve _spinStartEase 
            = AnimationCurve.Linear(0, 0, 1, 1);

        public float SpinStartSpeed { get; private set; }
        public float SpinStartDuration => _spinStartDuration;
        public AnimationCurve SpinStartEase => _spinStartEase;
        
        
        [Header("SPIN STOP")]
        [SerializeField, Range(0.01f, 10.0f)] private float _spinStopDuration = 0.5f;
        [SerializeField] private AnimationCurve _spinStopEase 
            = AnimationCurve.Linear(0, 0, 1, 1);
        
        public float SpinStopSpeed { get; private set; }
        public float SpinStopDuration => _spinStopDuration;
        public AnimationCurve SpinStopEase => _spinStopEase;

        

        private void OnValidate()
        {
            SpinSpeed = SpinDurationToSpinSpeed(_durationPerSpin);
            SpinStartSpeed = SpinDurationToSpinSpeed(_spinStartDuration);
            SpinStopSpeed = SpinDurationToSpinSpeed(_spinStopDuration);
        }

        private void Awake()
        {
            OnValidate();
        }


        private float SpinDurationToSpinSpeed(float spinDuration)
        {
            return (2 * Mathf.PI) / spinDuration;
        }

        
    }
}