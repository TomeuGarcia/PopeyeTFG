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
        
        public Quaternion ComputePathLookRotationBetweenIndices(Vector3[] pathPoints, int startIndex, int endIndex)
        {
            Vector3 pathForward = (pathPoints[endIndex] - pathPoints[startIndex]).normalized;
            Vector3 up = Vector3.up;
            Vector3 right = Vector3.Cross(pathForward, up).normalized;
            up = Vector3.Cross(pathForward, right).normalized;

            return Quaternion.LookRotation(pathForward, up);
        }
        
        
        
        
        
        
        
        
        
        //// NEW
        
        /*
        1. Check floor normal (to determine straight or floor aligned throw).
        2. Trace straight line with the previously determined direction and the given length (throw distance).
        3. If find any auto-targets
            a) Build Auto-target trajectory:
                    - Overwrite direction & distance
                    - Steps [0,N-1] → direction + distance + height offset curve
                    - Step N → reach snap position
        4. Otherwise
            a) Build a Normal trajectory:
                    - Steps [0,N-1] → direction + distance + height offset curve
                    - Step N → check for floor & determine EndsOnVoid
            b) Check collisions.
                    - Rebuild the trajectory if collision.

         */


        private Vector3[] _straightLineTrajectoryPoints = new Vector3[2];
        private Vector3[] _curvedTrajectoryPoints = new Vector3[20];
        private LayerMask _autoTargetLayerMask;
        private LayerMask _obstaclesLayerMask;
        private AnimationCurve HeightOffsetCurve;
        private float FloorProbeDistance;
        
        public Vector3[] ComputeUpdatedTrajectory(Vector3 startPosition, Vector3 direction, Vector3 floorNormal, 
            float distance, out float trajectoryDistance)
        {
            MakeStraightLineTrajectory(_straightLineTrajectoryPoints, startPosition, direction, distance);
            if (CheckForAutoAimTarget(_straightLineTrajectoryPoints, out IAutoAimTarget autoAimTarget))
            {
                MakeAutoTargetTrajectory(_curvedTrajectoryPoints, startPosition, floorNormal, out trajectoryDistance,
                    autoAimTarget);
                return _curvedTrajectoryPoints;
            }

            
            // Floor at the end of the trajectory
            if (CheckFloorHitInTrajectoryPoint(_straightLineTrajectoryPoints, _straightLineTrajectoryPoints.Length-1, 
                0.1f, out RaycastHit floorHit, _obstaclesLayerMask, QueryTriggerInteraction.Ignore))
            {
                MakeCurvedHeightWithEndTrajectory(_curvedTrajectoryPoints, startPosition, floorHit.point, 
                    direction, distance, floorNormal, HeightOffsetCurve, out trajectoryDistance);
            }
            else
            {
                MakeCurvedHeightTrajectory(_curvedTrajectoryPoints, startPosition, direction, distance,
                    floorNormal, HeightOffsetCurve, out trajectoryDistance);
                
                // NO FLOOR EXISTS
            }


            // Obstacle in the way of the trajectory
            if (CheckFirstHitInTrajectory(_curvedTrajectoryPoints, 0.1f, out int trajectoryCollisionIndex, 
                    out RaycastHit obstacleHit, _obstaclesLayerMask, QueryTriggerInteraction.Ignore))
            {
                if (!RemakeTrajectoryAfterCollisionHit(_curvedTrajectoryPoints, trajectoryCollisionIndex, 
                        -direction, 1.0f, obstacleHit, out floorHit))
                {
                    // TODO compute for void ????? aaaaaaahhh
                    
                    // NO FLOOR EXISTS
                }
            }



            return _curvedTrajectoryPoints;
        }

        
        private void MakeStraightLineTrajectory(Vector3[] trajectoryPoints, Vector3 startPosition, Vector3 direction, 
            float distance)
        {
            trajectoryPoints[0] = startPosition;

            int numberOfSteps = trajectoryPoints.Length - 1;
            float distancePerStep = distance / numberOfSteps;
            Vector3 displacementPerStep = direction * distancePerStep;

            for (int i = 1; i < trajectoryPoints.Length; ++i)
            {
                trajectoryPoints[i] = trajectoryPoints[i - 1] + displacementPerStep;
            }
        }

        private void MakeCurvedHeightTrajectory(Vector3[] trajectoryPoints, Vector3 startPosition, 
            Vector3 direction, float distance, Vector3 offsetDirection, AnimationCurve offsetCurve, 
            out float trajectoryDistance)
        {
            trajectoryDistance = distance;
            trajectoryPoints[0] = startPosition;
            
            int numberOfSteps = trajectoryPoints.Length - 1;
            float distancePerStep = distance / numberOfSteps;
            Vector3 displacementPerStep = direction * distancePerStep;

            for (int i = 1; i < trajectoryPoints.Length; ++i)
            {
                float t = (float)i / numberOfSteps;
                float offset = offsetCurve.Evaluate(t);
                trajectoryPoints[i] = trajectoryPoints[i - 1] + displacementPerStep + 
                                      offsetDirection * offset;

                trajectoryDistance += offset;
            }
        }
        private void MakeCurvedHeightWithEndTrajectory(Vector3[] trajectoryPoints, Vector3 startPosition, Vector3 endPosition, 
            Vector3 direction, float distance, Vector3 offsetDirection, AnimationCurve offsetCurve, 
            out float trajectoryDistance)
        {
            trajectoryDistance = distance;
            trajectoryPoints[0] = startPosition;
            
            int numberOfSteps = trajectoryPoints.Length - 2;
            float distancePerStep = distance / numberOfSteps;
            Vector3 displacementPerStep = direction * distancePerStep;

            for (int i = 1; i < trajectoryPoints.Length - 1; ++i)
            {
                float t = (float)i / numberOfSteps;
                float offset = offsetCurve.Evaluate(t);
                trajectoryPoints[i] = trajectoryPoints[i - 1] + displacementPerStep + 
                                      offsetDirection * offset;

                trajectoryDistance += offset;
            }

            trajectoryPoints[^1] = endPosition;

            trajectoryDistance += Vector3.Distance(trajectoryPoints[^2], endPosition);
        }
        
        
        

        private bool CheckForAutoAimTarget(Vector3[] trajectoryPoints, out IAutoAimTarget autoAimTarget)
        {
            if (CheckFirstHitInTrajectory(trajectoryPoints, 0.1f, out int trajectoryCollisionIndex, 
                    out RaycastHit hit, _autoTargetLayerMask, QueryTriggerInteraction.Collide))
            {
                autoAimTarget = null;
                return false;
            }
            
            if (!hit.collider.gameObject.TryGetComponent<IAutoAimTarget>(out autoAimTarget))
            {
                return false;
            }

            return true;
        }

        private bool CheckFirstHitInTrajectory(Vector3[] trajectoryPoints, float extraDistance, out int trajectoryCollisionIndex,
            out RaycastHit hit, LayerMask layerMask, QueryTriggerInteraction queryTriggerInteraction)
        {
            for (int i = 1; i < trajectoryPoints.Length; ++i)
            {
                Vector3 origin = trajectoryPoints[i - 1]; 
                Vector3 end = trajectoryPoints[i];

                if (CheckRaycastHit(origin, end, extraDistance, out hit, layerMask, queryTriggerInteraction))
                {
                    trajectoryCollisionIndex = i;
                    return true;
                }
            }

            trajectoryCollisionIndex = -1;
            hit = default;
            return false;
        }

        private bool CheckFloorHitInTrajectoryPoint(Vector3[] trajectoryPoints, int trajectoryPointIndex,
            float extraDistance, out RaycastHit hit, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
        {
            Vector3 origin = trajectoryPoints[trajectoryPointIndex]; 

            return CheckFloorHit(origin, extraDistance, FloorProbeDistance, out hit, layerMask, queryTriggerInteraction);
        }
        private bool CheckFloorHit(Vector3 origin, float extraDistance, float floorProbeDistance,
            out RaycastHit hit, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
        {
            Vector3 end = origin + (Vector3.down * floorProbeDistance);

            return CheckRaycastHit(origin, end, extraDistance, out hit, layerMask, queryTriggerInteraction);
        }

        private bool CheckRaycastHit(Vector3 origin, Vector3 end, float extraDistance, out RaycastHit hit, 
            int layerMask, QueryTriggerInteraction queryTriggerInteraction)
        {
            Vector3 originToEnd = end - origin;
            float distance = originToEnd.magnitude + extraDistance;
            Vector3 direction = originToEnd.normalized;
            
            return Physics.Raycast(origin, direction, out hit, distance, layerMask, queryTriggerInteraction);
        }
        

        private void MakeAutoTargetTrajectory(Vector3[] trajectoryPoints, Vector3 startPosition, Vector3 floorNormal, 
            out float trajectoryDistance, IAutoAimTarget autoAimTarget)
        {
            Vector3 autoAimTargetPosition = autoAimTarget.GetAimLockPosition();
            Vector3 startToAutoAim = autoAimTargetPosition - startPosition;
            Vector3 projectedStartToAutoAim = Vector3.ProjectOnPlane(startToAutoAim, floorNormal); 
            Vector3 direction = projectedStartToAutoAim.normalized;
            float distance = projectedStartToAutoAim.magnitude;
                
            MakeCurvedHeightWithEndTrajectory(trajectoryPoints, startPosition, autoAimTargetPosition,
                direction, distance, floorNormal, HeightOffsetCurve, out trajectoryDistance);
        }

        private bool RemakeTrajectoryAfterCollisionHit(Vector3[] trajectoryPoints, int trajectoryCollisionIndex,
            Vector3 bounceDirection, float bounceOffset, RaycastHit collisionHit, out RaycastHit floorHit)
        {
            Vector3 origin = collisionHit.point - (bounceDirection * bounceOffset);


            if (!CheckFloorHit(origin, 0.1f, FloorProbeDistance, out floorHit,
                    _obstaclesLayerMask, QueryTriggerInteraction.Ignore))
            {
                // TODO
                return false;
            }
            
            
            trajectoryPoints[trajectoryCollisionIndex] = collisionHit.point;
            for (int i = trajectoryCollisionIndex + 1; i < trajectoryPoints.Length; ++i)
            {
                // TODO
            }

            return true;
        }
        
    }
}