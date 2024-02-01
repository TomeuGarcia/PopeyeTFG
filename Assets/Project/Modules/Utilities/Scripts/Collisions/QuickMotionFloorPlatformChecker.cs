using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using UnityEngine;

namespace Popeye.Modules.Utilities.Scripts.Collisions
{
    public class QuickMotionFloorPlatformChecker
    {
        private CollisionProbingConfig _floorPlatformsProbingConfig;
        
        private LayerMask FloorLayerMask => _floorPlatformsProbingConfig.CollisionLayerMask;
        private float FloorProbeDistance => _floorPlatformsProbingConfig.ProbeDistance;
        private float _extraDistanceNoFloorFarPlatform;
        private float _distanceFromPlatformLedge;


        public QuickMotionFloorPlatformChecker(CollisionProbingConfig floorPlatformsProbingConfig,
            float extraDistanceNoFloorFarPlatform = 1.0f, float distanceFromPlatformLedge = 1.0f)
        {
            _floorPlatformsProbingConfig = floorPlatformsProbingConfig;
            _extraDistanceNoFloorFarPlatform = extraDistanceNoFloorFarPlatform;
            _distanceFromPlatformLedge = distanceFromPlatformLedge;
        }
        
        
        public Vector3 ComputeEndPositionCheckingForFloor(Vector3 startPosition, Vector3 endPosition,
            out float distanceChangeRatio01)
        {
            // Check Floor
            Vector3 probeOrigin = endPosition;
            if (Physics.Raycast(probeOrigin, Vector3.down, out RaycastHit floorHit,
                    FloorProbeDistance, FloorLayerMask, QueryTriggerInteraction.Ignore))
            {
                distanceChangeRatio01 = 1;
                return endPosition;
            }
            
            Vector3 startToEnd = endPosition - startPosition;
            Vector3 startToEndDirection = startToEnd.normalized;
            float startToEndDistance = startToEnd.magnitude;

            
            // Check for far front platform (past no floor)
            probeOrigin = endPosition + (Vector3.down * FloorProbeDistance);
            if (Physics.Raycast(probeOrigin, startToEndDirection, out RaycastHit farPlatformHit,
                    _extraDistanceNoFloorFarPlatform, FloorLayerMask, QueryTriggerInteraction.Ignore))
            {
                distanceChangeRatio01 = 1;
                return GetEndPositionOnPlatformBorder(endPosition, farPlatformHit);
            }
            
            
            // Check for near front platform (before no floor) 
            if (Physics.Raycast(probeOrigin, -startToEndDirection, out RaycastHit nearPlatformHit,
                    startToEndDistance, FloorLayerMask, QueryTriggerInteraction.Ignore))
            {
                distanceChangeRatio01 = nearPlatformHit.distance / startToEndDistance;
                return GetEndPositionOnPlatformBorder(endPosition, nearPlatformHit);
            }
            

            distanceChangeRatio01 = 0;
            return startPosition;
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