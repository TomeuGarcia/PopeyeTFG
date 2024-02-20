using Project.Scripts.Math.Curves;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class AnchorTrajectoryView
    {
        private readonly LineRenderer _firstLine;
        private readonly LineRenderer _secondLine;
        private readonly QuadraticBezierCurve _curve;
        private readonly Vector3[] _points;

        public AnchorTrajectoryView(LineRenderer firstLine, LineRenderer secondLine, int linePoints)
        {
            _firstLine = firstLine;
            _secondLine = secondLine;
            _curve = new QuadraticBezierCurve();
            _points = new Vector3[linePoints];

            _firstLine.positionCount = linePoints;
            _secondLine.positionCount = linePoints;
        }

        public void DrawTrajectory(Vector3[] trajectoryPoints, bool trajectoryHitsObstacle, RaycastHit obstacleHit)
        {
            Vector3 startPosition = trajectoryPoints[0];
            Vector3 endPosition = trajectoryPoints[^1];
            Vector3 startToEndDirection = (startPosition - endPosition).normalized;
            Vector3 lastPointControl = endPosition + startToEndDirection;
            lastPointControl.y = startPosition.y;
            Vector3 firstPointControl = Vector3.Lerp(startPosition, lastPointControl, 0.5f);

            _curve.P0 = startPosition;
            _curve.P1 = firstPointControl;
            _curve.P2 = lastPointControl;
            _curve.P3 = endPosition;
            
            _curve.FillPointsFromCurve(_points, out float dist);
            _firstLine.SetPositions(_points);
        }
    }
}