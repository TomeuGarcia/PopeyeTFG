using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class AnchorThrowTrajectory
    {
        private AnchorTrajectoryEndSpot _trajectoryEndSpot;
        private AnchorThrowConfig _anchorThrowConfig;
        private AnchorPullConfig _anchorPullConfig;


        private Vector3[] _straightTrajectoryPath;
        private Vector3[] _curvedEndTrajectoryPath;
        
        private Vector3[] _correctedTrajectoryPath;
        private float _correctedPathDistance;
        
        private int _trajectoryHitIndex;

        public bool TrajectoryEndsOnVoid { get; private set; }


        private LineRenderer _debugLine;
        private LineRenderer _debugLine2;
        private LineRenderer _debugLine3;
        public void Configure(AnchorTrajectoryEndSpot trajectoryEndSpot, 
            AnchorThrowConfig anchorThrowConfig, AnchorPullConfig anchorPullConfig,
            LineRenderer debugLine, LineRenderer debugLine2, LineRenderer debugLine3)
        {
            _trajectoryEndSpot = trajectoryEndSpot;
            _anchorThrowConfig = anchorThrowConfig;
            _anchorPullConfig = anchorPullConfig;

            _trajectoryEndSpot.Hide();
            
            _straightTrajectoryPath = new Vector3[2];
            _curvedEndTrajectoryPath = new Vector3[20];
            _debugLine = debugLine;
            _debugLine2 = debugLine2;
            _debugLine3 = debugLine3;
        }
        



        public void UpdateTrajectoryEndSpot()
        {
            Vector3 spotPosition;
            Vector3 spotLookDirection;
            
            if (GetFirstHitInTrajectoryPath(_curvedEndTrajectoryPath, out RaycastHit hit, out _trajectoryHitIndex))
            {
                TrajectoryEndsOnVoid = false;
                spotPosition = hit.point;
                spotLookDirection = hit.normal;
            }
            else
            {
                TrajectoryEndsOnVoid = true;
                _trajectoryHitIndex = _curvedEndTrajectoryPath.Length - 1;
                spotPosition = _curvedEndTrajectoryPath[_curvedEndTrajectoryPath.Length - 1];
                spotLookDirection = Vector3.up;
            }
            spotPosition += spotLookDirection * 0.05f;
            _trajectoryEndSpot.MatchSpot(spotPosition, spotLookDirection);

            
            _debugLine.positionCount = _straightTrajectoryPath.Length;
            _debugLine.SetPositions(_straightTrajectoryPath);
            
            _debugLine2.positionCount = _curvedEndTrajectoryPath.Length;
            _debugLine2.SetPositions(_curvedEndTrajectoryPath);
        }
        public void ShowTrajectoryEndSpot()
        {
            _trajectoryEndSpot.Show();
        }
        public void HideTrajectoryEndSpot()
        {
            _trajectoryEndSpot.Hide();
        }


        public void UpdateTrajectoryPath(Vector3 startPoint, Vector3 direction, float distance)
        {
            DoUpdateTrajectoryPath(_straightTrajectoryPath, startPoint, direction, distance);
            DoUpdateTrajectoryPath(_curvedEndTrajectoryPath, startPoint, direction, distance);
            CurveTrajectoryPath(_curvedEndTrajectoryPath, _anchorThrowConfig.TrajectoryBendSharpness);
        }
        
        private void DoUpdateTrajectoryPath(Vector3[] trajectoryPath, Vector3 startPoint, Vector3 direction, float distance)
        {
            trajectoryPath[0] = startPoint;

            int steps = trajectoryPath.Length - 1;
            float distanceStep = distance / steps;
            
            for (int i = 1; i < trajectoryPath.Length; ++i)
            {
                trajectoryPath[i] = startPoint + (direction * (distanceStep * i));
            }
        }
        
        private void CurveTrajectoryPath(Vector3[] trajectoryPath, int bendSharpness)
        {
            for (int i = bendSharpness; i < trajectoryPath.Length; ++i)
            {
                float t = (float)i / (trajectoryPath.Length - 1.0f);
                t = Mathf.Pow(t, bendSharpness);

                Vector3 curvedPathPoint;                
                
                if (Physics.Raycast(trajectoryPath[i], Vector3.down, out RaycastHit hit, 
                        _anchorThrowConfig.HeightToConsiderFloor, 
                        PositioningHelper.Instance.ObstaclesLayerMask, QueryTriggerInteraction.Ignore))
                {
                    curvedPathPoint = Vector3.Lerp(trajectoryPath[i], hit.point, t);
                }
                else
                {
                    curvedPathPoint = trajectoryPath[i] + Vector3.down * (_anchorThrowConfig.HeightToConsiderFloor * t);
                }

                trajectoryPath[i] = curvedPathPoint;
            }
        }


        private bool GetFirstHitInTrajectoryPath(Vector3[] trajectoryPath, out RaycastHit trajectoryHit, 
            out int trajectoryIndex)
        {
            for (int i = 0; i < trajectoryPath.Length - 1; ++i)
            {
                Vector3 pointA = trajectoryPath[i];
                Vector3 pointB = trajectoryPath[i+1];
                Vector3 AtoB = pointB - pointA;

                if (Physics.Raycast(pointA, AtoB.normalized, out trajectoryHit, AtoB.magnitude + 0.2f, 
                        PositioningHelper.Instance.ObstaclesLayerMask, QueryTriggerInteraction.Ignore))
                {
                    trajectoryIndex = i;
                    return true;
                }
            }
            
            if (Physics.Raycast(trajectoryPath[trajectoryPath.Length - 1], Vector3.down, out trajectoryHit, 
                    1000, PositioningHelper.Instance.ObstaclesLayerMask,QueryTriggerInteraction.Ignore))
            {
                trajectoryIndex = trajectoryPath.Length - 1;
                return true;
            }

            trajectoryIndex = -1;
            trajectoryHit = new RaycastHit();
            return false;
        }


        public Vector3[] GetCorrectedTrajectoryPath()
        {
            int pathCount = _trajectoryHitIndex + 1;
            _correctedTrajectoryPath = new Vector3[pathCount];
            _correctedPathDistance = 0.0f;

            if (_curvedEndTrajectoryPath.Length > 0)
            {
                _correctedTrajectoryPath[0] = _curvedEndTrajectoryPath[0];
            }
            for (int i = 1; i < pathCount; ++i)
            {
                _correctedTrajectoryPath[i] = _curvedEndTrajectoryPath[i];
                _correctedPathDistance += Vector3.Distance(_correctedTrajectoryPath[i-1], _correctedTrajectoryPath[i]);
            }

            return _correctedTrajectoryPath;
        }

        public float GetCorrectedDurationByDistance(float distance, float duration)
        {
            return (duration / distance) * _correctedPathDistance;
        }


        public Vector3[] GetReverseCorrectedTrajectoryPath()
        {
            return GetReverseTrajectoryPath(_correctedTrajectoryPath);
        }
        private Vector3[] GetReverseTrajectoryPath(Vector3[] trajectoryPath)
        {
            Vector3[] reverseTrajectoryPath = new Vector3[trajectoryPath.Length];
            trajectoryPath.CopyTo(reverseTrajectoryPath, 0);
            Array.Reverse(reverseTrajectoryPath);

            return reverseTrajectoryPath;
        }
        
    }
}