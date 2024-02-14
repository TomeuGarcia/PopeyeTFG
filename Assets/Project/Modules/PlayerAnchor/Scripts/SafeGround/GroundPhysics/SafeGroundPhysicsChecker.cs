using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGround
{
    public class SafeGroundPhysicsChecker : ISafeGroundChecker
    {
        private readonly Transform positionTrackingTransform;
        private readonly SafeGroundCheckerConfig _safeGroundCheckerConfig;
        
        
        
        public Vector3 LastSafePosition { get; private set; }
        
        private LayerMask GroundCollisionLayerMask => _safeGroundCheckerConfig.GroundCollisionLayerMask;
        private QueryTriggerInteraction GroundQueryTriggerInteraction => _safeGroundCheckerConfig.GroundQueryTriggerInteraction;
        private float GroundProbeDistance => _safeGroundCheckerConfig.GroundProbeDistance;
        private Vector3 ProbeOriginLocalOffset => _safeGroundCheckerConfig.ProbeOriginLocalOffset;
        private float ProbeSize => _safeGroundCheckerConfig.ProbeSize;


        public SafeGroundPhysicsChecker(Transform positionTrackingTransform, SafeGroundCheckerConfig safeGroundCheckerConfig)
        {
            this.positionTrackingTransform = positionTrackingTransform;
            _safeGroundCheckerConfig = safeGroundCheckerConfig;
        }
        
        
        public void UpdateChecking(float deltaTime)
        {
            throw new System.NotImplementedException();
        }


        private void Check()
        {
            Vector3 origin = positionTrackingTransform.position;
            origin += ProbeOriginLocalOffset.x * positionTrackingTransform.right;
            origin += ProbeOriginLocalOffset.y * positionTrackingTransform.up;
            origin += ProbeOriginLocalOffset.z * positionTrackingTransform.forward;

            float radius = ProbeSize / 2;
            if (Physics.SphereCast(origin, radius, Vector3.down, out RaycastHit groundHit, 
                    GroundProbeDistance, GroundCollisionLayerMask))
            {
                LastSafePosition = positionTrackingTransform.position;
            }
        }
        
        
        
        
        
    }
}