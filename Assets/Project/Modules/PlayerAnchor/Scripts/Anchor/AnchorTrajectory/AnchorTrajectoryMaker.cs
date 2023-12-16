using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class AnchorTrajectoryMaker
    {
        private Vector3[] _straightLineTrajectoryPoints;
        private Vector3[] _curvedTrajectoryPoints;
        
        private ObstacleProbingConfig _obstacleProbingConfig;
        private LayerMask AutoTargetLayerMask => _obstacleProbingConfig.AutoTargetLayerMask;
        private LayerMask ObstaclesLayerMask => _obstacleProbingConfig.ObstaclesLayerMask;
        private float FloorProbeDistance => _obstacleProbingConfig.HeightToConsiderFloor;
        private float FloorDotThreshold => _obstacleProbingConfig.MaxSteepDotToConsiderFloor;
        
        
        
        private AnchorTrajectoryEndSpot _trajectoryEndSpot;
        private AnchorPullConfig _anchorPullConfig;
        

        private LineRenderer _debugLine;
        private LineRenderer _debugLine2;
        private LineRenderer _debugLine3;

        public bool drawDebugLines = false;
        
        public void Configure(AnchorTrajectoryEndSpot trajectoryEndSpot, 
            ObstacleProbingConfig obstacleProbingConfig, AnchorPullConfig anchorPullConfig,
            LineRenderer debugLine, LineRenderer debugLine2, LineRenderer debugLine3)
        {
            _trajectoryEndSpot = trajectoryEndSpot;
            _obstacleProbingConfig = obstacleProbingConfig;
            _anchorPullConfig = anchorPullConfig;

            _trajectoryEndSpot.Hide();
            
            _straightLineTrajectoryPoints = new Vector3[2];
            _curvedTrajectoryPoints = new Vector3[10];
            
            
            _debugLine = debugLine;
            _debugLine2 = debugLine2;
            _debugLine3 = debugLine3;
        }

        public void DrawDebugLines()
        {
            DrawDebugLines(_straightLineTrajectoryPoints, _curvedTrajectoryPoints);
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
            _debugLine2.gameObject.SetActive(true);
        }
        public void HideTrajectoryEndSpot()
        {
            _trajectoryEndSpot.Hide();
            _debugLine2.gameObject.SetActive(false);
        }



        public void MakeTrajectoryEndSpotMatchSpot(Vector3 position, Vector3 lookDirection, bool endsOnFloor)
        {
            _trajectoryEndSpot.MatchSpot(position, lookDirection, endsOnFloor);
        }
        
        public Vector3[] ComputeCurvedTrajectory(Vector3 startPosition, Vector3 goalPosition, int numberOfSteps,
            out float trajectoryDistance)
        {
            if (numberOfSteps < 1)
            {
                throw new Exception("numberOfSteps must be greater than 0");
            }
            

            bool goalIsHigher = goalPosition.y > startPosition.y;
            
            float bendSharpness = goalIsHigher ? 
                1.0f/_anchorPullConfig.TrajectoryBendSharpness : 
                _anchorPullConfig.TrajectoryBendSharpness;
            
            return DoComputeCurvedTrajectory(startPosition, goalPosition, numberOfSteps, out trajectoryDistance,
                bendSharpness);
        }
        
        private Vector3[] DoComputeCurvedTrajectory(Vector3 startPosition, Vector3 goalPosition, int numberOfSteps,
            out float trajectoryDistance, float bendSharpness)
        {
            Vector3[] trajectoryPoints = new Vector3[numberOfSteps];
            trajectoryDistance = 0.0f;

            Vector3 anchorToGoal = goalPosition - startPosition;
            float distancePerStep = anchorToGoal.magnitude / (numberOfSteps-1);
            Vector3 anchorToGoalDirection = anchorToGoal.normalized;

            
            trajectoryPoints[0] = startPosition;
            for (int i = 1; i < numberOfSteps; ++i)
            {
                float t = (float)i / (numberOfSteps-1);
                t = Mathf.Pow(t, bendSharpness);
                
                Vector3 trajectoryPoint = startPosition + (anchorToGoalDirection * (distancePerStep * i));
                trajectoryPoint.y = Mathf.Lerp(startPosition.y, goalPosition.y, t);

                trajectoryPoints[i] = trajectoryPoint;

                trajectoryDistance += Vector3.Distance(trajectoryPoints[i - 1], trajectoryPoint);
            }

            return trajectoryPoints;
        }




        public Vector3[] ComputeUpdatedTrajectory(Vector3 startPosition, Vector3 direction, Vector3 floorNormal,
            AnimationCurve heightOffsetCurve, float distance, out float trajectoryDistance,
            out bool trajectoryEndsOnTheFloor,
            out RaycastHit obstacleHit, out bool trajectoryHitsObstacle)
        {
            MakeStraightLineTrajectory(_straightLineTrajectoryPoints, startPosition, direction, distance);
            return DoComputeUpdatedTrajectory(startPosition, direction, floorNormal,
                heightOffsetCurve, distance, out trajectoryDistance, out trajectoryEndsOnTheFloor,
                out obstacleHit, out trajectoryHitsObstacle);
        }
        
        public Vector3[] ComputeUpdatedTrajectoryWithAutoAim(Vector3 startPosition, Vector3 direction, Vector3 floorNormal, 
            AnimationCurve heightOffsetCurve, float distance, out float trajectoryDistance, out bool trajectoryEndsOnTheFloor, 
            out IAutoAimTarget autoAimTarget, out bool validAutoAimTarget, 
            out RaycastHit  obstacleHit, out bool trajectoryHitsObstacle)
        {
            MakeStraightLineTrajectory(_straightLineTrajectoryPoints, startPosition, direction, distance);
            
            if (CheckForAutoAimTarget(_straightLineTrajectoryPoints, out autoAimTarget))
            {
                if (autoAimTarget.CanBeAimedFromPosition(startPosition))
                {
                    MakeAutoTargetTrajectory(_curvedTrajectoryPoints, startPosition, floorNormal, heightOffsetCurve,
                        out trajectoryDistance, autoAimTarget);
                    
                    validAutoAimTarget = true;
                    trajectoryEndsOnTheFloor = true;
                    obstacleHit = default;
                    trajectoryHitsObstacle = false;
                    
                    return _curvedTrajectoryPoints;
                }
            }

            validAutoAimTarget = false;
            return ComputeUpdatedTrajectory(startPosition, direction, floorNormal,
                heightOffsetCurve, distance, out trajectoryDistance, out trajectoryEndsOnTheFloor,
                out obstacleHit, out trajectoryHitsObstacle);
        }

        private Vector3[] DoComputeUpdatedTrajectory(Vector3 startPosition, Vector3 direction, Vector3 floorNormal,
            AnimationCurve heightOffsetCurve, float distance, out float trajectoryDistance,
            out bool trajectoryEndsOnTheFloor,
            out RaycastHit obstacleHit, out bool trajectoryHitsObstacle)
        {
            MakeCurvedTrajectory(_curvedTrajectoryPoints, startPosition, direction, distance, 
                floorNormal, heightOffsetCurve, out trajectoryDistance);

            
            bool obstacleInTheWayOfTheTrajectory =
                CheckFirstHitInTrajectory(_curvedTrajectoryPoints, 0.1f, out int lastIndexBeforeCollision,
                    out obstacleHit, ObstaclesLayerMask, QueryTriggerInteraction.Ignore);
            if (obstacleInTheWayOfTheTrajectory)
            {
                trajectoryEndsOnTheFloor =
                    MakeHitObstacleTrajectory(_curvedTrajectoryPoints, lastIndexBeforeCollision,
                        out Vector3[] trajectoryPointsAfterCollision, direction, obstacleHit);

                trajectoryDistance = ComputeTrajectoryDistance(trajectoryPointsAfterCollision);
                trajectoryHitsObstacle = true;
                return trajectoryPointsAfterCollision;
            }
            
            
            trajectoryEndsOnTheFloor =
                CheckFloorHitInTrajectoryPoint(_straightLineTrajectoryPoints, _straightLineTrajectoryPoints.Length - 1,
                    0.1f, out RaycastHit floorHit, ObstaclesLayerMask, QueryTriggerInteraction.Ignore);
            if (trajectoryEndsOnTheFloor)
            {
                RemakeTrajectoryEnd(_curvedTrajectoryPoints, floorHit.point, 
                    distance, out distance);
                
                obstacleHit = floorHit;
            }

            trajectoryDistance = ComputeTrajectoryDistance(_curvedTrajectoryPoints);
            trajectoryHitsObstacle = trajectoryEndsOnTheFloor;
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
            updatedTrajectoryDistance = Mathf.Max(updatedTrajectoryDistance, 0);
            updatedTrajectoryDistance += Vector3.Distance(newSecondToLastPoint, endPosition);
        }
        
        

        private bool CheckForAutoAimTarget(Vector3[] trajectoryPoints, out IAutoAimTarget autoAimTarget)
        {
            if (CheckAtopHitInTrajectoryPoint(trajectoryPoints, 0, 0.5f, out RaycastHit hit,
                    AutoTargetLayerMask, QueryTriggerInteraction.Collide))
            {
                if (CheckHitIsAutoAimTarget(hit, out autoAimTarget))
                {
                    return true;
                }
            }
            
            if (CheckFirstHitInTrajectory(trajectoryPoints, 0.5f, out int lastIndexBeforeCollision, 
                    out hit, AutoTargetLayerMask, QueryTriggerInteraction.Collide))
            {
                if (CheckHitIsAutoAimTarget(hit, out autoAimTarget))
                {
                    return true;
                }
            }

            autoAimTarget = null;
            return false;
        }

        private bool CheckHitIsAutoAimTarget(RaycastHit hit, out IAutoAimTarget autoAimTarget)
        {
            return hit.collider.gameObject.TryGetComponent<IAutoAimTarget>(out autoAimTarget);
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
        private bool CheckAtopHitInTrajectoryPoint(Vector3[] trajectoryPoints, int trajectoryPointIndex,
            float extraDistance, out RaycastHit hit, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
        {
            Vector3 origin = trajectoryPoints[trajectoryPointIndex] + (Vector3.up * FloorProbeDistance); 

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
            AnimationCurve heightOffsetCurve, out float trajectoryDistance, IAutoAimTarget autoAimTarget)
        {
            Vector3 autoAimTargetPosition = autoAimTarget.GetAimLockPosition();
            Vector3 startToAutoAim = autoAimTargetPosition - startPosition;
            Vector3 projectedStartToAutoAim = Vector3.ProjectOnPlane(startToAutoAim, floorNormal); 
            Vector3 direction = projectedStartToAutoAim.normalized;
            float distance = projectedStartToAutoAim.magnitude;
                
            MakeCurvedTrajectory(trajectoryPoints, startPosition, 
                direction, distance, floorNormal, heightOffsetCurve, out trajectoryDistance);
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
                trajectoryDistance += Vector3.Distance(trajectoryPoints[i], trajectoryPoints[i + 1]);
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


        public Vector3[] ComputeUpAndDownTrajectory(Vector3 startPosition, float distance)
        {
            Vector3[] upAndDownTrajectory = ComputeBackAndForthTrajectory(startPosition, Vector3.up, distance);
            
            if (CheckFloorHit(upAndDownTrajectory[^1], 0.1f, FloorProbeDistance, out RaycastHit floorHit, 
                    ObstaclesLayerMask, QueryTriggerInteraction.Ignore))
            {
                upAndDownTrajectory[^1] = floorHit.point + (floorHit.normal * 0.2f);
            }

            return upAndDownTrajectory;
        }

        private Vector3[] ComputeBackAndForthTrajectory(Vector3 startPosition, Vector3 direction, float distance,
            int numberOfSteps = 3)
        {
            if (numberOfSteps < 3)
            {
                throw new Exception("numberOfSteps must be greater than 2");
            }

            if (numberOfSteps % 2 == 0)
            {
                ++numberOfSteps;
            }

            
            Vector3[] backAndForthTrajectory = new Vector3[numberOfSteps];
            int halfNumberOfSteps = numberOfSteps / 2;
            float distancePerStep = distance / halfNumberOfSteps;

            backAndForthTrajectory[0] = startPosition;
            for (int i = 1; i < halfNumberOfSteps; ++i)
            {
                backAndForthTrajectory[i] = startPosition + (direction * distancePerStep);
            }
            
            Vector3 furthestPoint = startPosition + (direction * distance);
            backAndForthTrajectory[halfNumberOfSteps] = furthestPoint;
            
            for (int i = halfNumberOfSteps + 1; i < numberOfSteps; ++i)
            {
                backAndForthTrajectory[i] = furthestPoint - (direction * distancePerStep);
            }
            
            return backAndForthTrajectory;
        }
        
        
    }
}