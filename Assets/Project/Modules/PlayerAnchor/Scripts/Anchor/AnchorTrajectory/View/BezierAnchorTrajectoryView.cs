using Project.Scripts.Math.Curves;
using UnityEngine;
using UnityEngine.Rendering;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class BezierAnchorTrajectoryView : IAnchorTrajectoryView
    {
        private readonly AnchorTrajectoryViewConfig _config;
        private readonly LineRenderer _firstLine;
        private readonly LineRenderer _secondLine;
        private readonly QuadraticBezierCurve _curve;
        private readonly Vector3[] _points;

        private int FullNumberOfLinePoints => _points.Length;

        public BezierAnchorTrajectoryView(LineRenderer firstLine, LineRenderer secondLine, 
            AnchorTrajectoryViewConfig config, int linePoints)
        {
            _config = config;
            
            _firstLine = firstLine;
            InitLine(_firstLine);
            
            _secondLine = secondLine;
            InitLine(_secondLine);
            
            _curve = new QuadraticBezierCurve();
            _points = new Vector3[linePoints];
        }

        private void InitLine(LineRenderer line)
        {
            line.widthCurve = _config.WidthCurve; 
            line.widthMultiplier = _config.WidthMultiplier; 
            line.material = _config.TrajectoryMaterial;
            line.alignment = _config.Alignment;
            line.textureMode = _config.TextureMode;
            line.shadowCastingMode = ShadowCastingMode.Off;
        }

        public void Hide()
        {
            _firstLine.positionCount = 0;
            _secondLine.positionCount = 0;
        }

        public void DrawTrajectory(Vector3[] trajectoryPoints, bool trajectoryHitsObstacle, int lastIndexBeforeCollision)
        {
            if (trajectoryHitsObstacle && lastIndexBeforeCollision > -1)
            {
                DrawObstacleHitTrajectory(trajectoryPoints, lastIndexBeforeCollision);
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

        private void DrawObstacleHitTrajectory(Vector3[] trajectoryPoints,int lastIndexBeforeCollision)
        {
            if (lastIndexBeforeCollision == trajectoryPoints.Length)
            {
                DrawSingleLineTrajectory(trajectoryPoints);
                return;
            }
            
            FillLine(_firstLine, trajectoryPoints, 0, lastIndexBeforeCollision);
            FillLine(_secondLine, trajectoryPoints, lastIndexBeforeCollision, trajectoryPoints.Length-1);
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
            Vector3[] points = CreatePositionsArray(numberOfPositions);
            
            _curve.FillPointsFromCurve(points, out float dist);
            
            line.positionCount = numberOfPositions;
            line.SetPositions(points);
        }

        private Vector3[] CreatePositionsArray(int numberOfPositions)
        {
            if (numberOfPositions == FullNumberOfLinePoints)
            {
                return _points;
            }

            return new Vector3[numberOfPositions];
        }
        
    }
}