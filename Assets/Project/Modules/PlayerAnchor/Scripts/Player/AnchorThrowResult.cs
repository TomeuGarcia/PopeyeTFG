using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class AnchorThrowResult
    {
        public Vector3[] TrajectoryPathPoints { get; private set; }
        public Quaternion StartLookRotation { get; private set; }
        public Quaternion EndLookRotation { get; private set; }
        public float Duration { get; private set; }
        public bool EndsOnVoid { get; private set; }

        
            
        public void Reset(Vector3[] throwPathPoints, Quaternion startLookRotation, Quaternion endLookRotation,
            float duration, bool endsOnVoid)
        {
            TrajectoryPathPoints = throwPathPoints;
            StartLookRotation = startLookRotation;
            EndLookRotation = endLookRotation;
            Duration = duration;
            EndsOnVoid = endsOnVoid;
        }
    }
}