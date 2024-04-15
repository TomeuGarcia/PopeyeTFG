using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorTrajectoryView
    {
        void Show();
        void Hide();
        void DrawTrajectory(Vector3[] trajectoryPoints, bool trajectoryHitsObstacle, int lastIndexBeforeCollision, bool endsOnVoid);
    }
}