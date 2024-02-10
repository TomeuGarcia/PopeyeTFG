using DG.Tweening;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class FoldingChainViewLogic : IChainViewLogic
    {
        private readonly FoldingChainViewLogicConfig _config;
        private readonly int _chainBoneCount;
        
        private readonly Vector3[] _chainPositions;

        private Vector3[] _previousStateChainPositions;
        
        private float _duration;
        private float _time;

        private float _globalT;
        
        private float DurationMultiplier => _config.DurationMultiplier;


        public FoldingChainViewLogic(FoldingChainViewLogicConfig config, int chainBoneCount)
        {
            _config = config;
            _chainBoneCount = chainBoneCount;

            _chainPositions = new Vector3[_chainBoneCount];
        }


        public void EnterSetup(float dashDuration, Ease dashEase)
        {
            _duration = dashDuration;

            _globalT = 0;
            DOTween.To(
                () => _globalT,
                (value) => _globalT = value,
                1.0f,
                _duration * (1-DurationMultiplier)
            ).SetEase(dashEase);
        }
        
        public void OnViewEnter(Vector3[] previousStateChainPositions, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {
            _previousStateChainPositions = previousStateChainPositions;
            _time = 0;
        }

        public void UpdateChainPositions(float deltaTime, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {
            if (_time > _duration) return;
            
            for (int i = 0; i < _chainBoneCount; ++i)
            {
                _chainPositions[i] = Vector3.Lerp(_previousStateChainPositions[i], anchorBindPosition, _globalT);
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