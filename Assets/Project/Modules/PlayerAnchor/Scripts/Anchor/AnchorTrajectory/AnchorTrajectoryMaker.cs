using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class AnchorTrajectoryMaker
    {
        private AnchorTrajectoryEndSpot _trajectoryEndSpot;
        private AnchorThrowConfig _anchorThrowConfig;
        private AnchorPullConfig _anchorPullConfig;
        private TrajectoryHitChecker _trajectoryHitChecker;
        

        private Vector3[] _straightTrajectoryPath;
        private Vector3[] _curvedEndTrajectoryPath;
        
        private Vector3[] _correctedTrajectoryPath;
        private float _correctedPathDistance;
        
        private int _trajectoryHitIndex;

        public bool TrajectoryEndsOnVoid { get; private set; }


        private LineRenderer _debugLine;
        private LineRenderer _debugLine2;
        private LineRenderer _debugLine3;

        public bool drawDebugLines = false;
        
        public void Configure(AnchorTrajectoryEndSpot trajectoryEndSpot, 
            AnchorThrowConfig anchorThrowConfig, AnchorPullConfig anchorPullConfig,
            TrajectoryHitChecker trajectoryHitChecker,
            LineRenderer debugLine, LineRenderer debugLine2, LineRenderer debugLine3)
        {
            _trajectoryEndSpot = trajectoryEndSpot;
            _anchorThrowConfig = anchorThrowConfig;
            _anchorPullConfig = anchorPullConfig;
            _trajectoryHitChecker = trajectoryHitChecker;

            _trajectoryEndSpot.Hide();
            
            _straightTrajectoryPath = new Vector3[2];
            _curvedEndTrajectoryPath = new Vector3[20];
            _debugLine = debugLine;
            _debugLine2 = debugLine2;
            _debugLine3 = debugLine3;
        }


        private void DrawDebugLines()
        {
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


        public Vector3[] UpdateTrajectoryPath(Vector3 startPoint, Vector3 direction, float distance,
            bool updateTrajectoryEndSpot)
        {
            DoUpdateTrajectoryPath(_straightTrajectoryPath, startPoint, direction, distance);
            DoUpdateTrajectoryPath(_curvedEndTrajectoryPath, startPoint, direction, distance);
            CurveTrajectoryPath(_curvedEndTrajectoryPath, _anchorThrowConfig.TrajectoryBendSharpness);

            if (updateTrajectoryEndSpot)
            {
                UpdateTrajectoryEndSpot();
            }

            if (drawDebugLines)
            {
                DrawDebugLines();
            }

            return _curvedEndTrajectoryPath;
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
            for (int i = trajectoryPath.Length / 2; i < trajectoryPath.Length; ++i)
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
        
        private void UpdateTrajectoryEndSpot()
        {
            Vector3 spotPosition;
            Vector3 spotLookDirection;
            
            if (_trajectoryHitChecker.GetFirstObstacleHitInTrajectoryPath(_curvedEndTrajectoryPath, 
                    out RaycastHit hit, out _trajectoryHitIndex))
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
            MakeTrajectoryEndSpotMatchSpot(spotPosition, spotLookDirection);
        }

        public void MakeTrajectoryEndSpotMatchSpot(Vector3 position, Vector3 lookDirection)
        {
            _trajectoryEndSpot.MatchSpot(position, lookDirection, !TrajectoryEndsOnVoid);
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



        public Vector3[] ComputeCurvedTrajectory(Vector3 anchorPosition, Vector3 goalPosition, int numberOfSteps,
            out float trajectoryDistance)
        {
            if (numberOfSteps < 1)
            {
                throw new Exception("numberOfSteps must be greater than 0");
            }
            

            bool goalIsHigher = goalPosition.y > anchorPosition.y;
            
            float bendSharpness = goalIsHigher ? 
                1.0f/_anchorPullConfig.TrajectoryBendSharpness : 
                _anchorPullConfig.TrajectoryBendSharpness;
            
            return DoComputeCurvedTrajectory(anchorPosition, goalPosition, numberOfSteps, out trajectoryDistance,
                bendSharpness);
        }
        
        private Vector3[] DoComputeCurvedTrajectory(Vector3 anchorPosition, Vector3 goalPosition, int numberOfSteps,
            out float trajectoryDistance, float bendSharpness)
        {
            Vector3[] trajectoryPoints = new Vector3[numberOfSteps];
            trajectoryDistance = 0.0f;

            Vector3 anchorToGoal = goalPosition - anchorPosition;
            float distancePerStep = anchorToGoal.magnitude / (numberOfSteps-1);
            Vector3 anchorToGoalDirection = anchorToGoal.normalized;

            
            trajectoryPoints[0] = anchorPosition;
            for (int i = 1; i < numberOfSteps; ++i)
            {
                float t = (float)i / (numberOfSteps-1);
                t = Mathf.Pow(t, bendSharpness);
                
                Vector3 trajectoryPoint = anchorPosition + (anchorToGoalDirection * (distancePerStep * i));
                trajectoryPoint.y = Mathf.Lerp(anchorPosition.y, goalPosition.y, t);

                trajectoryPoints[i] = trajectoryPoint;

                trajectoryDistance += Vector3.Distance(trajectoryPoints[i - 1], trajectoryPoint);
            }

            return trajectoryPoints;
        }
        
        
    }
}