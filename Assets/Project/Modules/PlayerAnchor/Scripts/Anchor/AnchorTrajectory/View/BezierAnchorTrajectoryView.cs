using Project.Scripts.Math.Curves;
using UnityEngine;
using UnityEngine.Rendering;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class BezierAnchorTrajectoryView : IAnchorTrajectoryView
    {
        private readonly AnchorTrajectoryViewConfig _config;
        private readonly QuadraticBezierCurve _curve;
        private readonly LineViewData _firstLineViewData;
        private readonly LineViewData _secondLineViewData;
        
        private class LineViewData
        {
            private readonly LineRenderer _lineRenderer;
            private readonly Vector3[] _points;

            public LineViewData(LineRenderer lineRenderer, int numberOfPoints)
            {
                _lineRenderer = lineRenderer;
                _lineRenderer.positionCount = numberOfPoints;
                _points = new Vector3[numberOfPoints];
            }

            public void UpdateWithCurve(QuadraticBezierCurve curve)
            { 
                curve.FillPointsFromCurve(_points, out float dist);
                _lineRenderer.positionCount = _points.Length;
                _lineRenderer.SetPositions(_points);
            }

            public void Hide()
            {
                _lineRenderer.positionCount = 0;
            }
        }

        public BezierAnchorTrajectoryView(LineRenderer firstLine, LineRenderer secondLine, 
            AnchorTrajectoryViewConfig config, int linePoints)
        {
            _config = config;
            
            InitLineRenderer(firstLine);
            _firstLineViewData = new LineViewData(firstLine, linePoints);
            
            InitLineRenderer(secondLine);
            _secondLineViewData = new LineViewData(secondLine, linePoints);
            
            _curve = new QuadraticBezierCurve();
        }

        private void InitLineRenderer(LineRenderer line)
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
            _firstLineViewData.Hide();
            _secondLineViewData.Hide();
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
            FillLine(_firstLineViewData, trajectoryPoints, 0, trajectoryPoints.Length - 1);
            
            _secondLineViewData.Hide();
        }

        private void DrawObstacleHitTrajectory(Vector3[] trajectoryPoints,int lastIndexBeforeCollision)
        {
            if (lastIndexBeforeCollision == trajectoryPoints.Length)
            {
                DrawSingleLineTrajectory(trajectoryPoints);
                return;
            }
            
            FillLine(_firstLineViewData, trajectoryPoints, 0, lastIndexBeforeCollision);
            FillLine(_secondLineViewData, trajectoryPoints, lastIndexBeforeCollision, trajectoryPoints.Length-1);
        }
        
        
        private void FillLine(LineViewData lineViewData, Vector3[] trajectoryPoints, 
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
            
            lineViewData.UpdateWithCurve(_curve);
        }

    }
}