using UnityEngine;

namespace Popeye.Scripts.Collisions
{
    public class PhysicsRayCaster : IPhysicsCaster
    {
        private readonly CastComputer _castComputer;
        private readonly CollisionProbingConfig _probingConfig;

        public PhysicsRayCaster(CastComputer castComputer, CollisionProbingConfig probingConfig)
        {
            _castComputer = castComputer;
            _probingConfig = probingConfig;
        }
        
        
        public bool CheckHit(out RaycastHit hit)
        {
            return Physics.Raycast(_castComputer.ComputeCastOrigin(), _castComputer.ComputeCastDirection(), out hit, 
                _probingConfig.ProbeDistance, _probingConfig.CollisionLayerMask, _probingConfig.QueryTriggerInteraction);
        }

        
    }
}