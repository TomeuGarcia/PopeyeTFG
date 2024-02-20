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

        private int LinePoints => _points.Length;

        public AnchorTrajectoryView(LineRenderer firstLine, LineRenderer secondLine, int linePoints)
        {
            _firstLine = firstLine;
            _secondLine = secondLine;
            _curve = new QuadraticBezierCurve();
            _points = new Vector3[linePoints];
        }

        public void Hide()
        {
            _firstLine.positionCount = 0;
            _secondLine.positionCount = 0;
        }

        public void DrawTrajectory(Vector3[] trajectoryPoints, RaycastHit obstacleHit, 
            bool trajectoryHitsObstacle, int lastIndexBeforeCollision)
        {
            if (trajectoryHitsObstacle)
            {
                DrawObstacleHitTrajectory(trajectoryPoints, obstacleHit, lastIndexBeforeCollision);
            }
            else
            {
                DrawSingleLineTrajectory(trajectoryPoints);    
            }
        }

        private void DrawSingleLineTrajectory(Vector3[] trajectoryPoints)
        {
            FillLine(_firstLine, trajectoryPoints, 0, trajectoryPoints.Length - 1);
            
            _secondLine.positionCount = 0;
        }

        private void DrawObstacleHitTrajectory(Vector3[] trajectoryPoints, RaycastHit obstacleHit, int lastIndexBeforeCollision)
        {
            if (lastIndexBeforeCollision == trajectoryPoints.Length - 1)
            {
                DrawSingleLineTrajectory(trajectoryPoints);
                return;
            }
            
            FillLine(_firstLine, trajectoryPoints, 0, lastIndexBeforeCollision);
            FillLine(_secondLine, trajectoryPoints, lastIndexBeforeCollision + 1, trajectoryPoints.Length-1);
        }
        
        
        private void FillLine(LineRenderer line, Vector3[] trajectoryPoints, 
            int trajectoryStartIndex, int trajectoryLastIndex)
        {
            Vector3 startPosition = trajectoryPoints[trajectoryStartIndex];
            Vector3 endPosition = trajectoryPoints[trajectoryLastIndex];
            Vector3 startToEndDirection = (startPosition - endPosition).normalized;
            Vector3 lastPointControl = endPosition + startToEndDirection;
            lastPointControl.y = startPosition.y;
            Vector3 firstPointControl = Vector3.Lerp(startPosition, lastPointControl, 0.5f);

            _curve.P0 = startPosition;
            _curve.P1 = firstPointControl;
            _curve.P2 = lastPointControl;
            _curve.P3 = endPosition;
            
            int numberOfPositions = (trajectoryLastIndex - trajectoryStartIndex) + 1;
            Vector3[] points = new Vector3[numberOfPositions];
            
            _curve.FillPointsFromCurve(points, out float dist);
            
            line.positionCount = numberOfPositions;
            line.SetPositions(points);
        }
        
    }
}