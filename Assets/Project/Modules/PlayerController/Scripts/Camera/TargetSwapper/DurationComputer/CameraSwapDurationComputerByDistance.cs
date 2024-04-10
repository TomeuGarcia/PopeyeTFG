using UnityEngine;

namespace Popeye.Modules.Camera.TargetSwapper
{
    public class CameraSwapDurationComputerByDistance : ICameraSwapDurationComputer
    {
        private readonly float _minDuration;
        private readonly float _maxDuration;
        private readonly float _speed;

        public CameraSwapDurationComputerByDistance(
            float minDuration,
            float maxDuration,
            float speed
        )
        {
            _minDuration = minDuration;
            _maxDuration = maxDuration;
            _speed = speed;
        }
        
        public float ComputeDuration(Transform originalFollowTarget, Transform newTarget)
        {
            Vector3 originPosition = originalFollowTarget.position;
            Vector3 newPosition = newTarget.position;
            float originToNewDistance = (newPosition - originPosition).magnitude;

            float duration = originToNewDistance * _speed;
            duration = Mathf.Clamp(duration, _minDuration, _maxDuration);

            return duration;
        }
    }
}