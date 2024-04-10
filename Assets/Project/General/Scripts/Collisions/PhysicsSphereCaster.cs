using UnityEngine;

namespace Popeye.Scripts.Collisions
{
    public class PhysicsSphereCaster : IPhysicsCaster
    {
        private readonly ICastComputer _castComputer;
        private readonly CollisionProbingConfig _probingConfig;
        private readonly float _radius;
        private readonly PhysicsCastRequirementsProcessor _requirementsProcessor;

        public PhysicsSphereCaster(ICastComputer castComputer, CollisionProbingConfig probingConfig,
            PhysicsCastRequirementsProcessor requirementsProcessor, float radius)
        {
            _castComputer = castComputer;
            _probingConfig = probingConfig;
            _radius = radius;
            _requirementsProcessor = requirementsProcessor;
        }

        public bool CheckHit(out RaycastHit hit)
        {
            return DoCheckHit(out hit, _castComputer.ComputeCastOrigin(), _castComputer.ComputeCastDirection());
        }

        public bool CheckHitAtPosition(out RaycastHit hit, Vector3 castOriginPosition)
        {
            return DoCheckHit(out hit, castOriginPosition, _castComputer.ComputeCastDirection());
        }
        private bool DoCheckHit(out RaycastHit hit, Vector3 origin, Vector3 direction)
        {
            if (Physics.SphereCast(origin, _radius, direction, out hit, _probingConfig.ProbeDistance,
                    _probingConfig.CollisionLayerMask, _probingConfig.QueryTriggerInteraction))
            {
                return _requirementsProcessor.HitMeetsAllRequirements(hit);
            }

            return false;
        }
    }
}