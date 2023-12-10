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
    }
}