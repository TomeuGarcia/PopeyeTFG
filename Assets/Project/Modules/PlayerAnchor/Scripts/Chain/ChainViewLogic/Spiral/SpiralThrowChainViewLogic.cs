using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.PlayerAnchor.Player;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class SpiralThrowChainViewLogic : IChainViewLogic
    {
        private readonly SpiralThrowChainViewLogicConfig _logicConfig;
        
        private readonly int _chainBoneCount;
        private readonly int _chainBoneCountMinusOne;

        private readonly Vector3[] _chainPositions;
        private Vector3[] _previousStateChainPositions;
        private float _previousStateTransitionT;

        private float _duration;
        private float _time;

        

        private float StateTransitionDuration => _logicConfig.StateTransitionDuration;
        private Ease StateTransitionEase => _logicConfig.StateTransitionEase;
        private float LoopSpread => _logicConfig.LoopSpread;
        private float MaxAmplitude => _logicConfig.MaxAmplitude;
        private float SpinSpeed =>_logicConfig.SpinSpeed;
        private float PhaseOffset =>_logicConfig.PhaseOffset;
        private AnimationCurve AmplitudeBoneWeightCurve =>_logicConfig.AmplitudeBoneWeightCurve;
        private AnimationCurve SpeedOverTimeCurve =>_logicConfig.SpeedOverTimeCurve;
        private AnimationCurve AmplitudeOverTimeCurve =>_logicConfig.AmplitudeOverTimeCurve;
        private float DurationMultiplier =>_logicConfig.DurationMultiplier;


        public SpiralThrowChainViewLogic(SpiralThrowChainViewLogicConfig logicConfig, int chainBoneCount)
        {
            _logicConfig = logicConfig;
            _chainBoneCount = chainBoneCount;
            _chainBoneCountMinusOne = _chainBoneCount - 1;

            _chainPositions = new Vector3[_chainBoneCount];
        }
        
        public void EnterSetup(float duration)
        {
            _duration = duration * DurationMultiplier;
        }


        public void OnViewEnter(Vector3[] previousStateChainPositions, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {
            _time = 0;

            _previousStateChainPositions = previousStateChainPositions;
            
            _previousStateTransitionT = 0;
            DOTween.To(
                () => _previousStateTransitionT,
                (value) => _previousStateTransitionT = value,
                1.0f,
                StateTransitionDuration
            ).SetEase(StateTransitionEase);
        }

        public void UpdateChainPositions(float deltaTime, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {

            _chainPositions[0] = anchorBindPosition;
            _chainPositions[^1] = playerBindPosition;

            Vector3 anchorToPlayer = playerBindPosition - anchorBindPosition;
            float anchorToPlayerDistance = anchorToPlayer.magnitude;
            Vector3 anchorToPlayerDirection = anchorToPlayer / anchorToPlayerDistance;

            float distanceStep = anchorToPlayerDistance / _chainBoneCountMinusOne;
            
            
            Vector3 spiralUp = Vector3.up;
            Vector3 spiralRight = Vector3.Cross(anchorToPlayer, spiralUp).normalized;
            spiralUp = Vector3.Cross(spiralRight, anchorToPlayer).normalized;
            
            for (int i = 1; i < _chainBoneCount - 1; ++i)
            {
                Vector3 chainBonePosition = anchorBindPosition + (anchorToPlayerDirection * (i * distanceStep));

                float t = i / (float)_chainBoneCountMinusOne;
                float time = _time + (t * PhaseOffset);

                float spread = t * LoopSpread - CurrentSpeedOverTime(time);
                float size = ChainBoneAmplitudeWeight(t) * CurrentAmplitudeOverTime(time);
                
                Vector3 spiralOffset = spiralUp * (Mathf.Sin(spread) * size) +
                                       spiralRight * (Mathf.Cos(spread) * size);

                chainBonePosition += spiralOffset;
                
                _chainPositions[i] = chainBonePosition;
            }

            _time += deltaTime;


            if (_previousStateTransitionT < 1)
            {
                for (int i = 0; i < _chainBoneCount; ++i)
                {
                    _chainPositions[i] = Vector3.Lerp(_previousStateChainPositions[i], _chainPositions[i],
                        _previousStateTransitionT);
                }
            }
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
            return SpeedOverTimeCurve.Evaluate(Mathf.Min(1f, time / _duration)) * SpinSpeed * time;
        }

        private float CurrentAmplitudeOverTime(float time)
        {
            return AmplitudeOverTimeCurve.Evaluate(Mathf.Min(1f, time / _duration)) * MaxAmplitude;
        }
        
    }
}