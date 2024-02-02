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
        
        
        private float Duration => _throwResult.Duration;
        
        private int ChainBoneCount => _config.ChainBoneCount;
        private int _chainBoneCountMinusOne;

        private float LoopSpread => _config.LoopSpread;
        private float MaxAmplitude => _config.MaxAmplitude;
        private float SpinSpeed =>_config.SpinSpeed;
        private float PhaseOffset =>_config.PhaseOffset;


        public SpiralThrowChainView(LineRenderer chainLine, SpiralThrowChainViewConfig config)
        {
            _chainLine = chainLine;
            _config = config;
        }
        
        public void Setup(AnchorThrowResult throwResult)
        {
            _throwResult = throwResult;
            _chainLine.enabled = true;
            _chainLine.positionCount = ChainBoneCount;

            _chainBoneCountMinusOne = ChainBoneCount - 1;

            _spiralUp = Vector3.up;
            _spiralRight = Vector3.Cross(throwResult.Direction, _spiralUp).normalized;

            _time = 0;
        }


        private float m;
        
        public void LateUpdate(float deltaTime, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {

            _chainLine.SetPosition(0, playerBindPosition);
            _chainLine.SetPosition(ChainBoneCount-1, anchorBindPosition);

            Vector3 playerToAnchor = anchorBindPosition - playerBindPosition;
            float playerToAnchorDistance = playerToAnchor.magnitude;
            Vector3 playerToAnchorDirection = playerToAnchor / playerToAnchorDistance;

            float distanceStep = playerToAnchorDistance / _chainBoneCountMinusOne;
            
            m = _config.ObstacleHitMultiplierCurve.Evaluate(Mathf.Min(_time/_throwResult.DurationHitObstacle, 1));
            
            for (int i = 1; i < ChainBoneCount - 1; ++i)
            {
                Vector3 chainBonePosition = playerBindPosition + (playerToAnchorDirection * (i * distanceStep));

                float t = i / (float)_chainBoneCountMinusOne;
                float time = _time + (t * PhaseOffset);

                float spread = t * LoopSpread - CurrentSpeedOverTime(time) *m;
                float size = ChainBoneAmplitudeWeight(t) * CurrentAmplitudeOverTime(time) *m;
                
                Vector3 spiralOffset = _spiralUp * (Mathf.Sin(spread) * size) +
                                       _spiralRight * (Mathf.Cos(spread) * size);

                chainBonePosition += spiralOffset;
                
                _chainLine.SetPosition(i, chainBonePosition);
            }

            _time += deltaTime;
        }

        public void OnViewSwapped()
        {
            
        }

        private float ChainBoneAmplitudeWeight(float chainT)
        {
            return _config.AmplitudeBoneWeightCurve.Evaluate(chainT);
        }

        private float CurrentSpeedOverTime(float time)
        {
            return _config.SpeedOverTimeCurve.Evaluate(Mathf.Min(1f, time / Duration)) * SpinSpeed * time;
        }

        private float CurrentAmplitudeOverTime(float time)
        {
            return _config.AmplitudeOverTimeCurve.Evaluate(Mathf.Min(1f, time / Duration)) * MaxAmplitude;
        }
        
    }
}