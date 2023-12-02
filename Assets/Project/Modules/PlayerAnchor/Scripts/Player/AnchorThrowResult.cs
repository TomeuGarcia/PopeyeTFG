using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class AnchorThrowResult
    {
        public Vector3[] TrajectoryPathPoints { get; private set; }
        public float Duration { get; private set; }
        public bool EndsOnVoid { get; private set; }
            
        public void Reset(Vector3[] throwPathPoints, float duration, bool endsOnVoid)
        {
            TrajectoryPathPoints = throwPathPoints;
            Duration = duration;
            EndsOnVoid = endsOnVoid;
        }
    }
}