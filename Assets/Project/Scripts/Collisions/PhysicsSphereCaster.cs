using UnityEngine;

namespace Popeye.Scripts.Collisions
{
    public class PhysicsSphereCaster : IPhysicsCaster
    {
        private readonly CastComputer _castComputer;
        private readonly CollisionProbingConfig _probingConfig;
        private readonly float _radius;

        public PhysicsSphereCaster(CastComputer castComputer, CollisionProbingConfig probingConfig, float radius)
        {
            _castComputer = castComputer;
            _probingConfig = probingConfig;
            _radius = radius;
        }

        public bool CheckHit(out RaycastHit hit)
        {
            return Physics.SphereCast(_castComputer.ComputeCastOrigin(), _radius, 
                _castComputer.ComputeCastDirection(), out hit, 
                _probingConfig.ProbeDistance, _probingConfig.CollisionLayerMask, _probingConfig.QueryTriggerInteraction);
        }
    }
}