using DG.Tweening;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class FoldingChainViewLogic : IChainViewLogic
    {
        private readonly FoldingChainViewLogicConfig _config;
        private readonly int _chainBoneCount;
        private readonly int _chainBoneCountMinusOne;
        
        private readonly Vector3[] _chainPositions;

        private Vector3[] _previousStateChainPositions;
        
        private float _duration;
        private float _time;

        private float _globalT;
        
        private float PhaseOffset => _config.PhaseOffset;
        private AnimationCurve PhaseWeightCurve => _config.PhaseWeightCurve;


        public FoldingChainViewLogic(FoldingChainViewLogicConfig config, int chainBoneCount)
        {
            _config = config;
            _chainBoneCount = chainBoneCount;
            _chainBoneCountMinusOne = _chainBoneCount - 1;

            _chainPositions = new Vector3[_chainBoneCount];
        }


        public void EnterSetup(Vector3[] previousStateChainPositions, float dashDuration, Ease dashEase)
        {
            _previousStateChainPositions = previousStateChainPositions;
            _duration = dashDuration;

            _globalT = 0;
            DOTween.To(
                () => _globalT,
                (value) => _globalT = value,
                1.0f,
                _duration
            ).SetEase(dashEase);
        }
        
        public void OnViewEnter()
        {
            _time = 0;
        }

        public void UpdateChainPositions(float deltaTime, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {
            if (_time > _duration) return;

            float t = PhaseOffset / _chainBoneCountMinusOne;
            
            
            for (int i = 0; i < _chainBoneCount; ++i)
            {
                float localT = t * i;
                if (localT > _globalT)
                {
                    _chainPositions[i] = _previousStateChainPositions[i];
                    continue;
                }

                float lerp = (_globalT - localT) / (1f - localT);

                _chainPositions[i] = Vector3.Lerp(_previousStateChainPositions[i], anchorBindPosition, lerp);


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
    }
}