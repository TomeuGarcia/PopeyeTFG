using Popeye.Scripts.Collisions;
using Popeye.Scripts.ObjectTypes;
using UnityEngine;

namespace Popeye.Modules.PlayerController
{
    
    public class LedgeDetectionController
    {
        private readonly LedgeDetectionConfig _ledgeDetectionConfig;
        private float LedgeProbeForwardDisplacement => _ledgeDetectionConfig.LedgeProbeForwardDisplacement;
        private float LedgeProbeBackwardDisplacement => _ledgeDetectionConfig.LedgeProbeBackwardDisplacement;
        private float LedgeDistance => _ledgeDetectionConfig.LedgeDistance;
        private float LedgeStartStopDistance => _ledgeDetectionConfig.LedgeStartStopDistance;
        private float LedgeFriction => _ledgeDetectionConfig.LedgeFriction;
        private float MinLedgeDotProduct => _ledgeDetectionConfig.MinLedgeDotProduct;
        private ObjectTypeAsset IgnoreLedgeObjectType => _ledgeDetectionConfig.IgnoreLedgeObjectType;

        

        private LayerMask GroundProbeMask => _ledgeDetectionConfig.GroundProbingConfig.CollisionLayerMask;
        private float GroundProbeDistance => _ledgeDetectionConfig.GroundProbingConfig.ProbeDistance;
        private QueryTriggerInteraction GroundQueryTriggerInteraction => _ledgeDetectionConfig.GroundProbingConfig.QueryTriggerInteraction;
        
        private LayerMask LedgeProbeMask => _ledgeDetectionConfig.LedgeProbingConfig.CollisionLayerMask;
        private float LedgeProbeDistance => _ledgeDetectionConfig.LedgeProbingConfig.ProbeDistance;
        private QueryTriggerInteraction LedgeQueryTriggerInteraction => _ledgeDetectionConfig.LedgeProbingConfig.QueryTriggerInteraction;
        
        
        
        private Vector3 _position;
        private Vector3 _movementInput;
        private Vector3 _movementDirection;

        private readonly Quaternion _leftPerpendicular;
        private readonly Quaternion _rightPerpendicular;

        private bool _checkingIgnoreLedges;


        public LedgeDetectionController(LedgeDetectionConfig ledgeDetectionLedgeDetectionConfig)
        {
            _ledgeDetectionConfig = ledgeDetectionLedgeDetectionConfig;

            _leftPerpendicular = Quaternion.AngleAxis(-90f, Vector3.up);
            _rightPerpendicular = Quaternion.AngleAxis(90f, Vector3.up);

            SetCheckingIgnoreLedges(true);
        }


        public void SetCheckingIgnoreLedges(bool checkingIgnoreLedges)
        {
            _checkingIgnoreLedges = checkingIgnoreLedges;
        }

        public Vector3 UpdateMovementDirectionFromMovementInput(Vector3 position, Vector3 movementInput,
            out bool isOnLedge)
        {
            _position = position + Vector3.up * 0.001f;
            _movementDirection = _movementInput = movementInput;
            
            UpdateMoveDirectionOnLedge(out isOnLedge);
            
            return _movementDirection;
        }
        
        
        private void UpdateMoveDirectionOnLedge(out bool isOnLedge)
        {
            bool isHeadingTowardsLedge = CheckIsHeadingTowardsLedge(out Vector3 ledgeNormal, out float distanceFromLedge);
            if (!isHeadingTowardsLedge)
            {
                isOnLedge = false;
                return;                
            }

            float movementLedgeNormalDot = Vector3.Dot(_movementInput, ledgeNormal);

            if (movementLedgeNormalDot < 0)
            {
                isOnLedge = false;
                return;
            }
            
            isOnLedge = true;
            
            float tFromLedge = (distanceFromLedge-LedgeDistance) / (LedgeStartStopDistance-LedgeDistance);

            bool isFarFromLedge = tFromLedge > 1;
            
            if (isFarFromLedge)
            {
                isOnLedge = false;
                return;
            }

            
            Vector3 projectedMoveDirection = Vector3.ProjectOnPlane(_movementInput, ledgeNormal);
            
            bool sideLedge = CheckIsOnLedge(projectedMoveDirection.normalized, 
                out Vector3 sideLedgeNormal, out float sideDistanceFromLedge);
            if (sideLedge)
            {
                float tFromSideLedge = (sideDistanceFromLedge-LedgeDistance) / (LedgeStartStopDistance-LedgeDistance);
                projectedMoveDirection -= sideLedgeNormal * (1-tFromSideLedge);
            }

            
            Vector3 correctedMoveDirection = projectedMoveDirection;
            
            _movementDirection = correctedMoveDirection;
        }

