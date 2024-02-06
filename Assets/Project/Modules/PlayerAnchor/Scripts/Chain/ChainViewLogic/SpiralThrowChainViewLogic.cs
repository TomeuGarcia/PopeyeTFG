using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Player;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class SpiralThrowChainViewLogic : IChainViewLogic
    {
        private AnchorThrowResult _throwResult;
        private readonly SpiralThrowChainViewLogicConfig _logicConfig;
        
        private readonly int _chainBoneCount;
        private readonly int _chainBoneCountMinusOne;

        private readonly Vector3[] _chainPositions;

        private Vector3 _spiralUp;
        private Vector3 _spiralRight;
        private float _time;
        private float _effectMultiplierAddition;
        
        
        private float Duration => _throwResult.Duration;
        private float DurationHitObstacle => _throwResult.DurationHitObstacle;
        

        private float LoopSpread => _logicConfig.LoopSpread;
        private float MaxAmplitude => _logicConfig.MaxAmplitude;
        private float SpinSpeed =>_logicConfig.SpinSpeed;
        private float PhaseOffset =>_logicConfig.PhaseOffset;
        private AnimationCurve AmplitudeBoneWeightCurve =>_logicConfig.AmplitudeBoneWeightCurve;
        private AnimationCurve SpeedOverTimeCurve =>_logicConfig.SpeedOverTimeCurve;
        private AnimationCurve AmplitudeOverTimeCurve =>_logicConfig.AmplitudeOverTimeCurve;
        private AnimationCurve ObstacleHitMultiplierCurve =>_logicConfig.ObstacleHitMultiplierCurve;


        public SpiralThrowChainViewLogic(SpiralThrowChainViewLogicConfig logicConfig, int chainBoneCount)
        {
            _logicConfig = logicConfig;
            _chainBoneCount = chainBoneCount;
            _chainBoneCountMinusOne = _chainBoneCount - 1;

            _chainPositions = new Vector3[_chainBoneCount];
        }
        
        public void EnterSetup(AnchorThrowResult throwResult)
        {
            _throwResult = throwResult;

            _spiralUp = Vector3.up;
            _spiralRight = Vector3.Cross(throwResult.Direction, _spiralUp).normalized;
        }


        public void OnViewEnter()
        {
            _time = 0;
        }

        public void UpdateChainPositions(float deltaTime, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {

            _chainPositions[0] = playerBindPosition;
            _chainPositions[^1] = anchorBindPosition;

            Vector3 playerToAnchor = anchorBindPosition - playerBindPosition;
            float playerToAnchorDistance = playerToAnchor.magnitude;
            Vector3 playerToAnchorDirection = playerToAnchor / playerToAnchorDistance;

            float distanceStep = playerToAnchorDistance / _chainBoneCountMinusOne;
            
            _effectMultiplierAddition = _throwResult.HitsObstacle ? 
                ObstacleHitMultiplierCurve.Evaluate(Mathf.Min(_time/DurationHitObstacle, 1)) :
                0;
            
            for (int i = 1; i < _chainBoneCount - 1; ++i)
            {
                Vector3 chainBonePosition = playerBindPosition + (playerToAnchorDirection * (i * distanceStep));

                float t = i / (float)_chainBoneCountMinusOne;
                float time = _time + (t * PhaseOffset);

                float spread = t * LoopSpread - CurrentSpeedOverTime(time);
                float size = ChainBoneAmplitudeWeight(t) * CurrentAmplitudeOverTime(time) + _effectMultiplierAddition;
                
                Vector3 spiralOffset = _spiralUp * (Mathf.Sin(spread) * size) +
                                       _spiralRight * (Mathf.Cos(spread) * size);

                chainBonePosition += spiralOffset;
                
                _chainPositions[i] = chainBonePosition;
            }

            _time += deltaTime;
        }

        public void OnViewExit()
        {
            
        }

        public Vector3[] GetChainPositions()
        {
            return _chainPositions;
        }

        private float ChainBoneAmplitudeWeight(float chainT)
        {
            return AmplitudeBoneWeightCurve.Evaluate(chainT);
        }

        private float CurrentSpeedOverTime(float time)
        {
            return SpeedOverTimeCurve.Evaluate(Mathf.Min(1f, time / Duration)) * SpinSpeed * time;
        }

        private float CurrentAmplitudeOverTime(float time)
        {
            return AmplitudeOverTimeCurve.Evaluate(Mathf.Min(1f, time / Duration)) * MaxAmplitude;
        }
        
    }
}