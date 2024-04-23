using Popeye.Scripts.Collisions;
using UnityEngine;

namespace Popeye.Modules.Utilities.Scripts.Collisions
{
    public class QuickMotionFloorPlatformChecker
    {
        private readonly CollisionProbingConfig _floorPlatformsProbingConfig;
        
        private LayerMask FloorLayerMask => _floorPlatformsProbingConfig.CollisionLayerMask;
        private float FloorProbeDistance => _floorPlatformsProbingConfig.ProbeDistance;
        private float _extraDistanceNoFloorFarPlatform;
        private float _distanceFromPlatformLedge;


        public QuickMotionFloorPlatformChecker(CollisionProbingConfig floorPlatformsProbingConfig,
            float extraDistanceNoFloorFarPlatform = 1.0f, float distanceFromPlatformLedge = 0.5f)
        {
            _floorPlatformsProbingConfig = floorPlatformsProbingConfig;
            _extraDistanceNoFloorFarPlatform = extraDistanceNoFloorFarPlatform;
            _distanceFromPlatformLedge = distanceFromPlatformLedge;
        }
        
        
        public Vector3 ComputeEndPosition_FrontRear(Vector3 startPosition, Vector3 endPosition,
            out float distanceChangeRatio01)
        {
            if (CheckFloorUnderEndPosition(endPosition))
            {
                distanceChangeRatio01 = 1;
                return endPosition;
            }
            
            
            ComputeNoFloorProbe(startPosition, endPosition, 
                out float startToEndDistance,
                out Vector3 startToEndDirection,
                out Vector3 probeOrigin);

            
            if (CheckNoFloorFrontPlatform(probeOrigin, startToEndDirection, 
                    out RaycastHit farPlatformHit))
            {
                distanceChangeRatio01 = 1;
                return GetEndPositionOnPlatformBorder(endPosition, farPlatformHit);
            }
            
            
            if (CheckNoFloorRearPlatform(probeOrigin, startToEndDirection, startToEndDistance, 
                    out RaycastHit rearPlatformHit))
            {
                distanceChangeRatio01 = rearPlatformHit.distance / startToEndDistance;
                return GetEndPositionOnPlatformBorder(endPosition, rearPlatformHit);
            }
            

            distanceChangeRatio01 = 0;
            return startPosition;
        }
        
        
        public Vector3 ComputeEndPosition_Rear(Vector3 startPosition, Vector3 endPosition,
            out float distanceChangeRatio01)
        {
            if (CheckFloorUnderEndPosition(endPosition))
            {
                distanceChangeRatio01 = 1;
                return endPosition;
            }

            ComputeNoFloorProbe(startPosition, endPosition, 
                out float startToEndDistance,
                out Vector3 startToEndDirection,
                out Vector3 probeOrigin);


            if (CheckNoFloorRearPlatform(probeOrigin, startToEndDirection, startToEndDistance, 
                    out RaycastHit rearPlatformHit))
            {
                distanceChangeRatio01 = rearPlatformHit.distance / startToEndDistance;
                return GetEndPositionOnPlatformBorder(endPosition, rearPlatformHit);
            }
            

            distanceChangeRatio01 = 0;
            return startPosition;
        }



        private bool CheckFloorUnderEndPosition(Vector3 endPosition)
        {
            return Physics.Raycast(endPosition, Vector3.down, out RaycastHit floorHit,
                FloorProbeDistance, FloorLayerMask, QueryTriggerInteraction.Ignore);
        }

        private void ComputeNoFloorProbe(Vector3 startPosition, Vector3 endPosition,
            out float startToEndDistance, out Vector3 startToEndDirection, out Vector3 probeOrigin)
        {
            Vector3 startToEnd = endPosition - startPosition;
            startToEndDistance = startToEnd.magnitude;
            startToEndDirection = startToEnd / startToEndDistance;
            probeOrigin = endPosition + (Vector3.down * FloorProbeDistance);
        }

        private bool CheckNoFloorFrontPlatform(Vector3 probeOrigin, Vector3 startToEndDirection,
            out RaycastHit frontNoPlatformHit)
        {
            return Physics.Raycast(probeOrigin, startToEndDirection, out frontNoPlatformHit,
                _extraDistanceNoFloorFarPlatform, FloorLayerMask, QueryTriggerInteraction.Ignore);
        }
        
        private bool CheckNoFloorRearPlatform(Vector3 probeOrigin, Vector3 startToEndDirection,
            float startToEndDistance, out RaycastHit rearNoPlatformHit)
        {
            return Physics.Raycast(probeOrigin, -startToEndDirection, out rearNoPlatformHit,
                startToEndDistance, FloorLayerMask, QueryTriggerInteraction.Ignore);
        }
        
        
        private Vector3 GetEndPositionOnPlatformBorder(Vector3 originalEndPosition, RaycastHit platformHit)
        {
            Vector3 platformBorderEndPosition = platformHit.point;
            platformBorderEndPosition.y = originalEndPosition.y;
            platformBorderEndPosition -= platformHit.normal * _distanceFromPlatformLedge;
            
            return platformBorderEndPosition;
        }
    }
}