        public void DrawGizmos()
        {
            Vector3 position = _position + Vector3.up * 2;
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(position, position + _movementDirection);
        }
        
        private bool CheckIsHeadingTowardsLedge(out Vector3 ledgeNormal, out float distanceFromLedge)
        {
            Vector3 movementInput = _movementInput.normalized;
            
            bool forwardLedge = CheckIsOnLedge(movementInput, out ledgeNormal, out distanceFromLedge);
            if (forwardLedge)
            {
                return true;
            }

            Vector3 leftMovementInput = _leftPerpendicular * movementInput;
            bool leftLedge = CheckIsOnLedge(leftMovementInput, out ledgeNormal, out distanceFromLedge);
            if (leftLedge)
            {
                return true;
            }
            
            Vector3 rightMovementInput = _rightPerpendicular * movementInput;
            bool rightLedge = CheckIsOnLedge(rightMovementInput, out ledgeNormal, out distanceFromLedge);
            if (rightLedge)
            {
                return true;
            }

            return false;
        }

        private bool CheckIsOnLedge(Vector3 probeDirection, out Vector3 ledgeNormal, out float distanceFromLedge)
        {
            ledgeNormal = Vector3.zero;
            distanceFromLedge = 0;
            
            Vector3 origin = _position + (probeDirection * LedgeProbeForwardDisplacement);
            
            if (Physics.Raycast(origin, Vector3.down, out RaycastHit floorHit, 
                    GroundProbeDistance, GroundProbeMask, GroundQueryTriggerInteraction))
            {
                if (floorHit.normal.y >= MinLedgeDotProduct)
                {
                    return false;
                }
            }

            origin += (LedgeProbeDistance * Vector3.down);
            probeDirection = -probeDirection;
            if (Physics.Raycast(origin, probeDirection, out RaycastHit ledgeHit, 
                    LedgeProbeBackwardDisplacement, LedgeProbeMask, LedgeQueryTriggerInteraction))
            {
                if (IgnoreLedgeHit(ledgeHit))
                {
                    return false;
                }
                
                ledgeNormal = ledgeHit.normal;

                Vector3 projectedLedgePosition = Vector3.ProjectOnPlane(ledgeHit.point, Vector3.up);
                Vector3 projectedPlayerPosition =  Vector3.ProjectOnPlane(_position, Vector3.up);

                Vector3 playerToLedge = projectedLedgePosition - projectedPlayerPosition;
                Vector3 projectedPlayerToLedge = Vector3.ProjectOnPlane(playerToLedge, ledgeNormal);

                distanceFromLedge = Vector3.Distance(playerToLedge, projectedPlayerToLedge);
            }
            return true;
        }
        
        private bool IgnoreLedgeHit(RaycastHit ledgeHit)
        {
            if (_checkingIgnoreLedges)
            {
                if (ledgeHit.collider.TryGetComponent(out ObjectTypeBehaviour objectTypeBehaviour))
                {
                    return objectTypeBehaviour.IsOfType(IgnoreLedgeObjectType);
                }
            }

            return false;
        }
        
    }
    
    
}