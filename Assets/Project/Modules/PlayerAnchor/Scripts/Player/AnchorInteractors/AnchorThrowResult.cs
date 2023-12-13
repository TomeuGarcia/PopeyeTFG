using DG.Tweening;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class AnchorThrowResult
    {
        public Vector3[] TrajectoryPathPoints { get; private set; }
        public Vector3 FirstTrajectoryPathPoint => TrajectoryPathPoints[0];
        public Vector3 LastTrajectoryPathPoint => TrajectoryPathPoints[^1];
        public Vector3 Direction { get; private set; }
        public Quaternion StartLookRotation { get; private set; }
        public Quaternion EndLookRotation { get; private set; }
        public float Duration { get; private set; }
        public bool EndsOnVoid { get; private set; }

        
        public AnimationCurve InterpolationEaseCurve { get; private set; }


        public AnchorThrowResult(AnimationCurve interpolationEaseCurve)
        {
            InterpolationEaseCurve = interpolationEaseCurve;
        }
        
        public void Reset(Vector3[] throwPathPoints, Vector3 direction, Quaternion startLookRotation, Quaternion endLookRotation,
            float duration, bool endsOnVoid)
        {
            TrajectoryPathPoints = throwPathPoints;
            Direction = direction;
            StartLookRotation = startLookRotation;
            EndLookRotation = endLookRotation;
            Duration = duration;
            EndsOnVoid = endsOnVoid;
        }
        
        public void Reset(Vector3[] throwPathPoints, Vector3 direction, Vector3 floorNormal,
            float duration, bool endsOnVoid)
        {
            TrajectoryPathPoints = throwPathPoints;
            Direction = direction;
            
            Vector3 right = Vector3.Cross(direction, floorNormal).normalized;
            StartLookRotation = ComputePathLookRotationBetweenIndices(0, 1, right);
            EndLookRotation = ComputePathLookRotationBetweenIndices(TrajectoryPathPoints.Length-2, 
                TrajectoryPathPoints.Length-1, right);
            
            Duration = duration;
            EndsOnVoid = endsOnVoid;
        }
        
        private Quaternion ComputePathLookRotationBetweenIndices(int startIndex, int endIndex,
            Vector3 right)
        {
            if (Mathf.Max(startIndex, endIndex) >= TrajectoryPathPoints.Length ||
                Mathf.Min(startIndex, endIndex) < 0)
            {
                return Quaternion.identity;
            }
            
            Vector3 pathForward = (TrajectoryPathPoints[endIndex] - TrajectoryPathPoints[startIndex]).normalized;
            Vector3 up = Vector3.Cross(pathForward, right).normalized;

            return Quaternion.LookRotation(pathForward, up);
        }
        
    }
}