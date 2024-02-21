using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorTrajectoryView
    {
        void Hide();
        void DrawTrajectory(Vector3[] trajectoryPoints, RaycastHit obstacleHit,
            bool trajectoryHitsObstacle, int lastIndexBeforeCollision);
    }
}