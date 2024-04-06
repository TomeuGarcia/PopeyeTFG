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
        private float AdaptInDuration => _throwConfig.MovingForwardExtraDistanceData.AdaptInDuration;
        private float AdaptOutDuration => _throwConfig.MovingForwardExtraDistanceData.AdaptOutDuration;
        private AnimationCurve AdaptEaseInCurve => _throwConfig.MovingForwardExtraDistanceData.AdaptEaseInCurve;
        private AnimationCurve AdaptEaseOutCurve => _throwConfig.MovingForwardExtraDistanceData.AdaptEaseOutCurve;
        private float DotToConsider => _throwConfig.MovingForwardExtraDistanceData.DotToConsider;


        private float _currentTimer;
        
        
        public MovingForwardRangeThrowDistanceComputer(AnchorThrowConfig throwConfig, IPlayerMovementStateReader playerMovementStateReader)
        {
            _throwConfig = throwConfig;
            _playerMovementStateReader = playerMovementStateReader;
            _rangeThrowDistanceComputer = new RangeThrowDistanceComputer(_throwConfig);

            _currentTimer = 0;
        }
        
        public float ComputeThrowDistance(float throwForce01)
        {
            float rangeDistance = _rangeThrowDistanceComputer.ComputeThrowDistance(throwForce01);

            bool movingInLookDirection = ComputeMovingInLookDirection();
            float timerRatio01 = ComputeExtraDistanceTimerRatio(movingInLookDirection);
            float extraDistanceMultiplier = ComputeExtraDistanceMultiplier(movingInLookDirection, timerRatio01);
            
            float extraDistance = MaxExtraDistance * extraDistanceMultiplier;
            
            return rangeDistance + extraDistance;
        }

        public void ClearState()
        {
            _rangeThrowDistanceComputer.ClearState();
            _currentTimer = 0;
        }



        private bool ComputeMovingInLookDirection()
        {
            float movementLookDot = Vector3.Dot(
                _playerMovementStateReader.MovementDirectionNormalized, 
                _playerMovementStateReader.LookDirection
                );

            return movementLookDot > DotToConsider;
        }
        
        private float ComputeExtraDistanceTimerRatio(bool movingInLookDirection)
        {
            int timerSign = movingInLookDirection ? 1 : -1;
            float maxDuration = movingInLookDirection ? AdaptInDuration : AdaptOutDuration;
            
            _currentTimer += timerSign * Time.deltaTime;
            _currentTimer = Mathf.Clamp(_currentTimer, 0f, maxDuration);
            
            return _currentTimer / maxDuration;
        }

        private float ComputeExtraDistanceMultiplier(bool movingInLookDirection, float timerRatio01)
        {
            AnimationCurve adaptEaseCurve = movingInLookDirection
                ? AdaptEaseInCurve
                : AdaptEaseOutCurve;
                
            return adaptEaseCurve.Evaluate(timerRatio01);
        }
        
    }
}