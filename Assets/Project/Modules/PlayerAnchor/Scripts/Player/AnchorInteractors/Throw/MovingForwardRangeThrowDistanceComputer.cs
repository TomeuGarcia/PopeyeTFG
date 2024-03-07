using Popeye.Modules.PlayerAnchor.Anchor;
using Popeye.Modules.PlayerController;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class MovingForwardRangeThrowDistanceComputer : IThrowDistanceComputer
    {
        private readonly AnchorThrowConfig _throwConfig;
        private readonly IPlayerMovementStateReader _playerMovementStateReader;
        private readonly RangeThrowDistanceComputer _rangeThrowDistanceComputer;

        private float MaxExtraDistance => _throwConfig.MovingForwardExtraDistanceData.Distance;
        private AnimationCurve AdaptEaseCurve => _throwConfig.MovingForwardExtraDistanceData.AdaptEaseCurve;
        
        
        public MovingForwardRangeThrowDistanceComputer(AnchorThrowConfig throwConfig, IPlayerMovementStateReader playerMovementStateReader)
        {
            _throwConfig = throwConfig;
            _playerMovementStateReader = playerMovementStateReader;
            _rangeThrowDistanceComputer = new RangeThrowDistanceComputer(_throwConfig);
        }
        
        public float ComputeThrowDistance(float throwForce01)
        {
            float rangeDistance = _rangeThrowDistanceComputer.ComputeThrowDistance(throwForce01);

            float extraDistanceMultiplier = Vector3.Dot(_playerMovementStateReader.MovementDirection,
                _playerMovementStateReader.LookDirection);
            
            extraDistanceMultiplier = Mathf.Max(0.0f, extraDistanceMultiplier);
            extraDistanceMultiplier = AdaptEaseCurve.Evaluate(extraDistanceMultiplier);

            float extraDistance = MaxExtraDistance * extraDistanceMultiplier;
            
            Debug.Log(extraDistanceMultiplier);
            
            return rangeDistance + extraDistance;
        }
    }
}