using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Player;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class SpiralThrowChainView : IChainView
    {
        private AnchorThrowResult _throwResult;
        private readonly LineRenderer _chainLine;
        private readonly SpiralThrowChainViewConfig _config;

        private Vector3 _spiralUp;
        private Vector3 _spiralRight;
        private float _time;
        private float _effectMultiplier;
        
        private float Duration => _throwResult.Duration;
        private float DurationHitObstacle => _throwResult.DurationHitObstacle;
        
        private int _chainBoneCount;
        private int _chainBoneCountMinusOne;

        private float LoopSpread => _config.LoopSpread;
        private float MaxAmplitude => _config.MaxAmplitude;
        private float SpinSpeed =>_config.SpinSpeed;
        private float PhaseOffset =>_config.PhaseOffset;
        private AnimationCurve AmplitudeBoneWeightCurve =>_config.AmplitudeBoneWeightCurve;
        private AnimationCurve SpeedOverTimeCurve =>_config.SpeedOverTimeCurve;
        private AnimationCurve AmplitudeOverTimeCurve =>_config.AmplitudeOverTimeCurve;
        private AnimationCurve ObstacleHitMultiplierCurve =>_config.ObstacleHitMultiplierCurve;


        public SpiralThrowChainView(LineRenderer chainLine, SpiralThrowChainViewConfig config, int chainBoneCount)
        {
            _chainLine = chainLine;
            _config = config;
            _chainBoneCount = chainBoneCount;
        }
        
        public void EnterSetup(AnchorThrowResult throwResult)
        {
            _throwResult = throwResult;
            _chainLine.enabled = true;

            _spiralUp = Vector3.up;
            _spiralRight = Vector3.Cross(throwResult.Direction, _spiralUp).normalized;
        }


        public void OnViewEnter()
        {
            _chainLine.positionCount = _chainBoneCount;
            _chainBoneCountMinusOne = _chainBoneCount - 1;
            _time = 0;
        }

        public void LateUpdate(float deltaTime, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {

            _chainLine.SetPosition(0, playerBindPosition);
            _chainLine.SetPosition(_chainBoneCount-1, anchorBindPosition);

            Vector3 playerToAnchor = anchorBindPosition - playerBindPosition;
            float playerToAnchorDistance = playerToAnchor.magnitude;
            Vector3 playerToAnchorDirection = playerToAnchor / playerToAnchorDistance;

            float distanceStep = playerToAnchorDistance / _chainBoneCountMinusOne;
            
            _effectMultiplier = _throwResult.HitsObstacle ? 
                ObstacleHitMultiplierCurve.Evaluate(Mathf.Min(_time/DurationHitObstacle, 1)) :
                1;
            
            for (int i = 1; i < _chainBoneCount - 1; ++i)
            {
                Vector3 chainBonePosition = playerBindPosition + (playerToAnchorDirection * (i * distanceStep));

                float t = i / (float)_chainBoneCountMinusOne;
                float time = _time + (t * PhaseOffset);

                float spread = t * LoopSpread - CurrentSpeedOverTime(time);
                float size = ChainBoneAmplitudeWeight(t) * CurrentAmplitudeOverTime(time) * _effectMultiplier;
                
                Vector3 spiralOffset = _spiralUp * (Mathf.Sin(spread) * size) +
                                       _spiralRight * (Mathf.Cos(spread) * size);

                chainBonePosition += spiralOffset;
                
                _chainLine.SetPosition(i, chainBonePosition);
            }

            _time += deltaTime;
        }

        public void OnViewExit()
        {
            
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