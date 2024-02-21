using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using Popeye.Scripts.Collisions;
using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public class AutoAimTargetFilterer : IAutoAimTargetFilterer
    {
        private AutoAimTargetFilterConfig _filterConfig;
        private CollisionProbingConfig _probingConfig;

        private float AcceptanceFieldOfViewDot => _filterConfig.AcceptanceFieldOfViewDot;
        private float AcceptanceHeightDistance => _filterConfig.AcceptanceHeightDistance;
        private float LineOfSighRaysDistanceRange => _filterConfig.LineOfSighRaysDistanceRange;
        private int NumberOfLineOfSightRays => _filterConfig.NumberOfLineOfSightRays;
        private LayerMask LayerMask => _probingConfig.CollisionLayerMask;
        private QueryTriggerInteraction QueryTriggerInteraction => _probingConfig.QueryTriggerInteraction;
        

        public void Configure(AutoAimTargetFilterConfig filterConfig, CollisionProbingConfig probingConfig)
        {
            _filterConfig = filterConfig;
            _probingConfig = probingConfig;
        }

        public bool IsValidTarget(IAutoAimTarget autoAimTarget, Vector3 targeterPosition, 
            Vector3 targeterForwardDirection, Vector3 targeterUpDirection)
        {
            if (!autoAimTarget.CanBeAimedAt(targeterPosition))
            {
                return false;
            }

            Vector3 toTargetDirection = (autoAimTarget.Position - targeterPosition).normalized;
            if (IsOutsideFieldOfView(toTargetDirection, targeterForwardDirection))
            {
                return false;
            }
            
            if (!IsInsideAcceptanceHeight(autoAimTarget, targeterPosition))
            {
                return false;
            }

            if (IsViewObstructed(autoAimTarget, targeterPosition, toTargetDirection, targeterUpDirection))
            {
                return false;
            }

            return true;
        }


        private bool IsOutsideFieldOfView(Vector3 toTargetDirection, Vector3 targeterLookForwardDirection)
        {
            return Vector3.Dot(toTargetDirection, targeterLookForwardDirection) < AcceptanceFieldOfViewDot;
        }

        private bool IsInsideAcceptanceHeight(IAutoAimTarget autoAimTarget, Vector3 targeterPosition)
        {
            return Mathf.Abs(autoAimTarget.Position.y - targeterPosition.y) < AcceptanceHeightDistance;
        }
        
        private bool IsViewObstructed(IAutoAimTarget autoAimTarget, Vector3 targeterPosition, 
            Vector3 toTargetDirection, Vector3 targeterUpDirection)
        {
            // Zig-zag order
            
            Vector3 right = Vector3.Cross(toTargetDirection, targeterUpDirection).normalized;
            
            int numberOfRays = NumberOfLineOfSightRays;
            int halfStepCount = numberOfRays / 2;
            int stepCount = halfStepCount * 2;
            float halfLineOfSighRaysDistanceRange = stepCount > 0 ? LineOfSighRaysDistanceRange / 2 : 0;
            float step = stepCount > 0 ? LineOfSighRaysDistanceRange / (numberOfRays-1) : 0f;
            
            for (int i = 0; i < numberOfRays; ++i)
            {
                Vector3 lateralDisplacement = right * ((i % 2 == 0 ? -1 : 1) * (halfLineOfSighRaysDistanceRange - (step * (int)(i / 2))));
                Vector3 origin = targeterPosition + lateralDisplacement;
                Vector3 end = autoAimTarget.Position + lateralDisplacement;
                Vector3 originToEnd = (end - origin);

                if (Physics.Raycast(origin, originToEnd.normalized, out RaycastHit hit, originToEnd.magnitude, 
                        LayerMask, QueryTriggerInteraction))
                {
                    if (hit.collider.gameObject != autoAimTarget.GameObject)
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
        
    }
}