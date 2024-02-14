using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGround.GroundPhysics
{
    public class SafeGroundPhysicsChecker : ISafeGroundChecker
    {
        private readonly Transform _floorProbingOrigin;
        
        
        
        public Vector3 LastSafePosition { get; private set; }

        private Vector3 FloorProbingOriginPosition => _floorProbingOrigin.position;



        public SafeGroundPhysicsChecker(Transform floorProbingOrigin)
        {
            _floorProbingOrigin = floorProbingOrigin;
        }
        
        
        public void UpdateChecking(float deltaTime)
        {
            throw new System.NotImplementedException();
        }
        
        
        
        
    }
}