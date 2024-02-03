using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
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
        private string IgnoreLedgeTag => _ledgeDetectionConfig.IgnoreLedgeTag;

        

        private readonly CollisionProbingConfig _groundProbingConfig;
        private LayerMask GroundProbeMask => _groundProbingConfig.CollisionLayerMask;
        private float GroundProbeDistance => _groundProbingConfig.ProbeDistance;
        private QueryTriggerInteraction GroundQueryTriggerInteraction => _groundProbingConfig.QueryTriggerInteraction;
        
        
        
        private Vector3 _position;
        private Vector3 _movementInput;
        private Vector3 _movementDirection;

        private readonly Quaternion _leftPerpendicular;
        private readonly Quaternion _rightPerpendicular;

        private bool _checkingIgnoreLedges;


        public LedgeDetectionController(LedgeDetectionConfig ledgeDetectionLedgeDetectionConfig,
            CollisionProbingConfig groundProbingConfig)
        {
            _ledgeDetectionConfig = ledgeDetectionLedgeDetectionConfig;
            _groundProbingConfig = groundProbingConfig;

            _leftPerpendicular = Quaternion.AngleAxis(-90f, Vector3.up);
            _rightPerpendicular = Quaternion.AngleAxis(90f, Vector3.up);

            SetCheckingIgnoreLedges(true);
        }


        public void SetCheckingIgnoreLedges(bool checkingIgnoreLedges)
        {
            _checkingIgnoreLedges = checkingIgnoreLedges;
        }

        public Vector3 UpdateMovementDirectionFromMovementInput(Vector3 position, Vector3 movementInput)
        {
            _position = position;
            _movementDirection = _movementInput = movementInput;
            
            UpdateMoveDirectionOnLedge();
            
            return _movementDirection;
        }
        
        
        private void UpdateMoveDirectionOnLedge()
        {
            bool isHeadingTowardsLedge = CheckIsHeadingTowardsLedge(out Vector3 ledgeNormal, out float distanceFromLedge);
            if (!isHeadingTowardsLedge)
            {
                return;                
            }

            float movementLedgeNormalDot = Vector3.Dot(_movementInput, ledgeNormal);

            if (movementLedgeNormalDot < 0)
            {
                return;
            }
            
            float tFromLedge = Mathf.Clamp01((distanceFromLedge-LedgeDistance) / LedgeStartStopDistance);
            
            Vector3 projectedMoveDirection = Vector3.ProjectOnPlane(_movementInput, ledgeNormal);
            bool sideLedge = CheckIsOnLedge(projectedMoveDirection.normalized, 
                out Vector3 sideLedgeNormal, out float sideDistanceFromLedge);
            if (sideLedge)
            {
                projectedMoveDirection -= sideLedgeNormal *
                                    (1-Mathf.Clamp01((sideDistanceFromLedge-LedgeDistance)  / LedgeStartStopDistance));
            }

            
            bool alignedWithLedge = movementLedgeNormalDot > 0.95f;
            Vector3 correctedMoveDirection = alignedWithLedge ? _movementInput * tFromLedge : projectedMoveDirection;
            correctedMoveDirection = Vector3.LerpUnclamped(correctedMoveDirection, _movementInput * tFromLedge, 
                Mathf.Pow(tFromLedge, LedgeFriction));

            _movementDirection = correctedMoveDirection;
        }

        private bool CheckIsHeadingTowardsLedge(out Vector3 ledgeNormal, out float distanceFromLedge)
        {
            Vector3 movementInput = _movementInput;
            
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
            
            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 
                    GroundProbeDistance, GroundProbeMask, GroundQueryTriggerInteraction))
            {
                if (hit.normal.y >= MinLedgeDotProduct)
                {
                    return false;
                }
            }

            origin += (GroundProbeDistance * Vector3.down);
            probeDirection = -probeDirection;
            if (Physics.Raycast(origin, probeDirection, out RaycastHit ledgeHit, 
                    LedgeProbeBackwardDisplacement, GroundProbeMask, GroundQueryTriggerInteraction))
            {
                if (IgnoreLedgeHit(ledgeHit))
                {
                    return false;
                }
                
                ledgeNormal = ledgeHit.normal;

                Vector3 projectedLedgePosition = Vector3.ProjectOnPlane(ledgeHit.point, Vector3.up);
                Vector3 projectedPlayerPosition = Vector3.ProjectOnPlane(_position, Vector3.up);
                distanceFromLedge = Vector3.Distance(projectedLedgePosition, projectedPlayerPosition);
            }

            return true;
        }

        private bool IgnoreLedgeHit(RaycastHit ledgeHit)
        {
            return _checkingIgnoreLedges && 
                   ledgeHit.collider.CompareTag(IgnoreLedgeTag);
        }
        
    }
    
    
}