using Popeye.Timers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class SemicircleChainViewLogic : IChainViewLogic
    {
        private readonly int _chainBoneCount;
        private readonly Vector3 _centerAxis;
        private readonly float _startCircleRadius;
        private readonly float _endCircleRadius;
        private readonly int _chainBoneCountMinusOne;

        private readonly Vector3[] _chainPositions;
        private readonly Timer _viewTimer;
        
        private float CircleRadius => Mathf.Lerp(_startCircleRadius, _endCircleRadius, _viewTimer.GetCounterRatio01());

        public float PositionsExtraDistance { get; }

        public SemicircleChainViewLogic(int chainBoneCount, Vector3 centerAxis, 
            float startCircleRadius, float endCircleRadius, float duration, float positionsExtraDistance)
        {
            PositionsExtraDistance = positionsExtraDistance;
            _chainBoneCount = chainBoneCount;
            _centerAxis = centerAxis;
            _startCircleRadius = startCircleRadius;
            _endCircleRadius = endCircleRadius;
            _chainBoneCountMinusOne = _chainBoneCount - 1;

            _chainPositions = new Vector3[_chainBoneCount];
            
            _viewTimer = new Timer(duration);
        }



        public void OnViewEnter(Vector3[] previousStateChainPositions, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {
            _viewTimer.Clear();
        }

        public void UpdateChainPositions(float deltaTime, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {
            Vector3 anchorToPlayer = playerBindPosition - anchorBindPosition;
            float anchorToPlayerDistance = anchorToPlayer.magnitude;
            Vector3 anchorToPlayerDirection = anchorToPlayer / anchorToPlayerDistance;
            float distanceStep = anchorToPlayerDistance / _chainBoneCountMinusOne;


            Vector3 semicircleDirection = Vector3.Cross(anchorToPlayerDirection, _centerAxis).normalized;
            
            
            
            _chainPositions[0] = anchorBindPosition;
            _chainPositions[^1] = playerBindPosition;

            for (int i = 1; i < _chainBoneCountMinusOne; ++i)
            {
                Vector3 semiCircleDisplacement = ComputeSemicircleDisplacement(i, semicircleDirection);
                
                _chainPositions[i] = anchorBindPosition + (anchorToPlayerDirection * (i * distanceStep))
                    + semiCircleDisplacement;
            }
            
            _viewTimer.Update(deltaTime);
        }

        private Vector3 ComputeSemicircleDisplacement(int positionIndex, Vector3 semicircleDirection)
        {
            float t = (float)positionIndex / _chainBoneCountMinusOne;
            t = Mathf.Sin(t * Mathf.PI);
            
            float displacement = t * CircleRadius;
            
            Vector3 semiCircleDisplacement = semicircleDirection * displacement;
            return semiCircleDisplacement;
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