using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGround
{
    public class SafeGroundPhysicsChecker : ISafeGroundChecker
    {
        private readonly Transform _floorProbingOrigin;
        private readonly SafeGroundCheckerConfig _safeGroundCheckerConfig;
        
        
        
        public Vector3 LastSafePosition { get; private set; }
        
        private LayerMask CollisionLayerMask => _safeGroundCheckerConfig.GroundCollisionLayerMask;
        private QueryTriggerInteraction QueryTriggerInteraction => _safeGroundCheckerConfig.GroundQueryTriggerInteraction;
        private float ProbeDistance => _safeGroundCheckerConfig.GroundProbeDistance;
        private Vector3 ProbeOriginLocalOffset => _safeGroundCheckerConfig.ProbeOriginLocalOffset;
        private float ProbeSize => _safeGroundCheckerConfig.ProbeSize;


        public SafeGroundPhysicsChecker(Transform floorProbingOrigin, SafeGroundCheckerConfig safeGroundCheckerConfig)
        {
            _floorProbingOrigin = floorProbingOrigin;
            _safeGroundCheckerConfig = safeGroundCheckerConfig;
        }
        
        
        public void UpdateChecking(float deltaTime)
        {
            throw new System.NotImplementedException();
        }


        private void Check()
        {
            Vector3 origin = _floorProbingOrigin.position;
            origin += ProbeOriginLocalOffset.x * _floorProbingOrigin.right;
            origin += ProbeOriginLocalOffset.y * _floorProbingOrigin.up;
            origin += ProbeOriginLocalOffset.z * _floorProbingOrigin.forward;

            
            //Physics.SphereCast()
        }
        
        
        
        
        
    }
}