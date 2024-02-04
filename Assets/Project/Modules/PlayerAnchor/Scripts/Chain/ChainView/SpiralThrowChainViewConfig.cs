using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    [CreateAssetMenu(fileName = "SpiralThrowChainViewConfig", 
        menuName = ScriptableObjectsHelper.ANCHORCHAIN_ASSETS_PATH + "SpiralThrowChainViewConfig")]
    public class SpiralThrowChainViewConfig : ScriptableObject
    {
        [Header("CHAIN BONES")]
        [SerializeField, Range(-0.5f, 0.5f)] private float _phaseOffset = 0.01f;
        
        [Header("LOOPS")]
        [SerializeField, Range(0f, 100f)] private float _loopSpread = 50.0f;
        
        [Header("SPEED")]
        [SerializeField, Range(0f, 100f)] private float _spinSpeed = 50.0f;
        [SerializeField] private AnimationCurve _speedOverTimeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
        
        [Header("AMPLITUDE")]
        [SerializeField, Range(0f, 2f)] private float _maxAmplitude = 0.5f;
        [SerializeField] private AnimationCurve _amplitudeOverTimeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
        [SerializeField] private AnimationCurve _amplitudeBoneWeightCurve = AnimationCurve.EaseInOut(0, 1, 1, 1);
        
        [Header("MULTIPLIER")]
        [SerializeField] private AnimationCurve _obstacleHitMultiplierCurve = AnimationCurve.EaseInOut(0, 1, 1, 1);

        
        public float LoopSpread => _loopSpread;
        public float SpinSpeed => _spinSpeed;
        public float MaxAmplitude => _maxAmplitude;
        public float PhaseOffset => _phaseOffset;

        public AnimationCurve SpeedOverTimeCurve => _speedOverTimeCurve;
        public AnimationCurve AmplitudeOverTimeCurve => _amplitudeOverTimeCurve;
        public AnimationCurve AmplitudeBoneWeightCurve => _amplitudeBoneWeightCurve;
        public AnimationCurve ObstacleHitMultiplierCurve => _obstacleHitMultiplierCurve;
    }
}