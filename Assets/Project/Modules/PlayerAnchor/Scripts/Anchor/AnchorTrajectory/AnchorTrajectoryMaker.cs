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


        private void DrawDebugLines(Vector3[] trajectory1, Vector3[] trajectory2, Vector3[] trajectory3 = null)
        {
            _debugLine.positionCount = trajectory1.Length;
            _debugLine.SetPositions(trajectory1);
            
            _debugLine2.positionCount = trajectory2.Length;
            _debugLine2.SetPositions(trajectory2);

            if (trajectory3 != null)
            {
                _debugLine3.positionCount = trajectory2.Length;
                _debugLine3.SetPositions(trajectory2);
            }

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
                DrawDebugLines(_straightTrajectoryPath, _curvedEndTrajectoryPath);
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
        
        public Quaternion ComputePathLookRotationBetweenIndices(Vector3[] pathPoints, int startIndex, int endIndex,
            Vector3 right)
        {
            Vector3 pathForward = (pathPoints[endIndex] - pathPoints[startIndex]).normalized;
            Vector3 up = Vector3.Cross(pathForward, right).normalized;

            return Quaternion.LookRotation(pathForward, up);
        }
        
        
        
        
        
        
        
        
        
        //// NEW

        private Vector3[] _straightLineTrajectoryPoints;
        private Vector3[] _curvedTrajectoryPoints;
        private AnchorThrowConfig _throwConfig;
        private LayerMask AutoTargetLayerMask => _throwConfig.AutoTargetLayerMask;
        private LayerMask ObstaclesLayerMask => _throwConfig.ObstaclesLayerMask;
        private AnimationCurve HeightOffsetCurve => _throwConfig.HeightDisplacementCurve;
        private float FloorProbeDistance => _throwConfig.HeightToConsiderFloor;
        private float FloorDotThreshold => _throwConfig.MaxSteepDotToConsiderFloor;

        public void SuperDuperSetup(AnchorThrowConfig throwConfig, int trajectorySteps)
        {
            _straightLineTrajectoryPoints = new Vector3[2];
            _curvedTrajectoryPoints = new Vector3[trajectorySteps];
            _throwConfig = throwConfig;
        }
        
        public Vector3[] ComputeUpdatedTrajectory(Vector3 startPosition, Vector3 direction, Vector3 floorNormal, 
            float distance, out float trajectoryDistance, out bool trajectoryEndsOnTheFloor, 
            out IAutoAimTarget autoAimTarget, out bool validAutoAimTarget)
        {
            
            MakeStraightLineTrajectory(_straightLineTrajectoryPoints, startPosition, direction, distance);
            if (CheckForAutoAimTarget(_straightLineTrajectoryPoints, out autoAimTarget))
            {
                if (autoAimTarget.CanBeAimedFromPosition(startPosition))
                {
                    MakeAutoTargetTrajectory(_curvedTrajectoryPoints, startPosition, floorNormal, out trajectoryDistance,
                        autoAimTarget);
                    
                    validAutoAimTarget = true;
                    trajectoryEndsOnTheFloor = true;
                    return _curvedTrajectoryPoints;
                }
            }

            
            validAutoAimTarget = false;
            MakeCurvedTrajectory(_curvedTrajectoryPoints, startPosition, direction, distance, 
                floorNormal, HeightOffsetCurve, out trajectoryDistance);

            
            bool obstacleInTheWayOfTheTrajectory =
                CheckFirstHitInTrajectory(_curvedTrajectoryPoints, 0.1f, out int lastIndexBeforeCollision,
                    out RaycastHit hit, ObstaclesLayerMask, QueryTriggerInteraction.Ignore);
            if (obstacleInTheWayOfTheTrajectory)
            {
                trajectoryEndsOnTheFloor =
                    MakeHitObstacleTrajectory(_curvedTrajectoryPoints, lastIndexBeforeCollision,
                        out Vector3[] trajectoryPointsAfterCollision, direction, hit);

                return trajectoryPointsAfterCollision;
            }
            
            
            trajectoryEndsOnTheFloor =
                CheckFloorHitInTrajectoryPoint(_straightLineTrajectoryPoints, _straightLineTrajectoryPoints.Length - 1,
                    0.1f, out RaycastHit floorHit, ObstaclesLayerMask, QueryTriggerInteraction.Ignore);
            if (trajectoryEndsOnTheFloor)
            {
                RemakeTrajectoryEnd(_curvedTrajectoryPoints, floorHit.point, 
                    distance, out distance);
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

        private void MakeCurvedTrajectory(Vector3[] trajectoryPoints, Vector3 startPosition, 
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
                trajectoryPoints[i] = startPosition + (displacementPerStep * i) + 
                                      offsetDirection * offset;

                trajectoryDistance += offset;
            }
        }

        private void RemakeTrajectoryEnd(Vector3[] trajectoryPoints, Vector3 endPosition,
            float currentTrajectoryDistance, out float updatedTrajectoryDistance)
        {
            Vector3 secondToLastPoint = trajectoryPoints[^2];
            Vector3 lastPoint = trajectoryPoints[^1];
            Vector3 newSecondToLastPoint = Vector3.LerpUnclamped(secondToLastPoint, lastPoint, 0.5f);

            trajectoryPoints[^2] = newSecondToLastPoint;
            trajectoryPoints[^1] = endPosition;
            
            updatedTrajectoryDistance = currentTrajectoryDistance;
            updatedTrajectoryDistance -= Vector3.Distance(secondToLastPoint, lastPoint) / 2;
            updatedTrajectoryDistance += Vector3.Distance(newSecondToLastPoint, endPosition);
        }
        
        

        private bool CheckForAutoAimTarget(Vector3[] trajectoryPoints, out IAutoAimTarget autoAimTarget)
        {
            if (!CheckFirstHitInTrajectory(trajectoryPoints, 0.1f, out int lastIndexBeforeCollision, 
                    out RaycastHit hit, AutoTargetLayerMask, QueryTriggerInteraction.Collide))
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

        private bool CheckFirstHitInTrajectory(Vector3[] trajectoryPoints, float extraDistance, out int lastIndexBeforeCollision,
            out RaycastHit hit, LayerMask layerMask, QueryTriggerInteraction queryTriggerInteraction)
        {
            for (int i = 0; i < trajectoryPoints.Length - 1; ++i)
            {
                Vector3 origin = trajectoryPoints[i]; 
                Vector3 end = trajectoryPoints[i + 1];

                if (CheckRaycastHit(origin, end, extraDistance, out hit, layerMask, queryTriggerInteraction))
                {
                    lastIndexBeforeCollision = i;
                    return true;
                }
            }

            lastIndexBeforeCollision = -1;
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
                
            MakeCurvedTrajectory(trajectoryPoints, startPosition, 
                direction, distance, floorNormal, HeightOffsetCurve, out trajectoryDistance);
            RemakeTrajectoryEnd(trajectoryPoints, autoAimTargetPosition, 
                trajectoryDistance, out trajectoryDistance);
        }

        private bool RemakeTrajectoryAfterCollisionHit(Vector3[] trajectoryPoints, int lastIndexBeforeCollision,
            Vector3 bounceDirection, float bounceOffset, RaycastHit collisionHit, out RaycastHit floorHit)
        {
            Vector3 origin = collisionHit.point + (bounceDirection * bounceOffset);


            bool trajectoryEndsOnTheFloor =
                CheckFloorHit(origin, 0.1f, FloorProbeDistance, out floorHit,
                    ObstaclesLayerMask, QueryTriggerInteraction.Ignore);

            
            Vector3 endPoint = trajectoryEndsOnTheFloor ? floorHit.point : origin + (Vector3.down * 3.0f);

            trajectoryPoints[lastIndexBeforeCollision] = collisionHit.point;

            int remainingStepsAfterHit = trajectoryPoints.Length - lastIndexBeforeCollision - 1;

            Vector3 collisionToEnd = endPoint - collisionHit.point;
            Vector3 collisionToEndDirection = collisionToEnd.normalized;
            float collisionToEndDistance = collisionToEnd.magnitude;

            float distancePerStep = collisionToEndDistance / remainingStepsAfterHit;
            Vector3 displacementPerStep = collisionToEndDirection * distancePerStep;

            for (int i = lastIndexBeforeCollision + 1; i < trajectoryPoints.Length; ++i)
            {
                trajectoryPoints[i] = trajectoryPoints[i - 1] + displacementPerStep;
            }

            trajectoryPoints[^1] = endPoint;

            return trajectoryEndsOnTheFloor;
        }

        private float ComputeTrajectoryDistance(Vector3[] trajectoryPoints)
        {
            float trajectoryDistance = 0.0f;
            for (int i = 0; i < trajectoryPoints.Length-1; ++i)
            {
                trajectoryDistance = Vector3.Distance(trajectoryPoints[i], trajectoryPoints[i + 1]);
            }

            return trajectoryDistance;
        }


        private bool MakeHitObstacleTrajectory(Vector3[] originalTrajectoryPoints, int lastIndexBeforeCollision, 
            out Vector3[] resultingTrajectoryPoints, Vector3 direction, RaycastHit obstacleHit)
        {
            bool obstacleIsFloor = CheckHitObstacleIsFloor(obstacleHit);

            if (obstacleIsFloor)
            {
                Vector3[] cappedCurvedTrajectoryPoints =
                    MakeCappedTrajectory(originalTrajectoryPoints, lastIndexBeforeCollision, obstacleHit.point);

                resultingTrajectoryPoints = cappedCurvedTrajectoryPoints;
                return true;
            }
                
            bool trajectoryEndsOnTheFloorAfterBouncingOnObstacle =
                RemakeTrajectoryAfterCollisionHit(originalTrajectoryPoints, lastIndexBeforeCollision, 
                    -direction, 1.0f, obstacleHit, out RaycastHit floorHitAfterBounce);

            resultingTrajectoryPoints = originalTrajectoryPoints;
            return trajectoryEndsOnTheFloorAfterBouncingOnObstacle;
        }
        

        private bool CheckHitObstacleIsFloor(RaycastHit hit)
        {
            return Vector3.Dot(Vector3.up, hit.normal) > FloorDotThreshold;
        }
        
        private Vector3[] MakeCappedTrajectory(Vector3[] originalTrajectoryPoints, int indexToCapAt,
            Vector3 capPosition)
        {
            Vector3[] cappedCurvedTrajectoryPoints = new Vector3[indexToCapAt + 1];
            for (int i = 0; i < indexToCapAt; ++i)
            {
                cappedCurvedTrajectoryPoints[i] = _curvedTrajectoryPoints[i];
            }
            cappedCurvedTrajectoryPoints[indexToCapAt] = capPosition;

            return cappedCurvedTrajectoryPoints;
        }
        
    }
}